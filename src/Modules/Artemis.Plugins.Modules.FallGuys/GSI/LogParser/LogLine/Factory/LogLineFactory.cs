using FallGuys.LogParser.Enums.Context;
using FallGuys.LogParser.LogLine.Base;

namespace FallGuys.LogParser.LogLine.Factory
{
    public class LogLineFactory
    {
        public static BaseLogLine CreateLogLine(string logLine)
        {
            BaseLogLine line = new(logLine);
            if (line.IsValid == false)
                return null;

            return line.Context switch
            {
                LogLineContext.ClientGameManager => new ClientGameManagerLogLine(line),
                LogLineContext.ClientGameSession => new ClientGameSessionLogLine(line),
                LogLineContext.GameSession => new GameSessionLogLine(line),
                LogLineContext.GameStateMachine => new GameStateMachineLogLine(line),
                LogLineContext.StateGameLoading => new StateGameLoadingLogLine(line),
                LogLineContext.StateMatchmaking => new StateMatchmakingLogLine(line),
                LogLineContext.GameRules => new GameRulesLogLine(line),
                LogLineContext.Unknown => new UnknownLogLine(logLine),
                _ => null,
            };
        }
    }
}
