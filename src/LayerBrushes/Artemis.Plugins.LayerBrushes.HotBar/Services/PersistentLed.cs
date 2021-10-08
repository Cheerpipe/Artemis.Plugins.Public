using RGB.NET.Core;

namespace Artemis.Plugins.LayerBrushes.Hotbar.Services
{
    public class PersistentLed
    {
        public PersistentLed(LedId id, string deviceIdentifier)
        {
            Id = id;
            DeviceIdentifier = deviceIdentifier;
        }

        public LedId Id { get; set; }
        public string DeviceIdentifier { get; set; }
    }
}
