using Serilog;
using System.Threading.Tasks;
using Artemis.Core.Modules;
using Artemis.Core;
using System.Collections.Generic;
using Artemis.Plugins.DataModelExpansions.Teams.DataModels;
using Artemis.Plugins.DataModelExpansions.Teams.TeamsPresence;

namespace Artemis.Plugins.DataModelExpansions.Teams
{

    [PluginFeature(Name = "Teams", AlwaysEnabled = true)]
    public class TeamsDataModelExpansion : Module<TeamsDataModel>
    {
        #region Variables declarations

        private readonly ILogger _logger;
        private static TeamsLogService TeamsLogService;
        private static CameraDetectionService CameraDetectionService;

        #endregion

        #region Constructor

        public TeamsDataModelExpansion(ILogger logger)
        {
            _logger = logger;
        }

        public override List<IModuleActivationRequirement> ActivationRequirements { get; } = new()
        {
            // new ProcessActivationRequirement("Teams PROCESS NAME")
        };

        #endregion

        #region Plugin Methods
        public override void Enable()
        {
            //AddTimedUpdate(TimeSpan.FromSeconds(1), UpdateData);

            TeamsLogService = new TeamsLogService();
            TeamsLogService.StatusChanged += TeamsLogService_StatusChanged;
            TeamsLogService.ActivityChanged += TeamsLogService_ActivityChanged;
            TeamsLogService.Start();
            CameraDetectionService = new CameraDetectionService(1000);
            CameraDetectionService.StatusChanged += CameraDetectionService_StatusChanged;
            CameraDetectionService.Start();
        }

        private void CameraDetectionService_StatusChanged(object sender, CameraStatusChangedEventArgs e)
        {
            DataModel.CameraStatus = e.Status;
        }

        private void TeamsLogService_ActivityChanged(object sender, Enums.TeamsActivity e)
        {
            DataModel.TeamsActivity = e;
        }

        private void TeamsLogService_StatusChanged(object sender, Enums.TeamsStatus e)
        {
            DataModel.TeamsStatus = e;
        }

        public override void Disable()
        {
            TeamsLogService.Stop();
            TeamsLogService = null;
            CameraDetectionService.Stop();
            CameraDetectionService = null;
        }

        public override void Update(double deltaTime)
        {
        }
        #endregion

        #region DataModel update methods

        private async Task UpdateData(double deltaTime)
        {

        }


        #endregion
    }
}