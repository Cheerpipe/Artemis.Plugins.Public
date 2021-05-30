
namespace FallGuys.LogParser.Enums.States
{
    public enum StateMatchmakingState
    {
        Undefined,
        BeginMatchmakingSolo, // [StateMatchmaking] Begin matchmaking solo
        BeginMatchmakingParty , // [StateMatchmaking] Begin matchmaking solo // withoit solo i asume
        FoundGame // [StateMatchmaking] Found game on -> server IP: 128.1.240.130 port: 7977
    }
}
