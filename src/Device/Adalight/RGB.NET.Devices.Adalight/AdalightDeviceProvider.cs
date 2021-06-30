using System;
using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.PowerPlay
{
    public class PowerPlayDeviceProvider : AbstractRGBDeviceProvider
    {
        private static AdalightDeviceProvider? _instance;

        public static AdalightDeviceProvider Instance => _instance ?? new AdalightDeviceProvider();
        public List<AdalightDeviceDefinition> AdalightDeviceDefinitions { get; } = new();
        public AdalightDeviceProvider()
        {
            if (_instance != null)
            {
                throw new InvalidOperationException($"There can be only one instance of type {nameof(AdalightDeviceProvider)}");
            }

            _instance = this;
        }
        protected override IEnumerable<IRGBDevice> LoadDevices()
        {
            int i = 0;
            foreach (AdalightDeviceDefinition deviceDefinition in AdalightDeviceDefinitions)
            {
                IDeviceUpdateTrigger updateTrigger = GetUpdateTrigger(i++);
                foreach (IRGBDevice device in deviceDefinition.CreateDevices(updateTrigger))
                    yield return device;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            AdalightDeviceDefinitions.Clear();
        }
        public void AddDeviceDefinition(AdalightDeviceDefinition deviceDefinition) => AdalightDeviceDefinitions.Add(deviceDefinition);


        protected override void InitializeSDK() { }
    }
}
