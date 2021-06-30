using System.Linq;
using System.Threading.Tasks;
using Device.Net;
using Hid.Net.Windows;
// ReSharper disable AsyncConverter.AsyncWait

namespace RGB.NET.Devices.PowerPlay.PowerPlay
{
    public static class PowerPlayLoader
    {
        public const int LogitechVid = 0x046D;
        public const int LogitechPowerPlayPid = 0xC53A;
        public const int LogitechPowerPlayUsagePage = 0xFF00;
        public const int LogitechPowerPlayPage = 2;
        public const string LogitechPowerPlaFriendlyName = "Logitech Powerplay Mouse Mat";


        public static PowerPlayController? MouseController { get; private set; }
        public static PowerPlayController? MatController { get; private set; }

        public static bool IsInitialized { get; private set; }



        public static async Task InitializeAsync()
        {
            var hidFactory =
                new FilterDeviceDefinition(
                        LogitechVid,
                        LogitechPowerPlayPid,
                        LogitechPowerPlayUsagePage,
                        LogitechPowerPlaFriendlyName)
                    .CreateWindowsHidDeviceFactory();

            // The correct device
            var powerPlayDeviceDefinition = (await hidFactory.GetConnectedDeviceDefinitionsAsync().ConfigureAwait(false)).FirstOrDefault(d => d.Usage == LogitechPowerPlayPage);
            var powerPlayDevice = await hidFactory.GetDeviceAsync(powerPlayDeviceDefinition).ConfigureAwait(false);

            if (powerPlayDeviceDefinition == null || powerPlayDevice == null)
            {
                IsInitialized = false;
                return;
            }

            powerPlayDevice.InitializeAsync().Wait();

            MouseController = new PowerPlayController(powerPlayDevice, 0x01, 0x07);
            MatController = new PowerPlayController(powerPlayDevice, 0x07, 0x0B); // We assume the mouse exists because there is no way of know if there is a paired device.
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