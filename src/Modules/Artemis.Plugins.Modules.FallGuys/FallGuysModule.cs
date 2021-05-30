using Artemis.Core;
using Artemis.Core.Modules;
using Artemis.Plugins.Modules.FallGuys.DataModels;
using FallGuys.Gsi;
using SkiaSharp;

namespace Artemis.Plugins.Modules.Games
{
    [PluginFeature(AlwaysEnabled = true)]
    public class FallGuysModule : ProfileModule<FallGuysDataModel>
    {
        private Gsi _gsi;
        public override void Enable()
        {
            DisplayName = "Fall Guys";
            DisplayIcon = "FallGuys.svg";
            DefaultPriorityCategory = ModulePriorityCategory.Application;
            ActivationRequirements.Add(new ProcessActivationRequirement("FallGuys_client_game"));
        }

        public FallGuysModule(PluginSettings settings)
        {
        }

        private void _gamesModulePathWildCard_SettingChanged(object sender, System.EventArgs e)
        {
        }

        // This is the end of your plugin life cycle.
        public override void Disable()
        {
            // Make sure to clean up resources where needed (dispose IDisposables etc.)
        }

        public override void ModuleActivated(bool isOverride)
        {
            // When this gets called your activation requirements have been met and the module will start displaying
            _gsi = new Gsi();

            // Game state Data Model updates
            _gsi.GameStateMachine.ScreenChanged += GameStateMachine_ScreenChanged;
            _gsi.RoundStateMachine.LevelLoaded += RoundStateMachine_LevelLoaded;
            _gsi.RoundStateMachine.LocalPlayerQualified += RoundStateMachine_LocalPlayerQualified;
            _gsi.RoundStateMachine.LocalPlayerDisQualified += RoundStateMachine_LocalPlayerDisQualified;
            _gsi.RoundStateMachine.PlayerQualified += RoundStateMachine_PlayerQualified;
            _gsi.RoundStateMachine.PlayerDisQualified += RoundStateMachine_PlayerDisQualified;
            _gsi.RoundStateMachine.PlayerSpawmed += RoundStateMachine_PlayerSpawmed;

            _gsi.Start();
        }

        private void RoundStateMachine_PlayerSpawmed(object sender, global::FallGuys.Gsi.StateMachines.Events.PlayerStateChangedChangedArgs e)
        {
            DataModel.Players = e.Total;
        }

        private void RoundStateMachine_PlayerDisQualified(object sender, global::FallGuys.Gsi.StateMachines.Events.PlayerStateChangedChangedArgs e)
        {
            DataModel.OtherPlayerDisqualified.Trigger();
            DataModel.DisqualifiedPlayers = e.Total;
        }

        private void RoundStateMachine_PlayerQualified(object sender, global::FallGuys.Gsi.StateMachines.Events.PlayerStateChangedChangedArgs e)
        {
            DataModel.OtherPlayerQualified.Trigger();
            DataModel.QualifiedPlayers = e.Total;
        }

        private void RoundStateMachine_LocalPlayerDisQualified(object sender, System.EventArgs e)
        {
            DataModel.PlayerDisqualified.Trigger();
        }

        private void RoundStateMachine_LocalPlayerQualified(object sender, System.EventArgs e)
        {
            DataModel.PlayerQualified.Trigger();
        }

        private void RoundStateMachine_LevelLoaded(object sender, global::FallGuys.Gsi.StateMachines.Events.LevelLoadedArgs e)
        {
            DataModel.Level = e.LevelStats;
        }

        private void GameStateMachine_ScreenChanged(object sender, global::FallGuys.Gsi.StateMachines.Events.ScreenChangedArgs e)
        {
            DataModel.Screen = e.NewScreen;
        }

        public override void ModuleDeactivated(bool isOverride)
        {
            // When this gets called your activation requirements are no longer met and your module will stop displaying
            _gsi.Stop();
            _gsi.GameStateMachine.ScreenChanged -= GameStateMachine_ScreenChanged;
            _gsi.RoundStateMachine.LevelLoaded -= RoundStateMachine_LevelLoaded;
            _gsi.RoundStateMachine.LocalPlayerQualified -= RoundStateMachine_LocalPlayerQualified;
            _gsi.RoundStateMachine.LocalPlayerDisQualified -= RoundStateMachine_LocalPlayerDisQualified;
            _gsi.RoundStateMachine.PlayerQualified -= RoundStateMachine_PlayerQualified;
            _gsi.RoundStateMachine.PlayerDisQualified -= RoundStateMachine_PlayerDisQualified;
            _gsi.RoundStateMachine.PlayerSpawmed -= RoundStateMachine_PlayerSpawmed;
            _gsi = null;
        }

        public override void Update(double deltaTime)
        {
        }

        public override void Render(double deltaTime, SKCanvas canvas, SKImageInfo canvasInfo)
        {
        }
    }
}