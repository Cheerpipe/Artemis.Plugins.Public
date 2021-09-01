using RGB.NET.Core;
using System.Collections.Generic;

namespace RGB.NET.Devices.Adalight
{
    public class AdalightDeviceDefinition
    {
        public string Name { get; set; } = string.Empty;
        public int Port { get; set; }
        public int LedCount { get; set; }
        public int BaudRate { get; set; } = 115200;

        public AdalightDeviceDefinition(string name, int port, int ledCount, int baudRate)
        {
            Name = name;
            Port = port;
            BaudRate = baudRate;
            LedCount = ledCount;
        }

        public AdalightDeviceDefinition() { }

        public IEnumerable<IRGBDevice> CreateDevices(IDeviceUpdateTrigger updateTrigger)
        {
            AdalightUpdateQueue queue = new AdalightUpdateQueue(updateTrigger, Port, LedCount, BaudRate);

            string name = DeviceHelper.CreateDeviceName("Adalight", Name ?? "light");
            yield return new AdalightRGBDevice(new AdalightRGBDeviceInfo(name), queue, LedCount);

        }
    }
}
