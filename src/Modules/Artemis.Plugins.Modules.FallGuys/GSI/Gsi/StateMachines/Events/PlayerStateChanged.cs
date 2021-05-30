using System;

namespace FallGuys.Gsi.StateMachines.Events
{
    public delegate void PlayerStateChangedEventHandler(object sender, PlayerStateChangedChangedArgs e);

    public class PlayerStateChangedChangedArgs : EventArgs
    {
        internal PlayerStateChangedChangedArgs(int playerId, int total = 0)
        {
            PlayerId = playerId;
            Total = total;
        }

        public int PlayerId { get; }
        public int Total { get; }
    }
}
