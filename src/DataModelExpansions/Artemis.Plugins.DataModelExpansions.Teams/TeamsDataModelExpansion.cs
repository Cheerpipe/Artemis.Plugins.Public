using Artemis.Core.Modules;
using Artemis.Core;
using System.Collections.Generic;
using Artemis.Plugins.DataModelExpansions.Teams.DataModels;
using Artemis.Plugins.DataModelExpansions.Teams.TeamsPresence;
using Artemis.Core.Services;
using System.Linq;

namespace Artemis.Plugins.DataModelExpansions.Teams
{
    [PluginFeature(Name = "Teams")]
    public class Teams : Module<TeamsDataModel>
    {
        #region Variables declarations

        private static TeamsStateReader _teamsStateReader;
        private static CameraStateReader _cameraStateReader;
        private const string TEAMS_PROCESS_NAME = "Teams";

        #endregion

        // Allow Datamodel availabable to all profiles
        //public override List<IModuleActivationRequirement> ActivationRequirements => new() { new ProcessActivationRequirement("Teams") };

        public override List<IModuleActivationRequirement> ActivationRequirements => null;

        #region Plugin Methods
        public override void Enable()
        {
            _teamsStateReader = new TeamsStateReader();
            _teamsStateReader.StatusChanged += TeamsLogService_StatusChanged;
            _teamsStateReader.ActivityChanged += TeamsLogService_ActivityChanged;
            _teamsStateReader.Start();
            _cameraStateReader = new CameraStateReader(1000);
            _cameraStateReader.StatusChanged += CameraDetectionService_StatusChanged;
            _cameraStateReader.CameraOwnerChanged += _cameraStateReader_CameraOwnerChanged;
            _cameraStateReader.Start();
        }

        private bool TeamsIsRunning()
        {
            return ProcessMonitor.IsProcessRunning(TEAMS_PROCESS_NAME);
        }

        private void _cameraStateReader_CameraOwnerChanged(object sender, CameraOwnerChangedEventArgs e)
        {
            DataModel.CameraProcessOwner = string.IsNullOrEmpty(e.ProcessName) ? "None" : e.ProcessName;
        }

        private void CameraDetectionService_StatusChanged(object sender, CameraStatusChangedEventArgs e)
        {
            DataModel.CameraStatus = e.Status;
        }

        private void TeamsLogService_ActivityChanged(object sender, Enums.TeamsActivity e)
        {
            if (TeamsIsRunning())
                DataModel.TeamsActivity = e;
            else
            {
                DataModel.TeamsStatus = Enums.TeamsStatus.NotRunning;
                DataModel.TeamsActivity = Enums.TeamsActivity.NotRunning;
            }
        }

        private void TeamsLogService_StatusChanged(object sender, Enums.TeamsStatus e)
        {
            if (TeamsIsRunning())
                DataModel.TeamsStatus = e;
            else
            {
                DataModel.TeamsStatus = Enums.TeamsStatus.NotRunning;
                DataModel.TeamsActivity = Enums.TeamsActivity.NotRunning;
            }
        }

        public override void Disable()
        {
            _teamsStateReader.Stop();
            _teamsStateReader = null;
            _cameraStateReader.Stop();
            _cameraStateReader = null;
        }

        public override void Update(double deltaTime) { }

        #endregion

    }
}