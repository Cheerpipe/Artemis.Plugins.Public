using System;
using System.Timers;
using RGB.NET.Core;
using YeelightAPI;
// ReSharper disable AsyncConverter.AsyncWait

namespace RGB.NET.Devices.YeeLight.PerDevice
{
    public class YeeLightUpdateQueue : UpdateQueue
    {
        private readonly Device _light;
        private readonly bool _placeHolder;

        public YeeLightUpdateQueue(IDeviceUpdateTrigger updateTrigger, Device light, bool placeHolder = false)
            : base(updateTrigger)
        {
            _light = light;
            _placeHolder = placeHolder;
        }


        protected override void Update(in ReadOnlySpan<(object key, Color color)> dataSet)
        {
            if (_placeHolder)
                return;
            var color = dataSet[0].color;
            SetColor(color);
        }

        public void SetColor(Color color)
        {
            var r = color.GetR();
            var g = color.GetG();
            var b = color.GetB();
            var l = (int)color.GetLabL();

            if (l == 0)
            {
                _ = _light.SetPower(false);
                return;
            }

            _ = _light.SetPower();
            _ = _light.SetRGBColor(r, g, b);
            _ = _light.SetBrightness(l);
        }
    }
}

