using FallGuys.LogParser.Enums.States;
using FallGuys.LogParser.LogLine.Base;

namespace FallGuys.LogParser.LogLine
{
    public class GameStateMachineLogLine : BaseLogLine
    {
        public GameState State { get; protected set; }

        public GameStateMachineLogLine(BaseLogLine line) : base(line)
        {
            ParsePayoad(Line);
            Data["State"] = State.ToString();
        }

        protected override void ParsePayoad(string line)
        {
            if (Payload.Contains("FGClient.StateMainMenu"))
            {
                State = GameState.MainMenu;
            }
            else if (Payload.Contains("with FGClient.StateMatchmaking"))
            {
                State = GameState.Matchmaking;
            }
            else if (Payload.Contains("with FGClient.StateConnectToGame"))
            {
                State = GameState.ConnectToGame;
            }
            else if (Payload.Contains("with FGClient.StateWaitingForGameToStart"))
            {
                State = GameState.WaitingForGameToStart;
            }
            else if (Payload.Contains("with FGClient.StateGameLoading"))
            {
                State = GameState.GameLoading;
            }
            else if (Payload.Contains("with FGClient.StateGameInProgress"))
            {
                State = GameState.GameInProgress;
            }
            else if (Payload.Contains("with FGClient.StateQualificationScreen"))
            {
                State = GameState.QualificationScreen;
            }
            else if (Payload.Contains("with FGClient.StateDisconnectingFromServer"))
            {
                State = GameState.DisconnectingFromServer;
            }
            else if (Line.Contains("with FGClient.StateWaitingForRewards"))
            {
                State = GameState.WaitingForRewards;
            }
            else if (Payload.Contains("with FGClient.StateRewardScreen"))
            {
                State = GameState.RewardScreen;
            }
            else if (Payload.Contains("with FGClient.StateReloadingToMainMenu"))
            {
                State = GameState.ReloadingToMainMenu;
            }
            else
            {
                IsIndefined = true;
            }
        }

    }
}
