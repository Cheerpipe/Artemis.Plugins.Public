using RGB.NET.Core;

namespace Artemis.Plugins.LayerBrushes.Hotbar.Services
{
    public class PersistentLed
    {
        public PersistentLed(LedId id, string deviceIdentifier, string deviceName)
        {
            Id = id;
            DeviceIdentifier = deviceIdentifier;
            DeviceName = deviceName;
        }

        public LedId Id { get; set; }
        public string DeviceIdentifier { get; set; }
        public string DeviceName { get; set; }
    }
}
