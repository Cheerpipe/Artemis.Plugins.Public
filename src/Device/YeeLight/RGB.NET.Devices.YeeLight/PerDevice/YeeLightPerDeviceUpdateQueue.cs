using System;
using RGB.NET.Core;
using YeelightAPI;
using YeelightAPI.Models;

namespace RGB.NET.Devices.YeeLight
{
    public class YeeLightUpdateQueue : UpdateQueue
    {
        private Device _light;
        private MODEL _model;
        public YeeLightUpdateQueue(IDeviceUpdateTrigger updateTrigger, Device light, MODEL model)
            : base(updateTrigger)
        {
            _light = light;
            _model = model;
        }
        protected override void Update(in ReadOnlySpan<(object key, Color color)> dataSet)
        {
            Color color = dataSet[0].color;
            SetColor(color);
        }

        public void SetColor(Color color)
        {

            var R = color.GetR();
            var G = color.GetG();
            var B = color.GetB();
            var L = (int)color.GetLabL();

            if (_model == MODEL.Color) // Color bulb cant be turned off just by setting colors.
            {
                if (L == 0)
                {
                    _light.SetPower(false).Wait(500);
                    return;
                }
                else
                {
                    if (_light.Properties.TryGetValue("power", out object value))
                    {
                        if ((string)value == "off")
                            _light.SetPower(true).Wait(500);
                    }
                }
            }
            else
            {
                // Maybe other bulbs needs other protocol or command order.
            }

            _light.SetBrightness(L).Wait(500);
            var result = _light.SetRGBColor(R, G, B).GetAwaiter().GetResult();
        }
    }
}

