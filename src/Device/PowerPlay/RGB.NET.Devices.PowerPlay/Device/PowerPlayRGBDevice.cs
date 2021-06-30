using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.PowerPlay.Device
{
    public class PowerPlayRgbDevice : AbstractRGBDevice<PowerPlayRgbDeviceInfo>, IMouse, IMousepad
    {
        internal PowerPlayRgbDevice(PowerPlayRgbDeviceInfo info, IUpdateQueue updateQueue, int ledCount)
            : base(info, updateQueue)
        {
            InitializeLayout(ledCount);
        }

        private void InitializeLayout(int ledCount)
        {
            for (var i = 0; i < ledCount; i++)
                AddLed(LedId.LedStripe1 + i, new Point(i * 20, 0), new Size(20, 20));
        }

        /// <inheritdoc />
        protected override object GetLedCustomData(LedId ledId) => ((int)ledId - (int)LedId.LedStripe1);

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => UpdateQueue.SetData(GetUpdateData(ledsToUpdate));
    }
}
