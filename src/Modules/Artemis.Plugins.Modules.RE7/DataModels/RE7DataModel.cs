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
        public int MrEverything { get; set; }
        public int EnemyCount { get; set; }
        //public  EnemyHP[] EnemyHealth { get; set; }
        public int PlayerInventoryCount { get; set; }
        public int PlayerInventorySlots { get; set; }
        public int PlayerCurrentSelectedInventorySlots { get; set; }
        //public  InventoryEntry[] PlayerInventory { get; set; }

        public DataModelEvent DamageReceived { get; set; } = new();
        public DataModelEvent HealingReceived { get; set; } = new();
    }
}
