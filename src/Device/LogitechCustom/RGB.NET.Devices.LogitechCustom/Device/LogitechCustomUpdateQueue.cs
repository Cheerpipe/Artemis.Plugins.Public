using System;
using RGB.NET.Core;
using RGB.NET.Devices.LogitechCustom.LogitechCustom;

namespace RGB.NET.Devices.LogitechCustom.Device
{
    public class LogitechCustomUpdateQueue : UpdateQueue
    {
        private readonly LogitechCustomController? _LogitechCustomController;

        public LogitechCustomUpdateQueue(IDeviceUpdateTrigger updateTrigger, LogitechCustomController? LogitechCustomController)
            : base(updateTrigger)
        {
            if (LogitechCustomController != null) _LogitechCustomController = LogitechCustomController;
        }

        protected override void Update(in ReadOnlySpan<(object key, Color color)> dataSet)
        {
            foreach (var d in dataSet)
            {
                var a = d.color.GetA();
                var r = d.color.GetR();
                var g = d.color.GetG();
                var b = d.color.GetB();
                _LogitechCustomController?.SetColor(System.Drawing.Color.FromArgb(a, r, g, b), (byte)(int)d.key);
            }
        }
    }
}
