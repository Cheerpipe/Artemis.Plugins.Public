using Artemis.Core;
using Artemis.Core.Modules;
using Artemis.Core.Services;
using Artemis.Plugins.Module.DynamicExternalDataModule.DataModels;
using Artemis.UI.Services;
using System;
using System.Collections.Generic;

namespace Artemis.Plugins.Module.DynamicExternalDataModule
{
    public class DesktopVariablesDataModelExpansion : Module<DynamicExternalDataModuleDataModel>
    {
        private readonly IWebServerService _webServerService;
        private readonly PluginSettings _pluginSettings;
        private Dictionary<string, KeyValuePair<object, Type>> _savedDynamicData;
        private PluginSetting<Dictionary<string, KeyValuePair<object, Type>>> _savedDynamicDataSetting;
        public DesktopVariablesDataModelExpansion(IWebServerService webServerService, IDebugService _debugService, PluginSettings pluginSettings)
        {
            _webServerService = webServerService;
            _pluginSettings = pluginSettings;
        }

        public override List<IModuleActivationRequirement> ActivationRequirements => null;

        public override void Enable()
        {
            _savedDynamicDataSetting = _pluginSettings.GetSetting("SavedDynamicDataSetting", new Dictionary<string, KeyValuePair<object, Type>>());
            _webServerService.AddStringEndPoint(this, "SetBoolValue", payload => ProcessValue<bool>(payload));
            _webServerService.AddStringEndPoint(this, "ToggleBoolValue", payload => ToggleBoolValue(payload, true));
            _webServerService.AddStringEndPoint(this, "SetIntValue", payload => ProcessValue<int>(payload));
            _webServerService.AddStringEndPoint(this, "SetFloatValue", payload => ProcessValue<float>(payload));
            _webServerService.AddStringEndPoint(this, "SetStringValue", payload => ProcessValue<string>(payload));
            _webServerService.AddStringEndPoint(this, "RemoveAll", payload => RemoveAll());
            LoadSavedValues();
        }

        private bool ProcessValue<T>(string payload)
        {
            if (!ParsePayload<T>(payload, out string key, out object value))
            {
                // Log
                return false;
            }
            bool setValueResult = SetValue<T>(key, value);
            bool saveValueResult = SaveValue<T>(key, value);
            return setValueResult;
        }

        private bool ToggleBoolValue(string key, bool defaultValue)
        {
            if (DataModel.TryGetDynamicChild<bool>(key, out DynamicChild<bool> child))
            {
                child.Value = !child.Value;
            }
            {
                DataModel.AddDynamicChild<bool>(key, defaultValue);
            }
            return SaveValue<bool>(key, child.Value);
        }

        private bool SetValue<T>(string key, object value)
        {
            if (value is not T)
            {
                //Log
                return false;
            }

            if (DataModel.TryGetDynamicChild<T>(key, out DynamicChild<T> child))
            {
                child.Value = (T)value;
            }
            else
            {
                DataModel.AddDynamicChild<T>(key, (T)value);
            }
            return true;
        }

        private bool SaveValue<T>(string key, object value)
        {
            _savedDynamicData = _savedDynamicDataSetting.Value;
            _savedDynamicData[key] = new KeyValuePair<object, Type>(value, typeof(T));
            _savedDynamicDataSetting.Value = _savedDynamicData;
            _savedDynamicDataSetting.Save();
            return true;
        }

        private void LoadSavedValues()
        {
            _savedDynamicData = _savedDynamicDataSetting.Value;

            foreach (var savedData in _savedDynamicData)
            {
                switch (Type.GetTypeCode(savedData.Value.Value))
                {
                    case (TypeCode.Boolean):
                        SetValue<bool>(savedData.Key, (bool)savedData.Value.Key);
                        break;
                    case (TypeCode.Int32):
                        SetValue<int>(savedData.Key, (int)savedData.Value.Key);
                        break;
                    case (TypeCode.Single):
                        SetValue<float>(savedData.Key, (float)savedData.Value.Key);
                        break;
                    case (TypeCode.String):
                        SetValue<string>(savedData.Key, (string)savedData.Value.Key);
                        break;
                }
            }
        }

        private void RemoveAll()
        {
            _savedDynamicData = _savedDynamicDataSetting.Value;
            _savedDynamicDataSetting.Value.Clear();
            _savedDynamicDataSetting.Save();
            DataModel.ClearDynamicChildren();
        }

        public bool ParsePayload<T>(string payload, out string key, out object value)
        {
            key = string.Empty;
            value = null;
            string[] data = payload.Split(':');
            if (data.Length != 2)
            {
                return false;
            }
            key = data[0];
            value = (T)Convert.ChangeType(data[1], typeof(T));
            return true;
        }
        public override void Disable() { }
        public override void Update(double deltaTime) { }
    }
}