using System;
using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Devices.LogitechCustom.Device;
using RGB.NET.Devices.LogitechCustom.LogitechCustom;

namespace RGB.NET.Devices.LogitechCustom
{
    public class LogitechCustomDeviceProvider : AbstractRGBDeviceProvider
    {
        private static LogitechCustomDeviceProvider? _instance;

        public static LogitechCustomDeviceProvider Instance => _instance ?? new LogitechCustomDeviceProvider();

        public LogitechCustomDeviceProvider()
        {
            if (_instance != null)
            {
                throw new InvalidOperationException($"There can be only one instance of type {nameof(LogitechCustomDeviceProvider)}");
            }

            _instance = this;
        }
        protected override IEnumerable<IRGBDevice> LoadDevices()
        {
            LogitechCustomLoader.InitializeAsync().Wait();

            if (LogitechCustomLoader.IsInitialized)
            {
                LogitechCustomUpdateQueue matUpdateQueue = new LogitechCustomUpdateQueue(GetUpdateTrigger(0), LogitechCustomLoader.MatController);
                yield return new LogitechCustomRgbDevice(new LogitechCustomRgbDeviceInfo("Logitech Mousepad", RGBDeviceType.Mousepad), matUpdateQueue, 1);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            LogitechCustomLoader.FreeDevices();
        }
        protected override void InitializeSDK() { }
    }
}
