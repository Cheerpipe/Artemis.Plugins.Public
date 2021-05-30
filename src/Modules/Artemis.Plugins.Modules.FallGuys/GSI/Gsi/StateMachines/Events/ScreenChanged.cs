using FallGuys.Gsi.StateMachines.Enums;
using System;

namespace FallGuys.Gsi.StateMachines.Events
{
    public delegate void ScreenChangedEventHandler(object sender, ScreenChangedArgs e);

    public class ScreenChangedArgs : EventArgs
    {
        internal ScreenChangedArgs(Screen previousScreen, Screen newScreen)
        {
            PreviousScreen = previousScreen;
            NewScreen = newScreen;
        }

        public Screen PreviousScreen { get; }
        public Screen NewScreen { get; }
    }
}
