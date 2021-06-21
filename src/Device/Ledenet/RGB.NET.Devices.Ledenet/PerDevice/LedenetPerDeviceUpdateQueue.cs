using System;
using System.Threading.Tasks;
using MagicHome;
using RGB.NET.Core;

namespace RGB.NET.Devices.Ledenet.PerDevice
{
    public class LedenetUpdateQueue : UpdateQueue
    {
        private Light _light;
        private bool _placeHolder;
        public LedenetUpdateQueue(IDeviceUpdateTrigger updateTrigger, Light light, bool placeHolder = false)
            : base(updateTrigger)
        {
            _light = light;
            placeHolder = placeHolder;
        }
        protected override void Update(in ReadOnlySpan<(object key, Color color)> dataSet)
        {
            if (_placeHolder)
                return;

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
