using System;
using System.Threading.Tasks;
using System.Timers;
using RGB.NET.Core;
using YeelightAPI;

namespace RGB.NET.Devices.YeeLight.PerDevice
{
    public class YeeLightUpdateQueue : UpdateQueue
    {
        private readonly Device _light;

        public YeeLightUpdateQueue(IDeviceUpdateTrigger updateTrigger, Device light)
            : base(updateTrigger)
        {
            _light = light;
            Timer connectTimer = new Timer(1000);
            connectTimer.Elapsed += _connectTimer_Elapsed;
            connectTimer.Start();
        }

        private void _connectTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!_light.IsConnected)
            {
                _light.Connect();
            }
        }

        protected override bool Update(in ReadOnlySpan<(object key, Color color)> dataSet)
        {
            if (!_light.IsConnected)
            {
                return false;
            }

            var color = dataSet[0].color;
            SetColor(color);
            
            return true;
        }

        public async Task<bool> SetColor(Color color)
        {
            var r = color.GetR();
            var g = color.GetG();
            var b = color.GetB();
            var l = (int)color.GetLabL();


            if (l == 0)
            {
                return await _light.SetPower(false);
            }

            await _light.SetPower();
            await _light.SetRGBColor(r, g, b);
            return await _light.SetBrightness(l);
        }
    }
}

