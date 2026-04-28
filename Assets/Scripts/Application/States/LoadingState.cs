using System;
using IdxZero.Application.Settings;
using IdxZero.Base.States;
using UnityEngine.SceneManagement;
using Zenject;

namespace IdxZero.Application.States
{
    public class LoadingState : IBaseState
    {
        private readonly MainConfig _mainConfig;

        public LoadingState(MainConfig mainConfig)
        {
            _mainConfig = mainConfig;
        }

        public void OnEnter(Action releasePreviousStateCallback)
        {
            SceneManager.LoadSceneAsync(_mainConfig.LoadingSceneName, LoadSceneMode.Additive);
        }

        public void OnExit()
        {
            SceneManager.UnloadSceneAsync(_mainConfig.LoadingSceneName);
        }

        public void OnRelease()
        {
        }

        #region Factory

        public class Factory : PlaceholderFactory<IBaseState>
        {
        }

        #endregion Factory
    }
}