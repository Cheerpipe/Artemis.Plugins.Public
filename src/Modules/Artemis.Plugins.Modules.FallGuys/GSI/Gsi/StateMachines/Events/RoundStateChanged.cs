using FallGuys.LogParser.Enums.States;
using System;

namespace FallGuys.Gsi.StateMachines.Events
{
    public delegate void RoundStateChangedEventHandler(object sender, RoundStateChangedArgs e);

    public class RoundStateChangedArgs : EventArgs
    {
        internal RoundStateChangedArgs(GameSessionState previousState, GameSessionState newState)
        {
            PreviousState = previousState;
            NewState = newState;
        }

        public GameSessionState PreviousState { get; }
        public GameSessionState NewState { get; }
    }
}
