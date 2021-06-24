using RGB.NET.Core;
using RGB.NET.Devices.Ledenet.Generic;
using System.Collections.Generic;
using System.Linq;

namespace RGB.NET.Devices.Ledenet.PerDevice
{
    public class LedenetRGBDevice : LedenetRGBDevice<LedenetRGBDeviceInfo>, IUnknownDevice
    {
        internal LedenetRGBDevice(LedenetRGBDeviceInfo info, IUpdateQueue updateQueue)
            : base(info, updateQueue)
        {
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            AddLed(LedId.Custom1, new Point(0, 0), new Size(10, 10));
        }
        protected override object GetLedCustomData(LedId ledId) => (ledId, 0x00);

        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => UpdateQueue.SetData(GetUpdateData(ledsToUpdate.Take(1)));

        public void SetColor()
        {

        }
    }
}
