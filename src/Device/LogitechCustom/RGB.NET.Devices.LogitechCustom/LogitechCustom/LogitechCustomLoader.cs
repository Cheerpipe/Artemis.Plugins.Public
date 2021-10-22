using System.Linq;
using System.Threading.Tasks;
using Device.Net;
using Hid.Net.Windows;
// ReSharper disable AsyncConverter.AsyncWait

namespace RGB.NET.Devices.LogitechCustom.LogitechCustom
{
    public static class LogitechCustomLoader
    {
        public const int LogitechVid = 0x046D;
        public const int LogitechLogitechCustomPid = 0xC53A;
        public const int LogitechLogitechCustomUsagePage = 0xFF00;
        public const int LogitechLogitechCustomPage = 2;
        public const string LogitechPowerPlaFriendlyName = "Logitech LogitechCustom Mouse Mat";


        public static LogitechCustomController? MouseController { get; private set; }
        public static LogitechCustomController? MatController { get; private set; }

        public static bool IsInitialized { get; private set; }



        public static async Task InitializeAsync()
        {
            var hidFactory =
                new FilterDeviceDefinition(
                        LogitechVid,
                        LogitechLogitechCustomPid,
                        LogitechLogitechCustomUsagePage,
                        LogitechPowerPlaFriendlyName)
                    .CreateWindowsHidDeviceFactory();

            // The correct device
            var LogitechCustomDeviceDefinition = (await hidFactory.GetConnectedDeviceDefinitionsAsync().ConfigureAwait(false)).FirstOrDefault(d => d.Usage == LogitechLogitechCustomPage);
            var LogitechCustomDevice = await hidFactory.GetDeviceAsync(LogitechCustomDeviceDefinition).ConfigureAwait(false);

            if (LogitechCustomDeviceDefinition == null || LogitechCustomDevice == null)
            {
                IsInitialized = false;
                return;
            }

            LogitechCustomDevice.InitializeAsync().Wait();

            MouseController = new LogitechCustomController(LogitechCustomDevice, 0x01, 0x07, 0x1D);
            MatController = new LogitechCustomController(LogitechCustomDevice, 0x07, 0x0B, 0x3E);
            IsInitialized = true;
        }

        public static void FreeDevices()
        {
            MouseController?.Dispose();
            MatController?.Dispose();
            IsInitialized = false;
        }

    }
}