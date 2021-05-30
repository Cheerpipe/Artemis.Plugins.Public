using FallGuys.LogParser.Enums.States;
using FallGuys.LogParser.LogLine.Base;

namespace FallGuys.LogParser.LogLine
{
    public class ClientGameSessionLogLine : BaseLogLine
    {
        public int NumPlayersAchievingObjective { get; protected set; }
        public ClientGameSessionState State { get; protected set; }

        public ClientGameSessionLogLine(BaseLogLine line) : base(line)
        {
            ParsePayoad(Line);
            Data["State"] = State.ToString();
        }

        protected override void ParsePayoad(string line)
        {
            if (Payload.Contains("NumPlayersAchievingObjective="))
            {
                State = ClientGameSessionState.PlayerObjectiveAchieved;

                NumPlayersAchievingObjective = int.Parse(Payload.Remove(0, 29));
            }
            else if (Payload.Contains(".SwitchToResultsState"))
            {
                State = ClientGameSessionState.SwitchToResults;
            }
            else
            {
                IsIndefined = true;
            }
        }
    }
}

