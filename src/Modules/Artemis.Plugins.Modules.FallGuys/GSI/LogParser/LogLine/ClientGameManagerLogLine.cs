using FallGuys.LogParser.Enums.States;
using FallGuys.LogParser.LogLine.Base;
using System.Text.RegularExpressions;

namespace FallGuys.LogParser.LogLine
{
    public class ClientGameManagerLogLine : BaseLogLine
    {
        public ClientGameManagerState State { get; protected set; }
        public int LocalPlayerId { get; protected set; }
        public int UnSpawmedPlayerId { get; protected set; }

        public ClientGameManagerLogLine(BaseLogLine line) : base(line)
        {
            ParsePayoad(Line);
            Data["State"] = State.ToString();
        }

        protected override void ParsePayoad(string line)
        {
            if (Payload.Contains("GameLevelLoaded"))
            {
                State = ClientGameManagerState.GameLevelLoaded;
            }
            else if (Payload.Contains("state 'LevelLoaded'"))
            {
               State = ClientGameManagerState.LevelLoaded;
            }
            else if (Payload.Contains("state 'ObjectsSpawned'"))
            {
                State = ClientGameManagerState.ObjectsSpawmed;
            }
            else if (Payload.Contains("state 'ReadyToPlay'"))
            {
                State = ClientGameManagerState.ReadyToPlay;
            }
            else if (Payload.Contains("Handling bootstrap for local"))
            {
                State = ClientGameManagerState.BootStrapLocalPlayer;
                Regex contextRegex = new(@"(?i)playerID\s=\s+([0-9]+),");
                if (int.TryParse(contextRegex.Match(Payload).Groups[1].Value, out int unSpawnId))
                {
                    LocalPlayerId = unSpawnId;
                }
            }
            else if (Payload.Contains("Finalising spawn for player"))
            {
                State = ClientGameManagerState.PlayerSpawmed;
            }
            else if (Payload.Contains("Handling unspawn for player FallGuy"))
            {
                State = ClientGameManagerState.PlayerUnspawmed;

                Regex contextRegex = new(@"\[([0-9]+)\]");
                if (int.TryParse(contextRegex.Match(Payload).Groups[1].Value, out int unSpawnId))
                {
                    UnSpawmedPlayerId = unSpawnId;
                }
            }
            else
            {
                IsIndefined = true;
            }
        }
    }
}
