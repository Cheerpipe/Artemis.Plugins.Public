using RGB.NET.Core;
using System.Collections.Generic;

namespace RGB.NET.Devices.Adalight
{
    public class AdalightDeviceDefinition
    {
        public string Name { get; set; } = string.Empty;
        public int Port { get; set; }
        public int LedCount { get; set; }

        public AdalightDeviceDefinition(string name, int port, int ledCount)
        {
            Name = name;
            Port = port;
            LedCount = ledCount;
        }

        public AdalightDeviceDefinition() { }

        public IEnumerable<IRGBDevice> CreateDevices(IDeviceUpdateTrigger updateTrigger)
        {
            AdalightUpdateQueue queue = new AdalightUpdateQueue(updateTrigger, Port, LedCount);

            string name = DeviceHelper.CreateDeviceName("Adalight", Name ?? "light");
            yield return new AdalightRGBDevice(new AdalightRGBDeviceInfo(name), queue, LedCount);

        }
    }
}
