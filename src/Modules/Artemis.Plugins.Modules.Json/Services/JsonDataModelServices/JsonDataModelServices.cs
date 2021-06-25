using System;
using System.Collections.Generic;
using Artemis.Core;
using Artemis.Core.Modules;
using Artemis.Core.Services;
using Artemis.Plugins.Modules.Json.DataModels;
using Serilog;
// ReSharper disable FlagArgument

namespace Artemis.Plugins.Modules.Json.Services.JsonDataModelServices
{
    public class JsonDataModelServices : IPluginService
    {
        private readonly PluginSettings _pluginSettings;
        private readonly ILogger _logger;
        private JsonDataModel _jsonDataModel;
        private PluginSetting<Dictionary<string, string>> _savedJsonDynamicDataSetting;

        public JsonDataModelServices(PluginSettings pluginSettings, ILogger logger)
        {
            _pluginSettings = pluginSettings;
            _logger = logger;
        }

        public void Initialize(JsonDataModel jsonDataModel)
        {
            _jsonDataModel = jsonDataModel;
            _savedJsonDynamicDataSetting = _pluginSettings.GetSetting("SavedJsonDynamicDataSetting", new Dictionary<string, string>());
        }

        public bool TryGetJsonByKey(string key, out string json)
        {
            if (_savedJsonDynamicDataSetting.Value.TryGetValue(key, out json))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddOrMergeJson(string key, string payload, bool saveToRepository = false)
        {
            var newJsonDataModel = CreateJsonDataModelBridge(payload);

            if (newJsonDataModel == null)
                return false;

            JsonDataModelBridge currentJsonDataModel = null;
            if (_jsonDataModel.TryGetDynamicChild(key, out DynamicChild<DataModel> child))
            {
                currentJsonDataModel = new JsonDataModelBridge(_savedJsonDynamicDataSetting.Value[key]);
                currentJsonDataModel.Merge(newJsonDataModel);
                child.Value = currentJsonDataModel.DataModel;
            }
            else
            {
                _jsonDataModel.AddDynamicChild(key, newJsonDataModel.DataModel);
            }

            if (saveToRepository)
                SaveToRepository(key, currentJsonDataModel != null ? currentJsonDataModel.JObject.ToString() : newJsonDataModel.JObject.ToString());

            return true;
        }

        public void SaveToRepository(string key, string json)
        {
            _savedJsonDynamicDataSetting.Value[key] = json;
            _savedJsonDynamicDataSetting.Value = _savedJsonDynamicDataSetting.Value;
            _savedJsonDynamicDataSetting.Save();
        }

        public bool AddOrReplaceJson(string key, string payload, bool saveToRepository = false)
        {
            var jsonDataModel = CreateJsonDataModelBridge(payload);

            if (jsonDataModel == null)
                return false;

            if (_jsonDataModel.TryGetDynamicChild(key, out DynamicChild<DataModel> child))
            {
                child.Value = jsonDataModel.DataModel;
            }
            else
            {
                _jsonDataModel.AddDynamicChild(key, jsonDataModel.DataModel);
            }

            if (saveToRepository)
                SaveToRepository(key, jsonDataModel.JObject.ToString());
            return true;
        }

        public bool RemoveByKey(string key)
        {
            _jsonDataModel.RemoveDynamicChildByKey(key);
            bool result = _savedJsonDynamicDataSetting.Value.Remove(key);
            _savedJsonDynamicDataSetting.Save();
            return result;
        }

        public static JsonDataModelBridge CreateJsonDataModelBridge(string payload)
        {
            return new(payload);
        }

        public void LoadFromRepository()
        {
            foreach (var (key, value) in _savedJsonDynamicDataSetting.Value)
            {
                try
                {
                    AddOrReplaceJson(key, value);
                }
                catch (Exception e)
                {
                    _logger.Warning($"Json DataModel {key} couldn't be loaded. \r\nException: {e}");
                }
            }
        }
    }
}
