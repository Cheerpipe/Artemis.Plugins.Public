using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RGB.NET.Core;
using RGB.NET.Devices.YeeLight.Enums;
using YeelightAPI;

namespace RGB.NET.Devices.YeeLight
{
    public class YeeLightDeviceProvider : AbstractRGBDeviceProvider
    {
        private const int YEELIGHT_CONNECTION_TIMEOUT = 3000;
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
            List<IRGBDevice> _IRGBDevices = new List<IRGBDevice>();

            // TODO Connect async
            if (ScanMode == ScanMode.Automatic)
            {
                _discoveredYeeLightDevices = DeviceLocator.DiscoverAsync().GetAwaiter().GetResult().ToList();

                Parallel.ForEach(_discoveredYeeLightDevices, device =>
                {
                    bool add = false;
                    try
                    {
                        if (device.Connect().Wait(TimeSpan.FromMilliseconds(YEELIGHT_CONNECTION_TIMEOUT)))
                        {
                            add = device.IsConnected;
                        }
                    }
                    catch
                    {
                        // LOG
                    }

                    if (add)
                    {
                        _initializedDevices.Add(device);
                        _IRGBDevices.Add(new YeeLightRGBRGBDevice(new YeeLightRGBDeviceInfo(RGBDeviceType.LedStripe, string.Format("YeeLight {0} ({1})", device.Model, device.Hostname), device.Hostname, false), new YeeLightUpdateQueue(GetUpdateTrigger(), device, device.Model)));
                    }

                });
            }
            else if (YeeLightDeviceDefinitions != null)
            {
                Parallel.ForEach(YeeLightDeviceDefinitions, def =>
                {
                    Device device = new Device(def.HostName);
                    bool add = false;
                    try
                    {
                        if (device.Connect().Wait(TimeSpan.FromMilliseconds(YEELIGHT_CONNECTION_TIMEOUT)))
                        {
                            device.Connect().Wait();
                            device.Name = def.DeviceName;
                            add = device.IsConnected;
                        }
                    }
                    catch
                    {
                        // LOG
                    }

                    if (add)
                    {
                        _initializedDevices.Add(device);
                        _IRGBDevices.Add(new YeeLightRGBRGBDevice(new YeeLightRGBDeviceInfo(RGBDeviceType.LedStripe, string.Format("YeeLight {0} ({1})", def.Model, device.Hostname), device.Hostname, false), new YeeLightUpdateQueue(GetUpdateTrigger(), device, def.Model)));
                    }
                    else
                    {
                        _IRGBDevices.Add(new YeeLightRGBRGBDevice(new YeeLightRGBDeviceInfo(RGBDeviceType.LedStripe, string.Format("YeeLight {0} ({1})", def.Model, device.Hostname), device.Hostname, true), new YeeLightUpdateQueue(GetUpdateTrigger(), device, def.Model, true)));
                    }
                });
            }

            return _IRGBDevices;
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
