using Artemis.Core;
using Artemis.Core.Modules;
using Artemis.Plugins.Modules.FallGuys.DataModels;
using Serilog;
using SRTPluginProviderRE8;
using System;
using System.Linq;

namespace Artemis.Plugins.Modules.Games
{
    [PluginFeature(AlwaysEnabled = true)]
    public class RE8Module : Module<RE8DataModel>
    {
        private readonly ILogger _logger;
        private ReaderRE8 _readerRE8;
        private IGameMemoryRE8 _gameMemoryRE8;
        public override void Enable()
        {
            DisplayName = "Resident Evil 8 Village";
            DisplayIcon = "RE8V.svg";
            ActivationRequirements.Add(new ProcessActivationRequirement("re8"));
            UpdateDuringActivationOverride = false;
        }


        public RE8Module(ILogger logger)
        {
            _logger = logger;
        }

        // This is the end of your plugin life cycle.
        public override void Disable()
        {
            // Make sure to clean up resources where needed (dispose IDisposables etc.)
        }

        public override void ModuleActivated(bool isOverride)
        {
            _readerRE8 = new ReaderRE8(_logger);
            _readerRE8.Init();
            AddTimedUpdate(TimeSpan.FromMilliseconds(500), _ => ReadMemory());
        }

        private void ReadMemory()
        {
            _gameMemoryRE8 = (IGameMemoryRE8)_readerRE8.PullData();

            if (_gameMemoryRE8 == null)
                return;

            float previousHealth = DataModel.PlayerCurrentHealth;
            DataModel.PlayerStatus = _gameMemoryRE8.PlayerStatus;
            DataModel.PlayerCurrentHealth = _gameMemoryRE8.PlayerCurrentHealth;
            DataModel.PlayerMaxHealth = (_gameMemoryRE8.PlayerCurrentHealth / _gameMemoryRE8.PlayerMaxHealth) * 100;
            DataModel.PlayerHealthPercentage = _gameMemoryRE8.PlayerMaxHealth;
            DataModel.PlayerPositionX = _gameMemoryRE8.PlayerPositionX;
            DataModel.PlayerPositionY = _gameMemoryRE8.PlayerPositionY;
            DataModel.PlayerPositionZ = _gameMemoryRE8.PlayerPositionZ;
            DataModel.RankScore = _gameMemoryRE8.RankScore;
            DataModel.Rank = _gameMemoryRE8.Rank;
            DataModel.Lei = _gameMemoryRE8.Lei;
            DataModel.EventType = _gameMemoryRE8.EventType;
            DataModel.IsMotionPlay = _gameMemoryRE8.IsMotionPlay;
            DataModel.CurrentEvent = _gameMemoryRE8.CurrentEvent;
            //DataModel.PlayerInventory = _gameMemoryRE8.PlayerInventory.ToList();
           // DataModel.EnemyHealth = _gameMemoryRE8.EnemyHealth.ToList();
            //DataModel.LastKeyItem = _gameMemoryRE8.LastKeyItem;

            if (DataModel.PlayerCurrentHealth > previousHealth + 10)
                DataModel.HealingReceived.Trigger();
            else if (DataModel.PlayerCurrentHealth < previousHealth - 10 && DataModel.PlayerCurrentHealth != 0)
                DataModel.DamageReceived.Trigger();

        }

        public override void ModuleDeactivated(bool isOverride)
        {
            _readerRE8.Dispose();
            _readerRE8 = null;
        }

        public override void Update(double deltaTime)
        {
        }
    }
}