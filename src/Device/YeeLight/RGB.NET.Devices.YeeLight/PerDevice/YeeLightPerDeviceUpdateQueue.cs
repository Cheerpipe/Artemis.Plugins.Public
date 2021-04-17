using System;
using System.Threading.Tasks;
using RGB.NET.Core;
using YeelightAPI;

namespace RGB.NET.Devices.YeeLight
{
    public class YeeLightUpdateQueue : UpdateQueue
    {
        private Device _light;
        public YeeLightUpdateQueue(IDeviceUpdateTrigger updateTrigger, Device light)
            : base(updateTrigger)
        {
            this._light = light;
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

            if (_light.Model == YeelightAPI.Models.MODEL.Color) // Color bulb cant be turned off just by setting colors.
            {
                if (L == 0)
                {
                    _light.SetPower(false).Wait();
                    return;
                }
                else
                {
                    if (_light.Properties["power"].ToString() == "off")
                        _light.SetPower(true).Wait();
                }
            }
            else
            {
                // Maybe other bulbs needs other protocol or command order.
            }

            _light.SetBrightness(L).Wait();
            _light.SetRGBColor(R, G, B).Wait();
        }
    }
}

