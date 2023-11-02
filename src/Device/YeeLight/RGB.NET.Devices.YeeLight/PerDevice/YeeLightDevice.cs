using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Devices.YeeLight.Generic;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

namespace RGB.NET.Devices.YeeLight.PerDevice
{
    public class YeeLightDevice : YeeLightRGBDevice<YeeLightRGBDeviceInfo>, IUnknownDevice
    {
        internal YeeLightDevice(YeeLightRGBDeviceInfo info, IUpdateQueue updateQueue)
            : base(info, updateQueue)
        {
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            AddLed(LedId.Custom1, new Point(0, 0), new Size(10, 10));
        }
        protected override object GetLedCustomData(LedId ledId) => (ledId, 0x00);
    }
}
