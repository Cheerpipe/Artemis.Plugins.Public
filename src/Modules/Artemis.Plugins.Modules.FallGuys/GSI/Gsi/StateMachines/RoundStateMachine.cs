using FallGuys.Gsi.StateMachines.Events;
using FallGuys.LogParser.Enums.States;
using System;

namespace FallGuys.Gsi.StateMachines
{
    public class RoundStateMachine
    {
        public event RoundStateChangedEventHandler RoundStateChanged;
        public event EventHandler LocalPlayerQualified;
        public event EventHandler LocalPlayerDisQualified;
        public event PlayerStateChangedEventHandler PlayerQualified;
        public event PlayerStateChangedEventHandler PlayerDisQualified;
        public event PlayerStateChangedEventHandler PlayerSpawmed;
        public event PlayerStateChangedEventHandler PlayerUnSpawmed;
        public event LevelLoadedEventHandler LevelLoaded;

        private int _localPlayerId;
        private int _playerCount;
        private int _successfulCount;
        private int _unSuccessfulCount;
        private LevelStats _levelStats;

        public GameSessionState CurrentRoundState { get; private set; }
        public GameSessionState PreviousRoundState { get; private set; }

        public int QualifiedPlayerCount => _successfulCount;
        public int DisQualifiedPlayerCount => _unSuccessfulCount;
        public int SpawmedPlayerCount => _playerCount;


        public RoundStateMachine(GameSessionState initialState)
        {
            PreviousRoundState = GameSessionState.Undefined;
            CurrentRoundState = GameSessionState.Undefined;
        }

        public void SetLocalPlayerId(int playerId)
        {
            _localPlayerId = playerId;
        }

        public void SetMap(LevelStats levelStats)
        {
            _levelStats = levelStats;
            LevelLoaded?.Invoke(this, new LevelLoadedArgs(_levelStats));
        }

        public void SpawmPlayer(int playerId)
        {
            _playerCount++;
            PlayerSpawmed?.Invoke(this, new PlayerStateChangedChangedArgs(playerId, _playerCount));
        }

        public void UnSpawmPlayer(int playerId)
        {
            _playerCount--;
            PlayerUnSpawmed?.Invoke(this, new PlayerStateChangedChangedArgs(playerId, _playerCount));
        }

        public void PlayerSuccess(int playerId)
        {
            _successfulCount++;
            if (playerId == _localPlayerId)
            {
                LocalPlayerQualified?.Invoke(this, new EventArgs());
            }
            else
                PlayerQualified?.Invoke(this, new PlayerStateChangedChangedArgs(playerId, _successfulCount));
        }

        public void PlayerUnSuccess(int playerId)
        {
            _unSuccessfulCount++;
            if (playerId == _localPlayerId)
            {
                LocalPlayerDisQualified?.Invoke(this, new EventArgs());
            }
            PlayerDisQualified?.Invoke(this, new PlayerStateChangedChangedArgs(playerId, _unSuccessfulCount));
        }

        public void Reset()
        {
            _localPlayerId = -1;
            _playerCount = 0;
            _successfulCount = 0;
            _unSuccessfulCount = 0;
        }

        public void SetNewState(GameSessionState roundState)
        {
            if (CurrentRoundState == roundState)
                return;

            PreviousRoundState = CurrentRoundState;
            CurrentRoundState = roundState;

            RoundStateChanged?.Invoke(this, new RoundStateChangedArgs(PreviousRoundState, CurrentRoundState));
        }
    }
}
