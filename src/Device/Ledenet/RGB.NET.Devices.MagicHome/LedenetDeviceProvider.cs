using System;
using System.Collections.Generic;
using System.Net;
using MagicHome;
using RGB.NET.Core;
using RGB.NET.Devices.Ledenet.Generic;
using RGB.NET.Devices.Ledenet.PerDevice;

namespace RGB.NET.Devices.Ledenet
{
    public class LedenetDeviceProvider : AbstractRGBDeviceProvider
    {
        private static LedenetDeviceProvider? _instance;
        private readonly List<Light> _initializedDevices = new();

        public static LedenetDeviceProvider Instance => _instance ?? new LedenetDeviceProvider();
        public List<LedenetDeviceDefinition>? LedenetDeviceDefinitions { get; set; }

        public LedenetDeviceProvider()
        {
            if (_instance != null)
            {
                throw new InvalidOperationException($"There can be only one instance of type {nameof(LedenetDeviceProvider)}");
            }

            _instance = this;
        }
        protected override IEnumerable<IRGBDevice> LoadDevices()
        {
            // TODO Connect async
            if (LedenetDeviceDefinitions != null)
                foreach (LedenetDeviceDefinition device in LedenetDeviceDefinitions)
                {
                    Light light = new();
                    bool add = false;
                    try
                    {
                        light.ConnectAsync(IPAddress.Parse(device.HostName)).Wait();
                        light.TurnOnAsync().Wait();
                        light.AutoRefreshEnabled = true;
                    }
                    catch
                    {
                        add = false;
                    }
                    if (add)
                    {
                        yield return new LedenetRGBDevice(new LedenetRGBDeviceInfo(RGBDeviceType.LedStripe, "Ledenet light", device.HostName), new LedenetUpdateQueue(GetUpdateTrigger(), light));
                        _initializedDevices.Add(light);

                    }
                }
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (Light device in _initializedDevices)
            {
                device?.TurnOffAsync().Wait();
                device?.Dispose();
            }

            _initializedDevices.Clear();
            LedenetDeviceDefinitions?.Clear();
        }

        protected override void InitializeSDK() { }
    }
}
