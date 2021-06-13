using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MagicHome;
using RGB.NET.Core;
using RGB.NET.Devices.Ledenet.Generic;
using RGB.NET.Devices.Ledenet.PerDevice;

namespace RGB.NET.Devices.Ledenet
{
    public class LedenetDeviceProvider : AbstractRGBDeviceProvider
    {
        private const int LEDENET_CONNECTION_TIMEOUT = 3000;
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
            List<IRGBDevice> _IRGBDevices = new List<IRGBDevice>();
            if (LedenetDeviceDefinitions != null)
                Parallel.ForEach(LedenetDeviceDefinitions, device =>
                {
                    Light light = new();
                    bool add = false;
                    try
                    {
                        if (light.ConnectAsync(IPAddress.Parse(device.HostName)).Wait(TimeSpan.FromMilliseconds(LEDENET_CONNECTION_TIMEOUT)))
                        {
                            light.TurnOnAsync().Wait();
                            add = light.Connected;
                        }
                    }
                    catch
                    {
                        // Log
                    }

                    if (add)
                    {
                        _IRGBDevices.Add(new LedenetRGBDevice(new LedenetRGBDeviceInfo(RGBDeviceType.LedStripe, "Ledenet light", device.HostName), new LedenetUpdateQueue(GetUpdateTrigger(), light)));
                        _initializedDevices.Add(light);

                    }
                });
            return _IRGBDevices;
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
