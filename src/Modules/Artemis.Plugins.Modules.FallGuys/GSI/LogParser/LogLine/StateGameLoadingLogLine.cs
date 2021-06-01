using FallGuys.LogParser.Enums.States;
using FallGuys.LogParser.LogLine.Base;
using System.Text.RegularExpressions;

namespace FallGuys.LogParser.LogLine
{
    public class StateGameLoadingLogLine : BaseLogLine
    {
        public string LevelSceneName { get; protected set; }
        public string RoundName { get; protected set; }
        public GameLoadingState State { get; protected set; }
        public int PlayersSpawmed { get; protected set; }

        public StateGameLoadingLogLine(BaseLogLine line) : base(line)
        {
            ParsePayoad(Line);
            Data["State"] = State.ToString();
        }

        protected override void ParsePayoad(string line)
        {

            if (Payload.Contains("WaitAndLoadLevel"))
            {
                State = GameLoadingState.WaitAndLoadLevel;
            }
            else if (Payload.Contains("LoadCurrentlySelectedGameLevel"))
            {
                State = GameLoadingState.LoadCurrentlySelectedGameLevel;
            }
            else if (Payload.Contains("Loading game level scene"))
            {
                State = GameLoadingState.LoadingGameLevelScene;
                Regex contextRegex = new(@"(?i)scene\s+(.+?)\s+- frame");
                LevelSceneName = contextRegex.Match(Payload).Groups[1].Value;
            }
            else if (Payload.Contains("LoadGameLevelSceneASync"))
            {
                State = GameLoadingState.LoadGameLevelSceneASync;
            }
            else if (Payload.Contains("Finished loading game level"))
            {
                State = GameLoadingState.FinishedLoadingGameGevel;

                Regex contextRegex = new(@"(?i)to\sbe\s+(.+?)\.\s+Duration");
                RoundName = contextRegex.Match(Payload).Groups[1].Value;
            }
            else if (Payload.Contains("OnPlayerSpawned"))
            {
                Regex contextRegex = new(@"(?i)ID=([0-9]+)\s+was");
                if (int.TryParse(contextRegex.Match(Payload).Groups[1].Value, out int spawnCount))
                {
                    PlayersSpawmed = spawnCount;
                }
            }
            else
            {
                IsIndefined = true;
            }
        }
    }
}
