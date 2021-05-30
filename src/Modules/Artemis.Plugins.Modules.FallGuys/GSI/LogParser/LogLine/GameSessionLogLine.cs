using FallGuys.LogParser.Enums.States;
using FallGuys.LogParser.LogLine.Base;

namespace FallGuys.LogParser.LogLine
{
    public class GameSessionLogLine : BaseLogLine
    {
        public GameSessionState State { get; protected set; }

        public GameSessionLogLine(BaseLogLine line) : base(line)
        {
            ParsePayoad(Line);
            Data["State"] = State.ToString();
        }

        protected override void ParsePayoad(string line)
        {
            if (Payload.Contains("Precountdown to Countdown"))
            {
                State = GameSessionState.Countdown;
            }
            else if (Payload.Contains("Countdown to Playing"))
            {
                State = GameSessionState.Playing;
            }
            else if (Payload.Contains("Playing to GameOver"))
            {
                State = GameSessionState.GameOver;
            }
            else if (Payload.Contains("GameOver to Results"))
            {
                State = GameSessionState.Results;
            }
            else
            {
                IsIndefined = true;
            }
        }
    }
}
