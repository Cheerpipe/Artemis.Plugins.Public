using System;
using RGB.NET.Core;
using RGB.NET.Devices.PowerPlay.PowerPlay;

namespace RGB.NET.Devices.PowerPlay.Device
{
    public class PowerPlayUpdateQueue : UpdateQueue
    {
        private readonly PowerPlayController? _powerPlayController;

        public PowerPlayUpdateQueue(IDeviceUpdateTrigger updateTrigger, PowerPlayController? powerPlayController)
            : base(updateTrigger)
        {
            if (powerPlayController != null) _powerPlayController = powerPlayController;
        }

        protected override void Update(in ReadOnlySpan<(object key, Color color)> dataSet)
        {
            foreach (var d in dataSet)
            {
                var a = d.color.GetA();
                var r = d.color.GetR();
                var g = d.color.GetG();
                var b = d.color.GetB();
                _powerPlayController?.SetColor(System.Drawing.Color.FromArgb(a, r, g, b), (byte)(int)d.key);
            }
        }
    }
}
