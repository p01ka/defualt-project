using System;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace IdxZero.Application.States.Base
{
    public class ApplicationConditionManager
    {
        private readonly Dictionary<ApplicationStateTransition, ApplicationState> _transitions;
        private ApplicationState _currentState { get; set; }

        public ApplicationConditionManager()
        {
            _currentState = ApplicationState.LOADING;
            _transitions = new Dictionary<ApplicationStateTransition, ApplicationState>
            {
                {new ApplicationStateTransition(ApplicationState.LOADING, ApplicationCommand.OPEN_MAIN_MENU),ApplicationState.MAIN_MENU},
                {new ApplicationStateTransition(ApplicationState.MAIN_MENU, ApplicationCommand.GAMEPLAY_START),ApplicationState.GAMEPLAY},
                {new ApplicationStateTransition(ApplicationState.LOADING, ApplicationCommand.GAMEPLAY_START),ApplicationState.GAMEPLAY},
                {new ApplicationStateTransition(ApplicationState.GAMEPLAY, ApplicationCommand.BACK_TO_PREVIOUS_STATE),ApplicationState.MAIN_MENU}
            };
        }

        public ApplicationState MoveNext(ApplicationCommand command)
        {
            ApplicationState applicationState = GetNext(command);
            if (applicationState != ApplicationState.NULL)
                _currentState = GetNext(command);
            return applicationState;
        }

        private ApplicationState GetNext(ApplicationCommand command)
        {
            ApplicationStateTransition transition = new ApplicationStateTransition(_currentState, command);
            if (!_transitions.TryGetValue(transition, out ApplicationState nextState))
                throw new Exception("Invalid transition: " + _currentState + " -> " + command);
            return nextState;
        }
    }
}