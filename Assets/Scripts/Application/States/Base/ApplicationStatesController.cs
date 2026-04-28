using System;
using IdxZero.Application.Signals;
using IdxZero.Base.States;
using Zenject;

namespace IdxZero.Application.States.Base
{
    public class ApplicationStatesController : StateController, IInitializable, IDisposable
    {
        private readonly SignalBus _signals;
        private readonly ApplicationConditionManager _conditionManager;
        private readonly ApplicationStatesFactory _statesFactory;

        public ApplicationStatesController(ApplicationConditionManager conditionManager,
                                           SignalBus signals,
                                           ApplicationStatesFactory statesFactory)
        {
            _conditionManager = conditionManager;
            _signals = signals;
            _statesFactory = statesFactory;

            Subscribe();
        }

        public void Initialize()
        {
            SetNextState(ApplicationState.LOADING);
        }

        private void SetNextStateByCommand(ApplicationCommand command)
        {
            ApplicationState applicationState = _conditionManager.MoveNext(command);
            if (applicationState != ApplicationState.NULL)
                SetNextState(applicationState);
        }

        private void SetNextState(ApplicationState gameState)
        {
            IBaseState baseState = _statesFactory.GetState(gameState);
            SetState(baseState);
        }

        private void GameLoaded()
        {
#if RELEASE_MODE
            SetNextStateByCommand(ApplicationCommand.GAMEPLAY_START);
#else
            SetNextStateByCommand(ApplicationCommand.OPEN_MAIN_MENU);
#endif
        }

        private void StartGameplay()
        {
            SetNextStateByCommand(ApplicationCommand.GAMEPLAY_START);
        }

        private void BackToPreviousState()
        {
            SetNextStateByCommand(ApplicationCommand.BACK_TO_PREVIOUS_STATE);
        }

        private void Subscribe()
        {
            _signals.Subscribe<ApplicationSignals.OnApplicationLoaded>(GameLoaded);
            _signals.Subscribe<ApplicationSignals.OnStartGameplay>(StartGameplay);
            _signals.Subscribe<ApplicationSignals.OnBackToPreviousState>(BackToPreviousState);
        }

        private void Unsubscribe()
        {
            _signals.Unsubscribe<ApplicationSignals.OnApplicationLoaded>(GameLoaded);
            _signals.Unsubscribe<ApplicationSignals.OnStartGameplay>(StartGameplay);
            _signals.Unsubscribe<ApplicationSignals.OnBackToPreviousState>(BackToPreviousState);
        }

        public void Dispose()
        {
            Unsubscribe();
        }
    }
}