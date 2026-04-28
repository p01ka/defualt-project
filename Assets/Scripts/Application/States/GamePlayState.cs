using System;
using IdxZero.Application.Settings;
using IdxZero.Application.Signals;
using IdxZero.Application.UI.ApplicationScreen;
using IdxZero.Base.States;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace IdxZero.Application.States
{
    public class GamePlayState : IBaseState
    {
        private readonly MainConfig _mainConfig;
        private readonly SignalBus _signals;
        private readonly ApplicationScreenAdapter _applicationScreenAdapter;

        public GamePlayState(MainConfig mainConfig,
                             SignalBus signals,
                             ApplicationScreenAdapter applicationScreenAdapter)
        {
            _mainConfig = mainConfig;
            _signals = signals;
            _applicationScreenAdapter = applicationScreenAdapter;
        }

        public void OnEnter(Action releasePreviousStateCallback)
        {
            _applicationScreenAdapter.ApplicationScreenFacade.ActiveSplashScreen(true, true);
            releasePreviousStateCallback?.Invoke();
            LeanTween.delayedCall(Utils.Consts.TRANSITION_TIME_CONTENT_SCREEN, () =>
            {
                _applicationScreenAdapter.ApplicationScreenFacade.ActiveLoadingBar(true);
                AsyncOperation async = SceneManager.LoadSceneAsync(_mainConfig.GameplaySceneName, LoadSceneMode.Additive);
                async.completed += (x) =>
                {
                    Loaded(x);
                    _applicationScreenAdapter.ApplicationScreenFacade.ActiveSplashScreen(false, true);
                };
                async.allowSceneActivation = true;
            });
        }

        public void OnExit()
        {
        }

        public void OnRelease()
        {
            AsyncOperation async = SceneManager.UnloadSceneAsync(_mainConfig.GameplaySceneName);
            async.completed += (x) =>
            {
                _signals.TryFire(new MainMenuStateSignals.OnActiveMainMenuInteraction(true));
                AssetBundle.UnloadAllAssetBundles(true);
                Resources.UnloadUnusedAssets();
                GC.Collect();
            };
        }

        private void Loaded(AsyncOperation obj)
        {
            Scene scene = SceneManager.GetSceneByName(_mainConfig.GameplaySceneName);
            SceneManager.SetActiveScene(scene);
        }

        #region Factory

        public class Factory : PlaceholderFactory<IBaseState>
        {
        }

        #endregion Factory
    }
}