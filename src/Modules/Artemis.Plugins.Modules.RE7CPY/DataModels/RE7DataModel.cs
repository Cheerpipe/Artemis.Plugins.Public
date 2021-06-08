using Artemis.Core;
using Artemis.Core.DataModelExpansions;

namespace Artemis.Plugins.Modules.FallGuys.DataModels
{
    public class RE7DataModel : DataModel
    {
        public string MapName { get; set; }
        public float CurrentDA { get; set; }
        public float CurrentHP { get; set; }
        public float MaxHP { get; set; }
        public DataModelEvent DamageReceived { get; set; } = new();
        public DataModelEvent HealingReceived { get; set; } = new();
        public void Reset()
        {
            MaxHP = 0;
            CurrentHP = 0;
        }
    }
}
