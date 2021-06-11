using Artemis.Core.DataModelExpansions;
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
        public DesktopVariablesDataModelExpansion(IWebServerService webServerService, IDebugService _debugService)
        {
            _webServerService = webServerService;
        }

        public override List<IModuleActivationRequirement> ActivationRequirements => null;

        public override void Enable()
        {
            _webServerService.AddStringEndPoint(this, "SetBoolValue", value => SetBoolValue(value));
            _webServerService.AddStringEndPoint(this, "SetIntValue", value => SetIntValue(value));
            _webServerService.AddStringEndPoint(this, "SetFloatValue", value => SetFloatValue(value));
            _webServerService.AddStringEndPoint(this, "SetStringValue", value => SetStringValue(value));
        }

        private void SetBoolValue(string payload)
        {
            if (!ParsePayload(payload, out string key, out string stringValue))
            {
                // Log
                return;
            }

            if (bool.TryParse(stringValue, out bool boolValue))
            {
                AddOrReplaceValue(key, boolValue);
            }
        }


        private void SetIntValue(string payload)
        {
            if (!ParsePayload(payload, out string key, out string stringValue))
            {
                // Log
                return;
            }

            if (int.TryParse(stringValue, out int intValue))
            {
                AddOrReplaceValue(key, intValue);
            }
        }

        private void SetFloatValue(string payload)
        {
            if (!ParsePayload(payload, out string key, out string stringValue))
            {
                // Log
                return;
            }

            if (float.TryParse(stringValue, out float floatValue))
            {
                AddOrReplaceValue(key, floatValue);
            }
        }

        private void SetStringValue(string payload)
        {
            if (!ParsePayload(payload, out string key, out string stringValue))
            {
                // Log
                return;
            }
                AddOrReplaceValue(key, stringValue);
        }


        public void AddOrReplaceValue(string key, object? value)
        {
            DataModel.RemoveDynamicChildByKey(key);
            DataModel.AddDynamicChild(key, value);
        }

        public bool ParsePayload(string payload, out string key, out string stringValue)
        {
            key = string.Empty;
            stringValue = string.Empty;
            string[] data = payload.Split(':');
            if (data.Length != 2)
            {
                return false;
            }
            key = data[0];
            stringValue = data[1];
            return true;
        }

        public override void Disable() { }
        public override void Update(double deltaTime) { }
    }
}