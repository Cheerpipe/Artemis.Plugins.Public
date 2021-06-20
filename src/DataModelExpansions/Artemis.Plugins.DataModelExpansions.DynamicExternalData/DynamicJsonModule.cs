using Artemis.Core;
using Artemis.Core.Modules;
using Artemis.Core.Services;
using Artemis.Plugins.DataModelExpansions.DynamicExternalData;
using Artemis.Plugins.DataModelExpansions.DynamicExternalDataModelExpansions.DataModels;
using Artemis.UI.Services;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Artemis.Plugins.DataModelExpansions.DynamicExternalDataModelExpansions
{
    public class DynamicJsonModule : Module<DynamicJsonDataModel>
    {
        private readonly IWebServerService _webServerService;
        private readonly ILogger _logger;
        private readonly PluginSettings _pluginSettings;
        private Dictionary<string, string> _savedJsonDynamicData;
        private PluginSetting<Dictionary<string, string>> _savedJsonDynamicDataSetting;

        public DynamicJsonModule(IWebServerService webServerService, IDebugService _debugService, PluginSettings pluginSettings, ILogger logger)
        {
            _webServerService = webServerService;
            _pluginSettings = pluginSettings;
            _logger = logger;
        }

        public override List<IModuleActivationRequirement> ActivationRequirements => null;

        public override void Enable()
        {
            _savedJsonDynamicDataSetting = _pluginSettings.GetSetting("SavedJsonDynamicDataSetting", new Dictionary<string, string>());
            LoadFromRepository();
            _webServerService.AddStringEndPoint(this, "AddOrReplace", json => AddOrReplace(json, true));
            _webServerService.AddStringEndPoint(this, "AddOrMerge", json => AddOrCombine(json, true));
            _webServerService.AddStringEndPoint(this, "RemoveByKey", key => Remove(key));
            _webServerService.AddResponsiveStringEndPoint(this, "GetByKey", key =>
            {
                if (_savedJsonDynamicData.TryGetValue(key, out string json))
                {
                    return json;
                }
                return string.Empty;
            });
        }

        private void Remove(string key)
        {
            _savedJsonDynamicDataSetting.Value.Remove(key);
        }

        private void AddOrCombine(string payload, bool saveToRepository = false)
        {
            JsonDataModelBridge newJsonDataModel = CreateJsonDataModelBridge(payload);

            if (newJsonDataModel == null)
                return;

            JsonDataModelBridge currentJsonDataModel = null;
            if (DataModel.TryGetDynamicChild(newJsonDataModel.Key, out DynamicChild<DataModel> child))
            {
                currentJsonDataModel = new JsonDataModelBridge(_savedJsonDynamicData[newJsonDataModel.Key]);
                currentJsonDataModel.Merge(newJsonDataModel);
                child.Value = currentJsonDataModel.DataModel;
            }
            else
            {
                DataModel.AddDynamicChild(newJsonDataModel.Key, newJsonDataModel.DataModel);
            }

            if (saveToRepository && currentJsonDataModel != null)
                SaveToRepository(currentJsonDataModel.Key, currentJsonDataModel.JObject.ToString());
            else if (saveToRepository && newJsonDataModel != null)
                SaveToRepository(newJsonDataModel.Key, newJsonDataModel.JObject.ToString());
        }

        private void AddOrReplace(string payload, bool saveToRepository = false)
        {

            JsonDataModelBridge jsonDataModel = CreateJsonDataModelBridge(payload);


            if (jsonDataModel == null)
                return;

            if (DataModel.TryGetDynamicChild(jsonDataModel.Key, out DynamicChild<DataModel> child))
            {
                child.Value = jsonDataModel.DataModel;
            }
            else
            {
                DataModel.AddDynamicChild(jsonDataModel.Key, jsonDataModel.DataModel);
            }

            if (saveToRepository)
                SaveToRepository(jsonDataModel.Key, jsonDataModel.JObject.ToString());
        }

        private JsonDataModelBridge CreateJsonDataModelBridge(string payload)
        {
            try
            {
                return new JsonDataModelBridge(payload);

            }
            catch (FormatException)
            {

                _logger.Warning($"Json payload don't have a correct format. Ensure the Root element has only one child of type object.");
            }
            catch (Exception e)
            {
                _logger.Warning($"Json payload was not loaded into datamodel.\r\nPayload: {payload} \r\nException: {e.ToString()}");
            }
            return null;

        }

        private void SaveToRepository(string key, string json)
        {
            _savedJsonDynamicData[key] = json;
            _savedJsonDynamicDataSetting.Value = _savedJsonDynamicData;
            _savedJsonDynamicDataSetting.Save();
        }

        private void LoadFromRepository()
        {
            _savedJsonDynamicData = _savedJsonDynamicDataSetting.Value;

            foreach (KeyValuePair<string, string> d in _savedJsonDynamicData)
            {
                try
                {
                    AddOrReplace(d.Value, false);
                }
                catch (Exception e)
                {
                    _logger.Warning($"Json DataModel {d.Key} couldn't be loaded. \r\nException: {e.ToString()}");
                }
            }
        }

        public override void Disable() { }
        public override void Update(double deltaTime) { }
    }
}