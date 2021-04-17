using RGB.NET.Core;

namespace RGB.NET.Devices.YeeLight
{
    public abstract class YeeLightRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, IRGBDevice
        where TDeviceInfo : YeeLightRGBDeviceInfo
    {
        protected YeeLightRGBDevice(TDeviceInfo info, IUpdateQueue updateQueue)
            : base(info, updateQueue)
        { }
    }
}
