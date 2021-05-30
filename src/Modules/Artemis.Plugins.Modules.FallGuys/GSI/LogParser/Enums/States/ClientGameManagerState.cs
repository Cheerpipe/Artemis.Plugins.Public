
namespace FallGuys.LogParser.Enums.States
{
    public enum ClientGameManagerState
    {
        Undefined,
        LevelLoaded,    // Setting this client as readiness state 'LevelLoaded'.
        ObjectsSpawmed, // Setting this client as readiness state 'ObjectsSpawned'.
        GameLevelLoaded,
        BootStrapLocalPlayer,
        PlayerSpawmed,
        ReadyToPlay,    // Setting this client as readiness state 'ReadyToPlay'.
        PlayerUnspawmed,
    }
}
