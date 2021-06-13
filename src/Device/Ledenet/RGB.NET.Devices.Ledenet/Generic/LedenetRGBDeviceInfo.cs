using RGB.NET.Core;

namespace RGB.NET.Devices.Ledenet.Generic
{
    public class LedenetRGBDeviceInfo : IRGBDeviceInfo
    {
        public RGBDeviceType DeviceType { get; }

        public string DeviceName { get; }

        public string IpAddress { get; }

        public string Manufacturer => "Ledenet";

        public string Model { get; }

        public object? LayoutMetadata { get; set; }

        internal LedenetRGBDeviceInfo(RGBDeviceType deviceType, string model, string ipAddress)
        {
            DeviceType = deviceType;
            Model = model;
            IpAddress = ipAddress;

            DeviceName = $"Ledenet light ({IpAddress})";
        }
    }
}
