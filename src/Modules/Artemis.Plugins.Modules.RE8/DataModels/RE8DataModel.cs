using Artemis.Core;
using Artemis.Core.Modules;
using SRTPluginProviderRE8.Structs;
using SRTPluginProviderRE8.Structs.GameStructs;
using System.Collections.Generic;

namespace Artemis.Plugins.Modules.FallGuys.DataModels
{
    public class RE8DataModel : DataModel
    {
        public PlayerStatus PlayerStatus { get; set; }
        public float PlayerCurrentHealth { get; set; }
        public float PlayerMaxHealth { get; set; }
        public float PlayerHealthPercentage { get; set; }
        public float PlayerPositionX { get; set; }
        public float PlayerPositionY { get; set; }
        public float PlayerPositionZ { get; set; }
        public int RankScore { get; set; }
        public int Rank { get; set; }
        public int Lei { get; set; }
        public int EventType { get; set; }
        public byte IsMotionPlay { get; set; }
        public string CurrentEvent { get; set; }
        public DataModelEvent DamageReceived { get; set; } = new();

        public DataModelEvent HealingReceived { get; set; } = new();

        public List<InventoryEntry> PlayerInventory { get; set; }
       // public List<EnemyHP> EnemyHealth { get; set; }
        public InventoryEntry LastKeyItem { get; set; }
    }
}
