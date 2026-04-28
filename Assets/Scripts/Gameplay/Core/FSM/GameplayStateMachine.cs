using System;
using System.Collections.Generic;
using IdxZero.Application.Model;
using IdxZero.Base.States;
using UnityEngine;
using Zenject;

namespace IdxZero.Gameplay.States
{
    public enum GameplayStateMachineCommand
    {
        NULL,
        GAMEPLAY_PRELOADED,
        STATE_A_FINISHED,
        STATE_B_FINISHED,
        GAMEPLAY_RESTARTED
    }

    public enum GameplayStateMachineState
    {
        NULL,
        PREGAMEPLAY,
        STATE_A,
        STATE_B,
        #region DEBUG_STATES
        CAMERA_CONTROL_DEBUG_STATE_DEBUG,
        STATE_B_DEBUG,
        #endregion DEBUG_STATES
    }

    public class GameplayStateMachineConditionManager
    {
        private readonly Dictionary<GameplayStateMachineStateTransition, GameplayStateMachineState> _transitions;
        private readonly Settings _settings;

        private GameplayStateMachineState _currentState { get; set; }
        private GameplayStateMachineState _startState;

        private StateConfig _currentStateConfig;

        public GameplayStateMachineConditionManager(Settings settings, IGameplayTransactionModelGetter gameplayTransaction)
        {
            _settings = settings;

#if RELEASE_MODE
            _currentStateConfig = StateConfig.GENERAL_CONFIG;
#else
            if (gameplayTransaction.UseCustomerGameplayStateConfig)
                _currentStateConfig = gameplayTransaction.StateConfig;
            else
            _currentStateConfig = _settings.GameplayOnlyDebugStateConfig;
#endif
            _transitions = GetCurrentTransitionDict();
        }

        public GameplayStateMachineState GetStartState()
        {
            _currentState = _startState;
            return _startState;
        }

        public GameplayStateMachineState MoveNext(GameplayStateMachineCommand command)
        {
            GameplayStateMachineState state = GetNext(command);
            if (state != GameplayStateMachineState.NULL)
                _currentState = GetNext(command);
            return state;
        }

        private Dictionary<GameplayStateMachineStateTransition, GameplayStateMachineState> GetCurrentTransitionDict()
        {
            switch (_currentStateConfig)
            {
                case StateConfig.GENERAL_CONFIG:
                default:
                    _startState = GameplayStateMachineState.PREGAMEPLAY;
                    return new Dictionary<GameplayStateMachineStateTransition, GameplayStateMachineState>
                    {
                        {new GameplayStateMachineStateTransition(GameplayStateMachineState.PREGAMEPLAY, GameplayStateMachineCommand.GAMEPLAY_PRELOADED),GameplayStateMachineState.STATE_A},
                        {new GameplayStateMachineStateTransition(GameplayStateMachineState.STATE_A, GameplayStateMachineCommand.STATE_A_FINISHED),GameplayStateMachineState.STATE_B}
                    };

                #region DEBUG_STATES_TRANSITIONS

                case StateConfig.CAMERA_CONTROL:
                    _startState = GameplayStateMachineState.CAMERA_CONTROL_DEBUG_STATE_DEBUG;
                    return new Dictionary<GameplayStateMachineStateTransition, GameplayStateMachineState>
                    {
                        {new GameplayStateMachineStateTransition(GameplayStateMachineState.CAMERA_CONTROL_DEBUG_STATE_DEBUG, GameplayStateMachineCommand.GAMEPLAY_PRELOADED),GameplayStateMachineState.NULL},
                    };

                case StateConfig.STATE_B_ONLY:
                    _startState = GameplayStateMachineState.STATE_B_DEBUG;
                    return new Dictionary<GameplayStateMachineStateTransition, GameplayStateMachineState>
                    {
                        {new GameplayStateMachineStateTransition(GameplayStateMachineState.STATE_B_DEBUG, GameplayStateMachineCommand.GAMEPLAY_PRELOADED),GameplayStateMachineState.STATE_B},
                    };


                    #endregion DEBUG_STATES_TRANSITIONS
            }
        }

