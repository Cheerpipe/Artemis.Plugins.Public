using System;
using System.Drawing;
using Device.Net;
// ReSharper disable AsyncConverter.AsyncWait

namespace RGB.NET.Devices.LogitechCustom.LogitechCustom
{
    public class LogitechCustomController : IDisposable
    {
        public const int LogitechWirelessProtocolTimeout = 300;
        private readonly byte _deviceIndex;
        private readonly byte _featureIndex;
        private readonly byte _thirdIndex;
        private readonly IDevice _hidDevice;
        readonly byte[] _usbBuf = new byte[20];

        public LogitechCustomController(IDevice device, byte deviceIndex, byte featureIndex, byte thirdIndex)
        {
            _deviceIndex = deviceIndex;
            _featureIndex = featureIndex;
            _thirdIndex = thirdIndex;
            _hidDevice = device;

            Initialize();
        }
        public void Initialize()
        {
            if (!_hidDevice.IsInitialized)
                _hidDevice.InitializeAsync().Wait();

            if (!_hidDevice.IsInitialized)
            {
                throw new Exception($"Logitech device couldn't be initialized.");
            }

            _usbBuf[0x00] = 0x11;
            _usbBuf[0x01] = _deviceIndex;
            _usbBuf[0x02] = _featureIndex;
            _usbBuf[0x03] = _thirdIndex;
            _usbBuf[0x04] = 0x00; //Zone
            _usbBuf[0x05] = 0x01; //Mode
            _usbBuf[0x09] = 0x02; //Static. Not saved into Flash and fast enough for direct mode
            _usbBuf[0x10] = 0x01; // I don't know what is it
        }

        public void SetColor(Color color, byte zone)
        {
            lock (_hidDevice)
            {
                _usbBuf[0x04] = zone;
                _usbBuf[0x06] = color.R;
                _usbBuf[0x07] = color.G;
                _usbBuf[0x08] = color.B;
                _ = _hidDevice.WriteAsync(_usbBuf).Wait(LogitechWirelessProtocolTimeout);
            }
        }
        public void Dispose()
        {
            _hidDevice.Close();
            _hidDevice.Dispose();
        }
    }
}