using SRTPluginProviderRE8.Structs;
using SRTPluginProviderRE8.Structs.GameStructs;
using System.Diagnostics;
using System.Reflection;

namespace SRTPluginProviderRE8
{
    public class GameMemoryRE8 : IGameMemoryRE8
    {
        public string VersionInfo => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
        public PlayerStatus PlayerStatus { get => _playerstatus; set => _playerstatus = value; }
        internal PlayerStatus _playerstatus;

        public float PlayerCurrentHealth { get => _playerCurrentHealth; set => _playerCurrentHealth = value; }
        internal float _playerCurrentHealth;

        public float PlayerMaxHealth { get => _playerMaxHealth; set => _playerMaxHealth = value; }
        internal float _playerMaxHealth;

        public float PlayerPositionX { get => _playerPositionX; set => _playerPositionX = value; }
        internal float _playerPositionX;
        public float PlayerPositionY { get => _playerPositionY; set => _playerPositionY = value; }
        internal float _playerPositionY;
        public float PlayerPositionZ { get => _playerPositionZ; set => _playerPositionZ = value; }
        internal float _playerPositionZ;

        public int RankScore { get => _rankScore; set => _rankScore = value; }
        internal int _rankScore;

        public int Rank { get => _rank; set => _rank = value; }
        internal int _rank;

        public int Lei { get => _lei; set => _lei = value; }
        internal int _lei;

        public int EventType { get => _eventType; set => _eventType = value; }
        internal int _eventType;

        public byte IsMotionPlay { get => _isMotionPlay; set => _isMotionPlay = value; }
        internal byte _isMotionPlay;

        public string CurrentEvent { get => _currentevent; set => _currentevent = value; }
        internal string _currentevent;

        public EnemyHP[] EnemyHealth { get => _enemyHealth; set => _enemyHealth = value; }
        internal EnemyHP[] _enemyHealth;

        public InventoryEntry LastKeyItem { get => _lastKeyItem; set => _lastKeyItem = value; }
        internal InventoryEntry _lastKeyItem;

        public InventoryEntry[] PlayerInventory { get => _playerInventory; set => _playerInventory = value; }
        internal InventoryEntry[] _playerInventory;
    }
}
