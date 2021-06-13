using Artemis.Core;
using Artemis.Core.Modules;
using FallGuys.Gsi;
using FallGuys.Gsi.StateMachines.Enums;

namespace Artemis.Plugins.Modules.FallGuys.DataModels
{
    public class FallGuysDataModel : DataModel
    {
        public Screen Screen { get; set; }
        public LevelStats Level { get; set; }
        public int Players { get; set; }
        public int QualifiedPlayers { get; set; }
        public int DisqualifiedPlayers { get; set; }
        public DataModelEvent PlayerQualified { get; set; } = new();
        public DataModelEvent PlayerDisqualified { get; set; } = new();
        public DataModelEvent OtherPlayerQualified { get; set; } = new();
        public DataModelEvent OtherPlayerDisqualified { get; set; } = new();
    }
}
