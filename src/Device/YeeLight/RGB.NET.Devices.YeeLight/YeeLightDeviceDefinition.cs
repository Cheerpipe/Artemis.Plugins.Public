using RGB.NET.Devices.YeeLight.Enums;
using YeelightAPI.Models;

namespace RGB.NET.Devices.YeeLight
{
    public class YeeLightDeviceDefinition
    {
        public string HostName { get; set; }
        public MODEL Model { get; set; }
        public string DeviceName { get; set; }
        public OperationModes Mode { get; set; }
    }
}