        private GameplayStateMachineState GetNext(GameplayStateMachineCommand command)
        {
            GameplayStateMachineStateTransition transition = new GameplayStateMachineStateTransition(_currentState, command);
            if (!_transitions.TryGetValue(transition, out GameplayStateMachineState nextState))
                throw new Exception("Invalid transition: " + _currentState + " -> " + command);
            return nextState;
        }

        [Serializable]
        public class Settings
        {
            [Header("DEBUG")]
            public StateConfig GameplayOnlyDebugStateConfig;
        }

        public enum StateConfig
        {
            GENERAL_CONFIG,
            STATE_B_ONLY,
            CAMERA_CONTROL
        }
    }

    public class GameplayStateMachineStateTransition : StateTransition<GameplayStateMachineState, GameplayStateMachineCommand>
    {
        public GameplayStateMachineStateTransition(GameplayStateMachineState currentState, GameplayStateMachineCommand currentCommand) : base(currentState, currentCommand)
        {
        }
    }

    public class GameplayStateMachineStatesFactory
    {
        private readonly Dictionary<GameplayStateMachineState, PlaceholderFactory<IBaseState>> _factories;

        public GameplayStateMachineStatesFactory(PregameplayState.Factory pregameplayStateFactory,
                                                 State_A.Factory stateAFactory,
                                                 State_B.Factory stateBFactory,

                                                 CameraControlDebugState.Factory cameraControlDebugStateFactory,
                                                 State_B_Debug.Factory StateBDebugFactory)
        {
            _factories = new Dictionary<GameplayStateMachineState, PlaceholderFactory<IBaseState>>
            {
                {GameplayStateMachineState.PREGAMEPLAY,pregameplayStateFactory},
                {GameplayStateMachineState.STATE_A,stateAFactory},
                {GameplayStateMachineState.STATE_B,stateBFactory},

                #region DEBUG_STATES_FACTORY
                {GameplayStateMachineState.STATE_B_DEBUG,StateBDebugFactory},
                {GameplayStateMachineState.CAMERA_CONTROL_DEBUG_STATE_DEBUG,cameraControlDebugStateFactory}
                #endregion DEBUG_STATES_FACTORY
            };
        }

        public IBaseState GetState(GameplayStateMachineState state)
        {
            if (_factories.TryGetValue(state, out var factory))
            {
                return factory.Create();
            }
            throw new System.Exception($"No state factory with state {state}");
        }
    }

    public class GameplayStateMachineStatesController : StateController, IDisposable, IInitializable
    {
        private readonly SignalBus _signals;
        private readonly GameplayStateMachineConditionManager _conditionManager;
        private readonly GameplayStateMachineStatesFactory _statesFactory;

        public GameplayStateMachineStatesController(GameplayStateMachineConditionManager conditionManager,
                                                    SignalBus signals,
                                                    GameplayStateMachineStatesFactory statesFactory)
        {
            _conditionManager = conditionManager;
            _signals = signals;
            _statesFactory = statesFactory;

            Subscribe();
        }

        public void Initialize()
        {
            SetStartState();
        }

        private void SetStartState()
        {
            GameplayStateMachineState startState = _conditionManager.GetStartState();
            SetNextState(startState);
        }

        private void SetNextStateByCommand(GameplayStateMachineCommand command)
        {
            GameplayStateMachineState state = _conditionManager.MoveNext(command);
            if (state != GameplayStateMachineState.NULL)
                SetNextState(state);
        }

        private void SetNextState(GameplayStateMachineState gameState)
        {
            IBaseState baseState = _statesFactory.GetState(gameState);
            SetState(baseState);
        }

        private void TryToSetNextStateWithGameplayPreloadedCommand()
        {
            SetNextStateByCommand(GameplayStateMachineCommand.GAMEPLAY_PRELOADED);
        }

        private void TryToSetNextStateWithStateAFinishedCommand()
        {
            SetNextStateByCommand(GameplayStateMachineCommand.STATE_A_FINISHED);
        }

