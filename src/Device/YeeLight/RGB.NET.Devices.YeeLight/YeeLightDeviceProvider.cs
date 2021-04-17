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

        public static YeeLightDeviceProvider Instance => _instance ?? new YeeLightDeviceProvider();

        public YeeLightDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(YeeLightDeviceProvider)}");
            _instance = this;
        }
        protected override IEnumerable<IRGBDevice> LoadDevices()
        {
            IEnumerable<Device>? yeeLightDevices = null;
            Task.Run(async () =>
            {
                yeeLightDevices = await DeviceLocator.DiscoverAsync();

            }).GetAwaiter().GetResult();

            if (yeeLightDevices != null)
            {
                foreach (Device device in yeeLightDevices)
                {
                    bool add = false;
                    try
                    {
                        device.Connect().Wait();
                        add = true;
                        //TODO: Log
                    }
                    catch
                    {
                        //TODO: Log
                    }

                    if (add)
                        yield return new YeeLightRGBRGBDevice(new YeeLightRGBDeviceInfo(RGBDeviceType.LedStripe, string.Format("YeeLight {0} ({1})", device.Model, device.Hostname), device.Hostname), new YeeLightUpdateQueue(GetUpdateTrigger(), device));
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        protected override void InitializeSDK()
        {

        }
    }
}
