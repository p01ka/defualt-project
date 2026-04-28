using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using IdxZero.Application.Signals;
using IdxZero.Application.UI.ApplicationScreen;
using IdxZero.Services.Analytics;
using IdxZero.Services.ScreenSession;
using IdxZero.Services.ServicesStarter;
using IdxZero.Services.UserProperties;
using Zenject;

namespace IdxZero.Loading.Core
{
    public class GameLoader : IInitializable
    {
        private readonly SignalBus _signals;
        private readonly IServicesStarter _serviceStarter;
        private readonly IScreenSessionSetter _screenSessionSetter;
        private readonly ApplicationScreenAdapter _applicationScreenAdapter;

        private readonly IAnalyticsFacade _iAnalyticsFacade;
        private readonly IUserPropertiesFacade _userPropertiesFacade;

        public GameLoader(SignalBus signals,
                          IServicesStarter serviceStarter,
                          IScreenSessionSetter screenSessionSetter,
                          IAnalyticsFacade iAnalyticsFacade,
                          IUserPropertiesFacade userPropertiesFacade,
                          ApplicationScreenAdapter applicationScreenAdapter)
        {
            _signals = signals;
            _serviceStarter = serviceStarter;
            _screenSessionSetter = screenSessionSetter;
            _iAnalyticsFacade = iAnalyticsFacade;
            _userPropertiesFacade = userPropertiesFacade;
            _applicationScreenAdapter = applicationScreenAdapter;
        }

        public async void Initialize()
        {
            DOTween.SetTweensCapacity(500, 50);
            LeanTween.init(800);
            _screenSessionSetter.SetSessionScreen();

            List<UniTask> tasks = new List<UniTask>()
            {
                //Addressables.InitializeAsync(false).ToUniTask(), //UNCOMMENT IF ADDRESSABLES USES IN THE PROJECT//
                _serviceStarter.StartServices()
            };
            await _applicationScreenAdapter.ApplicationScreenFacade.ActiveLoadingBar(true);
            await UniTask.WhenAll(tasks);
            await _applicationScreenAdapter.ApplicationScreenFacade.FillLoadingBarWithServicesLoaded();
            LoadGame();
        }

        private void LoadGame()
        {
            _signals.TryFire<ApplicationSignals.OnApplicationLoaded>();
            _iAnalyticsFacade.LogSessionStartEvents();
        }
    }
}