using RGB.NET.Core;

namespace RGB.NET.Devices.Adalight
{
    public class AdalightRGBDeviceInfo : IRGBDeviceInfo
    {
        #region Properties & Fields

       public string DeviceName { get; }

        public RGBDeviceType DeviceType => RGBDeviceType.LedStripe;

        public string Manufacturer => "Adalight";

        public string Model => "Adalight";

        public object? LayoutMetadata { get; set; }

        #endregion

        #region Constructors

        public AdalightRGBDeviceInfo(string name)
        {
            this.DeviceName = name;
        }

        #endregion
    }
}
