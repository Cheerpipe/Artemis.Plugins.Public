using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RGB.NET.Core;
using RGB.NET.Devices.YeeLight.Generic;
using RGB.NET.Devices.YeeLight.Devices;
using YeelightAPI;
// ReSharper disable AsyncConverter.AsyncWait
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

namespace RGB.NET.Devices.YeeLight
{
    public class YeeLightDeviceProvider : AbstractRGBDeviceProvider
    {
        private const int YeelightConnectionTimeout = 3000;
        private static YeeLightDeviceProvider? _instance;
        private List<Device>? _discoveredYeeLightDevices;
        public bool UseAllAvailableMulticastAddresses { get; set; }
        public bool UseAutomaticScan  { get; set; }
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

            if (UseAutomaticScan)
            {
                _discoveredYeeLightDevices = DeviceLocator.DiscoverAsync().GetAwaiter().GetResult().ToList();

                Parallel.ForEach(_discoveredYeeLightDevices, device =>
                {
                    var add = false;
                    try
                    {
                        if (device.Connect().Wait(TimeSpan.FromMilliseconds(YeelightConnectionTimeout)))
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
                        _IRGBDevices.Add(new YeeLightDevice(new YeeLightRGBDeviceInfo(RGBDeviceType.LedStripe,
                            $"YeeLight {device.Model} ({device.Hostname})", device.Hostname), new YeeLightUpdateQueue(GetUpdateTrigger(), device)));
                    }

                });
            }
            else if (YeeLightDeviceDefinitions != null)
            {
                Parallel.ForEach(YeeLightDeviceDefinitions, def =>
                {
                    Device device = new Device(def.HostName);
                    var add = false;
                    try
                    {
                        if (device.Connect().Wait(TimeSpan.FromMilliseconds(YeelightConnectionTimeout)))
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
                    _initializedDevices.Add(device);
                    _IRGBDevices.Add(new YeeLightDevice(new YeeLightRGBDeviceInfo(RGBDeviceType.LedStripe,
                        $"YeeLight {def.Model} ({device.Hostname})", device.Hostname),
                        def.UseMusicMode ? new YeeLightMusicModeUpdateQueue(GetUpdateTrigger(), device) : new YeeLightUpdateQueue(GetUpdateTrigger(), device)
                        ));
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
                device.TurnOff();
                device.Disconnect();
                device.Dispose();
            }
            _initializedDevices.Clear();
            YeeLightDeviceDefinitions?.Clear();
            _discoveredYeeLightDevices?.Clear();
        }
    }
}
