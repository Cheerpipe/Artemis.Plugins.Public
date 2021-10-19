using RGB.NET.Core;
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

namespace RGB.NET.Devices.YeeLight.Generic
{
    public class YeeLightRGBDeviceInfo : IRGBDeviceInfo
    {
        public RGBDeviceType DeviceType { get; }

        public string DeviceName { get; }

        public string IpAddress { get; }

        public string Manufacturer => "YeeLight";

        public string Model { get; }

        public object? LayoutMetadata { get; set; }

        internal YeeLightRGBDeviceInfo(RGBDeviceType deviceType, string model, string ipAddress)
        {
            DeviceType = deviceType;
            Model = model;
            IpAddress = ipAddress;

            DeviceName = "YeeLight Light";
        }
    }
}
