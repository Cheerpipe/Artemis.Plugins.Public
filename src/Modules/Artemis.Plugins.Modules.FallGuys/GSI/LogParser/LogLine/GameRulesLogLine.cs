using FallGuys.LogParser.Enums.States;
using FallGuys.LogParser.LogLine.Base;
using System;
using System.Text.RegularExpressions;

namespace FallGuys.LogParser.LogLine
{
    public class GameRulesLogLine : BaseLogLine
    {
        public GameRulesState State { get; protected set; }
        public int PlayerId { get; protected set; }
        public GameRulesLogLine(BaseLogLine line) : base(line)
        {
            ParsePayoad(Line);
            Data["State"] = State.ToString();
        }

        protected override void ParsePayoad(string line)
        {
            if (Payload.Contains("unsuccessful"))
            {
                State = GameRulesState.UnSuccessful;

                Regex contextRegex = new(@"(?i)playertId\s+([0-9]+)\s+as");
                if (int.TryParse(contextRegex.Match(Payload).Groups[1].Value, out int unSpawnId))
                {
                    PlayerId = unSpawnId;
                }
            }
            else if (Payload.Contains("successful"))
            {
                State = GameRulesState.Successful;

                Regex contextRegex = new(@"(?i)playerId\s+([0-9]+)\s+as");
                if (int.TryParse(contextRegex.Match(Payload).Groups[1].Value, out int unSpawnId))
                {
                    PlayerId = unSpawnId;
                }
            }
            else
            {
                IsIndefined = true;
            }
        }
    }
}
