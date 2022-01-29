using FallGuys.Gsi.StateMachines.Enums;
using FallGuys.Gsi.StateMachines.Events;

namespace FallGuys.Gsi.StateMachines
{
    public class GameStateMachine
    {
        public event ScreenChangedEventHandler ScreenChanged;

        public GameStateMachine(Screen initialState)
        {
            SetNewState(initialState);
        }

        public void SetNewState(Screen screen)
        {
            if (CurrentScreen == screen)
                return;

            PreviousScreen = CurrentScreen;
            CurrentScreen = screen;
            ScreenChanged?.Invoke(this, new ScreenChangedArgs(PreviousScreen, CurrentScreen));
        }

        public Screen CurrentScreen { get; private set; }
        public Screen PreviousScreen { get; private set; }
    }
}
