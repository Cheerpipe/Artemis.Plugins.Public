using FallGuys.Gsi.StateMachines;
using FallGuys.Gsi.StateMachines.Enums;
using FallGuys.LogParser.Enums.Context;
using FallGuys.LogParser.Enums.States;
using FallGuys.LogParser.Events;
using FallGuys.LogParser.LogLine;

namespace FallGuys.Gsi
{
    public class Gsi
    {
        private readonly LogParser.LogParser _parser;
        private readonly GameStateMachine _gameStateMachine;
        private readonly RoundStateMachine _roundStateMachine;
        public Gsi()
        {
            string logPath = Utils.GetLogPath();
            string logFileName = Utils.GetLogFileName();

            _gameStateMachine = new GameStateMachine(Screen.Undefined);
            _roundStateMachine = new RoundStateMachine(GameSessionState.Undefined);

            _parser = new LogParser.LogParser(logPath, logFileName);
            _parser.LogLineParsed += _parser_LogLineParsed;
        }

        public void Start()
        {
            _parser.Start();
        }

        public void Stop()
        {
            _parser.Stop();
        }

        public GameStateMachine GameStateMachine { get => _gameStateMachine; }
        public RoundStateMachine RoundStateMachine { get => _roundStateMachine; }
        public LogParser.LogParser LogParser { get => _parser; }

        public bool IsRunning { get => _parser.IsRunning; }


        private void _parser_LogLineParsed(object sender, LogLineParsedArgs e)
        {
            switch (e.LogLine.Context)
            {
                case LogLineContext.GameStateMachine:
                    if ((e.LogLine as GameStateMachineLogLine).State == GameState.MainMenu)
                        _gameStateMachine.SetNewState(Screen.MainMenu);
                    else if ((e.LogLine as GameStateMachineLogLine).State == GameState.QualificationScreen)
                        _gameStateMachine.SetNewState(Screen.QualifyScreen);
                    else if ((e.LogLine as GameStateMachineLogLine).State == GameState.GameLoading)
                        _gameStateMachine.SetNewState(Screen.WaitingPlayers);
                    break;
                case LogLineContext.GameSession:
                    if ((e.LogLine as GameSessionLogLine).State == GameSessionState.Countdown)
                        _gameStateMachine.SetNewState(Screen.CountDown);
                    if ((e.LogLine as GameSessionLogLine).State == GameSessionState.GameOver)
                        _gameStateMachine.SetNewState(Screen.GameOver);
                    if ((e.LogLine as GameSessionLogLine).State == GameSessionState.Playing)
                        _gameStateMachine.SetNewState(Screen.Playing);
                    break;
                case LogLineContext.ClientGameManager:
                    if ((e.LogLine as ClientGameManagerLogLine).State == ClientGameManagerState.BootStrapLocalPlayer)
                        _roundStateMachine.SetLocalPlayerId((e.LogLine as ClientGameManagerLogLine).LocalPlayerId);
                    if ((e.LogLine as ClientGameManagerLogLine).State == ClientGameManagerState.PlayerSpawmed)
                        _roundStateMachine.SpawmPlayer((e.LogLine as ClientGameManagerLogLine).LocalPlayerId);
                    if ((e.LogLine as ClientGameManagerLogLine).State == ClientGameManagerState.PlayerUnspawmed)
                        _roundStateMachine.UnSpawmPlayer((e.LogLine as ClientGameManagerLogLine).LocalPlayerId);
                    else if ((e.LogLine as ClientGameManagerLogLine).State == ClientGameManagerState.GameLevelLoaded)
                    {
                        _gameStateMachine.SetNewState(Screen.WaitingPlayers);
                        _roundStateMachine.Reset();
                    }
                    else if ((e.LogLine as ClientGameManagerLogLine).State == ClientGameManagerState.ObjectsSpawmed)
                        _gameStateMachine.SetNewState(Screen.PreCountDown);
                    break;
                case LogLineContext.GameRules:
                    if ((e.LogLine as GameRulesLogLine).State == GameRulesState.Successful)
                        _roundStateMachine.PlayerSuccess((e.LogLine as GameRulesLogLine).PlayerId);
                    else
                        _roundStateMachine.PlayerUnSuccess((e.LogLine as GameRulesLogLine).PlayerId);
                    break;
                case LogLineContext.StateGameLoading:
                    if ((e.LogLine as StateGameLoadingLogLine).State == GameLoadingState.FinishedLoadingGameGevel)
                    {
                        _roundStateMachine.SetMap(LevelStats.GetStatsByLevelName((e.LogLine as StateGameLoadingLogLine).RoundName));
                    }
                    break;

                case LogLineContext.StateMatchmaking:
                    if ((e.LogLine as StateMatchmakingLogLine).State ==
                          StateMatchmakingState.BeginMatchmakingSolo ||
                          (e.LogLine as StateMatchmakingLogLine).State == StateMatchmakingState.BeginMatchmakingParty)
                        _gameStateMachine.SetNewState(Screen.Matching);
                    break;
            }
        }
    }
}
