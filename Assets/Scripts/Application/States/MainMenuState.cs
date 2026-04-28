using System;
using IdxZero.Application.Settings;
using IdxZero.Application.Signals;
using IdxZero.Application.UI.ApplicationScreen;
using IdxZero.Base.States;
using UnityEngine.SceneManagement;
using Zenject;

namespace IdxZero.Application.States
{
    public class MainMenuState : IBaseState
    {
        private readonly SignalBus _signals;
        private readonly MainConfig _mainConfig;
        private readonly ApplicationScreenAdapter _applicationScreenAdapter;

        private static bool _isMainMenuSceneLoaded;

        public MainMenuState(SignalBus signals,
                             MainConfig mainConfig,
                             ApplicationScreenAdapter applicationScreenAdapter)
        {
            _signals = signals;
            _mainConfig = mainConfig;
            _applicationScreenAdapter = applicationScreenAdapter;
        }

        public void OnEnter(Action releasePreviousStateCallback)
        {
            if (_isMainMenuSceneLoaded)
            {
                void AdditionalCallback()
                {
                    releasePreviousStateCallback?.Invoke();
                }
                _signals.TryFire(new MainMenuStateSignals.OnShowMainMenu(AdditionalCallback));

                return;
            }
            var async = SceneManager.LoadSceneAsync(_mainConfig.MainSceneName, LoadSceneMode.Additive);
            async.completed += (x) =>
            {
                releasePreviousStateCallback?.Invoke();
                _applicationScreenAdapter.ApplicationScreenFacade.ActiveSplashScreen(false, true);
            };

            _isMainMenuSceneLoaded = true;
        }

        public void OnExit()
        {
            _signals.TryFire(new MainMenuStateSignals.OnActiveMainMenuInteraction(false));
        }

        public void OnRelease()
        {
            _signals.TryFire<MainMenuStateSignals.OnHideMainMenu>();
        }

        #region Factory

        public class Factory : PlaceholderFactory<IBaseState>
        {
        }

        #endregion Factory
    }
}