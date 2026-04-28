using IdxZero.Base.States;
using System.Collections.Generic;
using Zenject;

namespace IdxZero.Application.States.Base
{
    public class ApplicationStatesFactory
    {
        private readonly Dictionary<ApplicationState, PlaceholderFactory<IBaseState>> _factories;

        public ApplicationStatesFactory(LoadingState.Factory loadingStateFactory,
                                        MainMenuState.Factory mainMenuStateFactory,
                                        GamePlayState.Factory gamePlayStateFactory)
        {
            _factories = new Dictionary<ApplicationState, PlaceholderFactory<IBaseState>>
            {
                {ApplicationState.LOADING,loadingStateFactory},
                {ApplicationState.MAIN_MENU,mainMenuStateFactory},
                {ApplicationState.GAMEPLAY,gamePlayStateFactory}
            };
        }

        public IBaseState GetState(ApplicationState state)
        {
            if (_factories.TryGetValue(state, out var factory))
            {
                return factory.Create();
            }
            return null;
        }
    }
}