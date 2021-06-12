using System;
using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Devices.YeeLight.Enums;
using YeelightAPI;

namespace RGB.NET.Devices.YeeLight
{
    public class YeeLightDeviceProvider : AbstractRGBDeviceProvider
    {
        private static YeeLightDeviceProvider? _instance;
        private List<IRGBDevice> _registeredDevices = new List<IRGBDevice>();
        private IEnumerable<Device>? _yeeLightDevices;
        public bool UseAllAvailableMulticastAddresses { get; set; }
        public ScanMode ScanMode { get; set; }
        public List<YeeLightDeviceDefinition> YeeLightDeviceDefinitions { get; set; }

        public static YeeLightDeviceProvider Instance => _instance ?? new YeeLightDeviceProvider();

        public YeeLightDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(YeeLightDeviceProvider)}");
            _instance = this;
        }
        protected override IEnumerable<IRGBDevice> LoadDevices()
        {
            DeviceLocator.UseAllAvailableMulticastAddresses = UseAllAvailableMulticastAddresses;

            if (ScanMode == ScanMode.Automatic)
            {

                _yeeLightDevices = DeviceLocator.DiscoverAsync().GetAwaiter().GetResult();

                foreach (var device in _yeeLightDevices)
                {
                    bool add = false;
                    try
                    {
                        device.Connect().Wait();
                        add = device.IsConnected;
                    }
                    catch
                    {
                        // LOG
                        add = false;
                    }

                    if (add)
                        yield return new YeeLightRGBRGBDevice(new YeeLightRGBDeviceInfo(RGBDeviceType.LedStripe, string.Format("YeeLight {0} ({1})", device.Model, device.Hostname), device.Hostname), new YeeLightUpdateQueue(GetUpdateTrigger(), device, device.Model));
                }
            }
            else if (YeeLightDeviceDefinitions != null)
            {
                foreach (YeeLightDeviceDefinition def in YeeLightDeviceDefinitions)
                {
                    Device device = new Device(def.HostName);
                    device.Name = def.DeviceName;
                    bool add = false;
                    try
                    {
                        device.Connect().Wait();
                        add = device.IsConnected;
                    }
                    catch
                    {
                        // LOG
                        add = false;
                    }

                    if (add)
                        yield return new YeeLightRGBRGBDevice(new YeeLightRGBDeviceInfo(RGBDeviceType.LedStripe, string.Format("YeeLight {0} ({1})", def.Model, device.Hostname), device.Hostname), new YeeLightUpdateQueue(GetUpdateTrigger(), device, def.Model));

                }
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
            YeeLightDeviceDefinitions?.Clear();
            _registeredDevices?.Clear();
        }
    }
}
