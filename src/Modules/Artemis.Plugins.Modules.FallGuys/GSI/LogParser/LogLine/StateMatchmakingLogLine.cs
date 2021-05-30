using FallGuys.LogParser.Enums.States;
using FallGuys.LogParser.LogLine.Base;
using System;

namespace FallGuys.LogParser.LogLine
{
    public class StateMatchmakingLogLine : BaseLogLine
    {
        public StateMatchmakingState State { get; protected set; }
        public StateMatchmakingLogLine(BaseLogLine line) : base(line)
        {
            ParsePayoad(Line);
            Data["State"] = State.ToString();
        }

        protected override void ParsePayoad(string line)
        {
            if (Payload.Contains("Begin matchmaking solo"))
            {
                State = StateMatchmakingState.BeginMatchmakingSolo;
            }
            else if(Payload.Contains("Begin matchmaking"))
            {
                State = StateMatchmakingState.BeginMatchmakingParty;
            }
            else if (Payload.Contains("Found game on"))
            {
                State = StateMatchmakingState.FoundGame;
            }
            else
            {
                IsIndefined = true;
            }
        }
    }
}
