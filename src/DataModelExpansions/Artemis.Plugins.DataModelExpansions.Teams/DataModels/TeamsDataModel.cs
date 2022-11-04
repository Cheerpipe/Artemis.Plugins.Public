using Artemis.Core.Modules;
using Artemis.Plugins.DataModelExpansions.Teams.Enums;

namespace Artemis.Plugins.DataModelExpansions.Teams.DataModels
{
    public class TeamsDataModel : DataModel
    {
        public TeamsStatus TeamsStatus { get; set; }
        public TeamsActivity TeamsActivity { get; set; }
        public CameraStatus CameraStatus { get; set; }
    }
}