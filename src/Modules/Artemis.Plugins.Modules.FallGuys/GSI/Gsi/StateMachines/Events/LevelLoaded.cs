using System;

namespace FallGuys.Gsi.StateMachines.Events
{
    public delegate void LevelLoadedEventHandler(object sender, LevelLoadedArgs e);

    public class LevelLoadedArgs : EventArgs
    {
        internal LevelLoadedArgs(LevelStats levelStats)
        {
            LevelStats = levelStats;
        }

        public LevelStats LevelStats { get; }
    }
}
