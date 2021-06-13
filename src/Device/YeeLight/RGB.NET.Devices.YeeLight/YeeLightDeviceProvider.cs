using System;
using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Devices.YeeLight.Enums;
using YeelightAPI;

namespace RGB.NET.Devices.YeeLight
{
    public class YeeLightDeviceProvider : AbstractRGBDeviceProvider
    {
        private static YeeLightDeviceProvider? _instance;
        private List<Device>? _discoveredYeeLightDevices;
        public bool UseAllAvailableMulticastAddresses { get; set; }
        public ScanMode ScanMode { get; set; }
        public List<YeeLightDeviceDefinition>? YeeLightDeviceDefinitions { get; set; }

        public static YeeLightDeviceProvider Instance => _instance ?? new YeeLightDeviceProvider();
        private readonly List<Device> _initializedDevices = new();

        public YeeLightDeviceProvider()
        {
            if (_instance != null)
            {
                throw new InvalidOperationException($"There can be only one instance of type {nameof(YeeLightDeviceProvider)}");
            }

            _instance = this;
        }
        protected override IEnumerable<IRGBDevice> LoadDevices()
        {
            DeviceLocator.UseAllAvailableMulticastAddresses = UseAllAvailableMulticastAddresses;
            // TODO Connect async
            if (ScanMode == ScanMode.Automatic)
            {

                _discoveredYeeLightDevices = DeviceLocator.DiscoverAsync().GetAwaiter().GetResult().ToList();

                foreach (var device in _discoveredYeeLightDevices)
                {
                    bool add;
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
                    {
                        _initializedDevices.Add(device);
                        yield return new YeeLightRGBRGBDevice(new YeeLightRGBDeviceInfo(RGBDeviceType.LedStripe, string.Format("YeeLight {0} ({1})", device.Model, device.Hostname), device.Hostname), new YeeLightUpdateQueue(GetUpdateTrigger(), device, device.Model));
                    }

                }
            }
            else if (YeeLightDeviceDefinitions != null)
            {
                foreach (YeeLightDeviceDefinition def in YeeLightDeviceDefinitions)
                {
                    Device device = new Device(def.HostName);
                    bool add;
                    try
                    {
                        device.Connect().Wait();
                        device.Name = def.DeviceName;
                        add = device.IsConnected;
                    }
                    catch
                    {
                        // LOG
                        add = false;
                    }

                    if (add)
                    {
                        _initializedDevices.Add(device);
                        yield return new YeeLightRGBRGBDevice(new YeeLightRGBDeviceInfo(RGBDeviceType.LedStripe, string.Format("YeeLight {0} ({1})", def.Model, device.Hostname), device.Hostname), new YeeLightUpdateQueue(GetUpdateTrigger(), device, def.Model));
                    }
                }
            }
        }

        protected override void InitializeSDK() { }

        public override void Dispose()
        {
            base.Dispose();

            foreach (Device device in _initializedDevices)
            {
                device?.TurnOff();
                device?.Disconnect();
                device?.Dispose();
            }
            _initializedDevices?.Clear();
            YeeLightDeviceDefinitions?.Clear();
            _discoveredYeeLightDevices?.Clear();
        }
    }
}
