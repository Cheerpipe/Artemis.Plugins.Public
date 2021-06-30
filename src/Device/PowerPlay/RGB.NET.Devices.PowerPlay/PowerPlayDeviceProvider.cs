using System;
using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Devices.PowerPlay.Device;
using RGB.NET.Devices.PowerPlay.PowerPlay;

// ReSharper disable AsyncConverter.AsyncWait

namespace RGB.NET.Devices.PowerPlay
{
    public class PowerPlayDeviceProvider : AbstractRGBDeviceProvider
    {
        private static PowerPlayDeviceProvider? _instance;

        public static PowerPlayDeviceProvider Instance => _instance ?? new PowerPlayDeviceProvider();

        public PowerPlayDeviceProvider()
        {
            if (_instance != null)
            {
                throw new InvalidOperationException($"There can be only one instance of type {nameof(PowerPlayDeviceProvider)}");
            }

            _instance = this;
        }
        protected override IEnumerable<IRGBDevice> LoadDevices()
        {
            PowerPlayLoader.InitializeAsync().Wait();

            if (PowerPlayLoader.IsInitialized)
            {
                PowerPlayUpdateQueue mouseUpdateQueue = new PowerPlayUpdateQueue(GetUpdateTrigger(0), PowerPlayLoader.MouseController);
                yield return new PowerPlayRgbDevice(new PowerPlayRgbDeviceInfo("Powerplay Lightspeed Mouse", RGBDeviceType.Mouse), mouseUpdateQueue, 2);

                PowerPlayUpdateQueue matUpdateQueue = new PowerPlayUpdateQueue(GetUpdateTrigger(0), PowerPlayLoader.MatController);
                yield return new PowerPlayRgbDevice(new PowerPlayRgbDeviceInfo("Powerplay Mousepad", RGBDeviceType.Mousepad), matUpdateQueue, 1);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            PowerPlayLoader.FreeDevices();
        }
        protected override void InitializeSDK() { }
    }
}
