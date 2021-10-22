using RGB.NET.Core;

namespace RGB.NET.Devices.LogitechCustom.Device
{
    public class LogitechCustomRgbDeviceInfo : IRGBDeviceInfo
    {
        #region Properties & Fields

        public string DeviceName { get; }

        public RGBDeviceType DeviceType { get; }

        public string Manufacturer => "Logitech";

        public string Model { get; }

        public object? LayoutMetadata { get; set; }

        #endregion

        #region Constructors

        public LogitechCustomRgbDeviceInfo(string model, RGBDeviceType deviceType)
        {
            Model = model;
            DeviceType = deviceType;
            DeviceName = DeviceHelper.CreateDeviceName(Manufacturer, model);
        }

        #endregion
    }
}
