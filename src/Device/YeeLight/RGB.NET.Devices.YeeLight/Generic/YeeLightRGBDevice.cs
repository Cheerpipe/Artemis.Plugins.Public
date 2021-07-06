using RGB.NET.Core;
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

namespace RGB.NET.Devices.YeeLight.Generic
{
    public abstract class YeeLightRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, IRGBDevice
        where TDeviceInfo : YeeLightRGBDeviceInfo
    {
        protected YeeLightRGBDevice(TDeviceInfo info, IUpdateQueue updateQueue)
            : base(info, updateQueue)
        { }
    }
}