        private void TryToSetNextStateWithStateBFinishedCommand()
        {
            SetNextStateByCommand(GameplayStateMachineCommand.STATE_B_FINISHED);
        }

        private void TryToSetNextStateWithGameplayRestartedCommand()
        {
            SetNextStateByCommand(GameplayStateMachineCommand.GAMEPLAY_RESTARTED);
        }

        private void Subscribe()
        {
            _signals.Subscribe<GameplayStateMachineStatesSignals.OnGameplayPreloaded>(TryToSetNextStateWithGameplayPreloadedCommand);
            _signals.Subscribe<GameplayStateMachineStatesSignals.OnStateAFinished>(TryToSetNextStateWithStateAFinishedCommand);
            _signals.Subscribe<GameplayStateMachineStatesSignals.OnStateBFinished>(TryToSetNextStateWithStateBFinishedCommand);
            _signals.Subscribe<GameplayStateMachineStatesSignals.OnGameplayRestarted>(TryToSetNextStateWithGameplayRestartedCommand);
        }

        private void Unsubscribe()
        {
            _signals.TryUnsubscribe<GameplayStateMachineStatesSignals.OnGameplayPreloaded>(TryToSetNextStateWithGameplayPreloadedCommand);
            _signals.TryUnsubscribe<GameplayStateMachineStatesSignals.OnStateAFinished>(TryToSetNextStateWithStateAFinishedCommand);
            _signals.TryUnsubscribe<GameplayStateMachineStatesSignals.OnStateBFinished>(TryToSetNextStateWithStateBFinishedCommand);
            _signals.TryUnsubscribe<GameplayStateMachineStatesSignals.OnGameplayRestarted>(TryToSetNextStateWithGameplayRestartedCommand);
        }

        public void Dispose()
        {
            Unsubscribe();
        }
    }

    public class GameplayStateMachineStatesSignals
    {
        public class OnGameplayPreloaded { }

        public class OnStateAFinished { }

        public class OnStateBFinished { }

        public class OnGameplayRestarted { }
    }

    public class GameplayStateMachineStatesInstaller : Installer<GameplayStateMachineStatesInstaller>
    {
        public override void InstallBindings()
        {
            InstallController();
            InstallStates();
            InstallSignals();
        }

        private void InstallController()
        {
            Container.Bind(typeof(IInitializable), typeof(IDisposable))
               .To<GameplayStateMachineStatesController>()
               .AsSingle();

            Container.Bind<GameplayStateMachineConditionManager>()
                .ToSelf()
                .AsSingle()
                .WhenInjectedInto<GameplayStateMachineStatesController>();
        }

        private void InstallStates()
        {
            InstallFactory<PregameplayState, PregameplayState.Factory>();
            InstallFactory<State_A, State_A.Factory>();
            InstallFactory<State_B, State_B.Factory>();

            #region DEBUG_STATES
            InstallFactory<State_B_Debug, State_B_Debug.Factory>();
            InstallFactory<CameraControlDebugState, CameraControlDebugState.Factory>();
            #endregion DEBUG_STATES

            Container.Bind<GameplayStateMachineStatesFactory>().ToSelf().AsSingle();
        }

        private void InstallFactory<T, TFactory>() where T : IBaseState
                                                   where TFactory : PlaceholderFactory<IBaseState>
        {
            Container.BindFactory<IBaseState, TFactory>()
                     .To<T>()
                     .WhenInjectedInto<GameplayStateMachineStatesFactory>();
        }

        private void InstallSignals()
        {
            Container.DeclareSignal<GameplayStateMachineStatesSignals.OnGameplayPreloaded>().OptionalSubscriber();
            Container.DeclareSignal<GameplayStateMachineStatesSignals.OnStateAFinished>().OptionalSubscriber();
            Container.DeclareSignal<GameplayStateMachineStatesSignals.OnStateBFinished>().OptionalSubscriber();
            Container.DeclareSignal<GameplayStateMachineStatesSignals.OnGameplayRestarted>().OptionalSubscriber();
        }
    }
}