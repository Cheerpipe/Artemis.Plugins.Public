using System.Collections.Generic;
using System.Linq;
using Artemis.Core;
using Artemis.Core.Services;

namespace Artemis.Plugins.LayerBrushes.Hotbar.Services
{
    public class PersistentLedService : IPluginService
    {
        public List<ArtemisLed> GetSortedLedsFromMap(IReadOnlyCollection<ArtemisLed> leds, List<PersistentLed> ledsMap)
        {
            if (ledsMap == null)
            {
                return leds.ToList();
            }

            List<ArtemisLed> sortedLeds = new();

            foreach (PersistentLed sortedLed in ledsMap)
            {
                var led = leds.FirstOrDefault(o => o.RgbLed.Id == sortedLed.Id && o.Device.Identifier == sortedLed.DeviceIdentifier);

                // If the map can't be mapped, return unsorted map
                if (led == null)
                {
                    return leds.ToList();
                }
                else
                {
                    sortedLeds.Add(led);
                }
            }
            return sortedLeds;
        }
    }
}
