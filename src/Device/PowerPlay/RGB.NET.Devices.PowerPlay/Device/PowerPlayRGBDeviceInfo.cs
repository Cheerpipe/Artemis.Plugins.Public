using RGB.NET.Core;

namespace RGB.NET.Devices.PowerPlay.Device
{
    public class PowerPlayRgbDeviceInfo : IRGBDeviceInfo
    {
        #region Properties & Fields

        public string DeviceName { get; }

        public RGBDeviceType DeviceType { get; }

        public string Manufacturer => "Logitech";

        public string Model { get; }

        public object? LayoutMetadata { get; set; }

        #endregion

        #region Constructors

        public PowerPlayRgbDeviceInfo(string model, RGBDeviceType deviceType)
        {
            Model = model;
            DeviceType = deviceType;
            DeviceName = DeviceHelper.CreateDeviceName(Manufacturer, model);
        }

        #endregion
    }
}
