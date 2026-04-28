using IdxZero.Application.Model;
using IdxZero.Application.Signals;
using IdxZero.Application.UI.ApplicationScreen;
using IdxZero.Base.MVP;
using IdxZero.DataBase;
using IdxZero.Gameplay.States;
using Zenject;

namespace IdxZero.MainMenu.UI.MainMenuScreen
{
    public class MainMenuScreenPresenter : BasePresenter
    {
        private readonly IMainMenuScreenView _view;
        private readonly SignalBus _signals;
        private readonly IGameplayTransactionModelSetter _gameplayTransaction;
        private readonly IUserDatabaseManager _userDatabaseManager;
        private readonly ApplicationScreenAdapter _applicationScreenAdapter;

        public MainMenuScreenPresenter(IMainMenuScreenView view,
                                       SignalBus signals,
                                       IGameplayTransactionModelSetter gameplayTransaction,
                                       IUserDatabaseManager userDatabaseManager,
                                       ApplicationScreenAdapter applicationScreenAdapter)
        {
            _view = view;
            _signals = signals;
            _gameplayTransaction = gameplayTransaction;
            _userDatabaseManager = userDatabaseManager;
            _applicationScreenAdapter = applicationScreenAdapter;

            _signals.Subscribe<MainMenuStateSignals.OnShowMainMenu>(ShowMainMenuScreen);
            _signals.Subscribe<MainMenuStateSignals.OnHideMainMenu>(HideMainMenuScreen);
        }

        private void HideMainMenuScreen()
        {
            _view.ActiveScreen(false);
        }

        private void ShowMainMenuScreen(MainMenuStateSignals.OnShowMainMenu args)
        {
            _view.ActiveScreen(true);
            args.MainMenuShowedCallback?.Invoke();
            _applicationScreenAdapter.ApplicationScreenFacade.ActiveSplashScreen(false, true);
        }

        private void TryToStartGameplayWithGeneralConfig()
        {
            _gameplayTransaction.StateConfig = GameplayStateMachineConditionManager.StateConfig.GENERAL_CONFIG;
            TryToStartGameplay();
        }

        private void TryToStartGameplayWithSelectedConfig()
        {
            _gameplayTransaction.StateConfig = _view.GetCurrentStateConfig();
            TryToStartGameplay();
        }

        private void TryToStartGameplay()
        {
            _gameplayTransaction.UseCustomerGameplayStateConfig = true;
            int currentTimescale = _view.GetDebugTimescale();
            UnityEngine.Time.timeScale = (float)currentTimescale;
            _signals.TryFire<ApplicationSignals.OnStartGameplay>();
        }

        private void ShowAdsDebug()
        {
            _signals.TryFire<Services.Signals.ServicesSignals.OnShowAdsDebugger>();
        }

        private void ClearProgress()
        {
            _userDatabaseManager.ClearDatabase();
        }

        private void OnUseDebugMode(bool togleOn)
        {
            _gameplayTransaction.UseDebugMode = togleOn;
        }

        protected override void SubscribeOnViewEvents()
        {
            _view.OnStartGameplayButtonPressed += TryToStartGameplayWithGeneralConfig;
            _view.OnStartGameplayStateConfigButtonPressed += TryToStartGameplayWithSelectedConfig;
            _view.OnUseDebugModeToggle += OnUseDebugMode;
            _view.OnShowAdsDebugButtonPressed += ShowAdsDebug;
            _view.OnClearProgressButtonPressed += ClearProgress;
        }

        protected override void UnsubscribeOnViewEvents()
        {
            _view.OnStartGameplayButtonPressed -= TryToStartGameplayWithGeneralConfig;
            _view.OnStartGameplayStateConfigButtonPressed -= TryToStartGameplayWithSelectedConfig;
            _view.OnUseDebugModeToggle -= OnUseDebugMode;
            _view.OnShowAdsDebugButtonPressed -= ShowAdsDebug;
            _view.OnClearProgressButtonPressed -= ClearProgress;
        }

        public override void Dispose()
        {
            _signals.TryUnsubscribe<MainMenuStateSignals.OnShowMainMenu>(ShowMainMenuScreen);
            _signals.TryUnsubscribe<MainMenuStateSignals.OnHideMainMenu>(HideMainMenuScreen);
        }

        #region Factory

        public class Factory : PlaceholderFactory<IMainMenuScreenView, BasePresenter>
        {
        }

        #endregion Factory
    }

}





