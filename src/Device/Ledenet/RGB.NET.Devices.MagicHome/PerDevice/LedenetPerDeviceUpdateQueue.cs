using System;
using System.Threading.Tasks;
using MagicHome;
using RGB.NET.Core;

namespace RGB.NET.Devices.Ledenet.PerDevice
{
    public class LedenetUpdateQueue : UpdateQueue
    {
        private Light _light;
        public LedenetUpdateQueue(IDeviceUpdateTrigger updateTrigger, Light light)
            : base(updateTrigger)
        {
            this._light = light;
        }
        protected override void Update(in ReadOnlySpan<(object key, Color color)> dataSet)
        {
            Color color = dataSet[0].color;
            SetColor(color);
        }

        public async Task SetColor(Color color)
        {
            if (!_light.Connected)
                return;
            var R = (int)Math.Round(color.R * 255);
            var G = (int)Math.Round(color.G * 255);
            var B = (int)Math.Round(color.B * 255);
            _light.SetColorAsync(System.Drawing.Color.FromArgb(R, G, B));
        }
    }
}
