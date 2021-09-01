using System;
using System.Timers;
using RGB.NET.Core;

namespace RGB.NET.Devices.Adalight
{
    public class AdalightUpdateQueue : UpdateQueue
    {
        private readonly Timer _refreshTimer; // To avoid strip power off by inactivity
        private readonly Timer _reconnectTimer; // To keep the controller conected.
        private readonly Timer _fixLastFrameTimer; // To reapply last frame because. Need when using cheaper controllers.
        private Adalight _strip;

        public AdalightUpdateQueue(IDeviceUpdateTrigger updateTrigger, int port, int baudRate, int ledCount)
            : base(updateTrigger)
        {
            _strip = new Adalight(port, baudRate, ledCount);

            _refreshTimer = new Timer(5000);
            _refreshTimer.Elapsed += _refreshTimer_Elapsed;
            _refreshTimer.AutoReset = true;

            _reconnectTimer = new Timer(5000);
            _reconnectTimer.Elapsed += _reconnectTimer_Elapsed;
            _reconnectTimer.AutoReset = true;
            _reconnectTimer.Start();

            _fixLastFrameTimer = new Timer(100);
            _fixLastFrameTimer.Elapsed += _fixLastFrameTimer_Elapsed;
            _fixLastFrameTimer.AutoReset = false;
            _fixLastFrameTimer.Start();
        }

        private void _fixLastFrameTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _strip.Update();
        }

        private void _reconnectTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!_strip.Connected)
                _strip.Connect();
        }

        private void _refreshTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _strip.Update();
        }

        protected override void OnStartup(object? sender, CustomUpdateData customData)
        {
            base.OnStartup(sender, customData);
            if (!_strip.Connected)
                _strip.Connect();
        }

        protected override void Update(in ReadOnlySpan<(object key, Color color)> dataSet)
        {
            foreach (var d in dataSet)
            {
                byte A = d.color.GetA();
                byte R = d.color.GetR();
                byte G = d.color.GetG();
                byte B = d.color.GetB();
                _strip.UpdatePixel((int)d.key, System.Drawing.Color.FromArgb(A, R, G, B), false);
            }

            _strip.Update();

            _fixLastFrameTimer.Stop();
            _fixLastFrameTimer.Start();

            _refreshTimer.Stop();
            _refreshTimer.Start();
        }

        public override void Dispose()
        {
            base.Dispose();

            _strip.Disconnect();
            _strip.Dispose();
        }
    }
}
