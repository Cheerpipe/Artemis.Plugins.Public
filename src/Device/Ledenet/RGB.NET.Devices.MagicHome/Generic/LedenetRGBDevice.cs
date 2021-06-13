using RGB.NET.Core;

namespace RGB.NET.Devices.Ledenet.Generic
{
    public abstract class LedenetRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, IRGBDevice
        where TDeviceInfo : LedenetRGBDeviceInfo
    {
        protected LedenetRGBDevice(TDeviceInfo info, IUpdateQueue updateQueue)
            : base(info, updateQueue)
        { }
    }
}
