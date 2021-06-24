using RGB.NET.Core;
using System.Collections.Generic;

namespace RGB.NET.Devices.Adalight
{
    public class AdalightRGBDevice : AbstractRGBDevice<AdalightRGBDeviceInfo>, IUnknownDevice
    {
        internal AdalightRGBDevice(AdalightRGBDeviceInfo info, IUpdateQueue updateQueue, int ledCount)
            : base(info, updateQueue)
        {
            InitializeLayout(ledCount);
        }


        private void InitializeLayout(int ledCount)
        {
            for (int i = 0; i < ledCount; i++)
                AddLed(LedId.LedStripe1 + i, new Point(i * 16, 0), new Size(17, 15));
        }

        /// <inheritdoc />
        protected override object GetLedCustomData(LedId ledId) => ((int)ledId - (int)LedId.LedStripe1);

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => UpdateQueue.SetData(GetUpdateData(ledsToUpdate));
    }
}
