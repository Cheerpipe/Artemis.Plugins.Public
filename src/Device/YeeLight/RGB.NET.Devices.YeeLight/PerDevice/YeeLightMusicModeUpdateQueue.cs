using System;
using System.Net.Sockets;
using System.Timers;
using RGB.NET.Core;
using YeelightAPI;

namespace RGB.NET.Devices.YeeLight.PerDevice
{
    public class YeeLightMusicModeUpdateQueue : UpdateQueue
    {
        private readonly Device _light;

        public YeeLightMusicModeUpdateQueue(IDeviceUpdateTrigger updateTrigger, Device light)
            : base(updateTrigger)
        {
            _light = light;
            _light.OnError += _light_OnError;
            Timer connectTimer = new Timer(1000);
            connectTimer.Elapsed += _connectTimer_Elapsed;
            StopLightStream();
            connectTimer.Start();
        }

        private void _light_OnError(object sender, UnhandledExceptionEventArgs e)
        {
            StopLightStream();
            StartLightStream();
        }

        private void _connectTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!_light.IsConnected)
            {
                _light.Connect();
            }
        }

        protected override void Update(in ReadOnlySpan<(object key, Color color)> dataSet)
        {
            if (!_light.IsConnected)
            {
                return;
            }

            var color = dataSet[0].color;
            SetColor(color);
        }

        public async void SetColor(Color color)
        {
            var L = (int)color.GetLabL();
            bool powerControlResult;

            try
            {
                if (L == 0)
                {
                    powerControlResult = StopLightStream();
                    return;
                }
                else
                {
                    powerControlResult = StartLightStream();
                }
                if (!powerControlResult)
                    return;

                var R = color.GetR();
                var G = color.GetG();
                var B = color.GetB();

                await _light.SetBrightness(L);
                await _light.SetRGBColor(R, G, B);
            }
            catch
            {
                _light.Disconnect();
            }
        }

        private void FixLowFrameRateWorkArround()
        {
            TcpClient tcpClient = ReflectionTools.GetPrivateField<TcpClient>(_light, "_tcpClient");
            tcpClient.Client.NoDelay = true;
        }

        bool IsTurnedOn;
        private bool StopLightStream()
        {
            lock (this)
            {
                if (!IsTurnedOn)
                    return true;

                _light.SetPower(false).Wait();

                if (_light.IsMusicModeEnabled)
                    _light.StopMusicMode().Wait();

                _light.Disconnect();

                IsTurnedOn = _light.IsConnected && _light.IsMusicModeEnabled;
                return !IsTurnedOn;
            }
        }

        private bool StartLightStream()
        {
            lock (this)
            {
                if (IsTurnedOn)
                    return true;

                if (!_light.IsConnected)
                    _light.Connect().Wait();

                _light.SetPower(true).Wait();

                try
                {
                    if (!_light.IsMusicModeEnabled)
                    {
                        _light.StartMusicMode().Wait();
                        FixLowFrameRateWorkArround();
                    }
                }
                catch (Exception E)
                {
                    // do nothing.
                }

                IsTurnedOn = _light.IsConnected && _light.IsMusicModeEnabled;
                return IsTurnedOn;
            }
        }
    }
}

