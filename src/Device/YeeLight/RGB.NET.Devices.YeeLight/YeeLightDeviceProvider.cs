using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RGB.NET.Core;
using YeelightAPI;

namespace RGB.NET.Devices.YeeLight
{
    public class YeeLightDeviceProvider : AbstractRGBDeviceProvider
    {
        private static YeeLightDeviceProvider? _instance;
        private List<IRGBDevice> _registeredDevices = new List<IRGBDevice>();
        private IEnumerable<Device>? _yeeLightDevices;

        public static YeeLightDeviceProvider Instance => _instance ?? new YeeLightDeviceProvider();

        public YeeLightDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(YeeLightDeviceProvider)}");
            _instance = this;
        }
        protected override IEnumerable<IRGBDevice> LoadDevices()
        {
            GetDevicesAsync().Wait();
            return _registeredDevices;
        }

        private async Task GetDevicesAsync()
        {
            var progressReporter = new Progress<Device>(OnDeviceFound);
            DeviceLocator.UseAllAvailableMulticastAddresses = true;
            await DeviceLocator.DiscoverAsync(progressReporter);
        }

        private void OnDeviceFound(Device device)
        {
            try
            {
                device.Connect().Wait();
                _registeredDevices.Add(new YeeLightRGBRGBDevice(new YeeLightRGBDeviceInfo(RGBDeviceType.LedStripe, string.Format("YeeLight {0} ({1})", device.Model, device.Hostname), device.Hostname), new YeeLightUpdateQueue(GetUpdateTrigger(), device)));
            }
            catch
            {
                //TODO: Log
            }
        }

        protected override void InitializeSDK() { }

        public override void Dispose()
        {
            base.Dispose();
            if (_yeeLightDevices != null)
                foreach (Device device in _yeeLightDevices)
                {
                    device?.Disconnect();
                    device?.Dispose();
                }
            _yeeLightDevices = null;
            _registeredDevices.Clear();
        }
    }
}
