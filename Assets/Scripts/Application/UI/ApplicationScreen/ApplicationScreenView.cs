using Cysharp.Threading.Tasks;
using IdxZero.Application.UI.ApplicationScreen.LoadingBar;
using IdxZero.Application.UI.ApplicationScreen.MockBannerPanel;
using IdxZero.Utils;
using IdxZero.Utils.Extensions;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace IdxZero.Application.UI.ApplicationScreen
{
    public class ApplicationScreenAdapter
    {
        public IApplicationScreenFacade ApplicationScreenFacade { get; private set; }

        public void SetApplicationScreen(IApplicationScreenFacade applicationScreenFacade)
        {
            ApplicationScreenFacade = applicationScreenFacade;
        }
    }

    public interface IApplicationScreenFacade
    {
        UniTask ActiveSplashScreen(bool active, bool withAnimation);

        UniTask ActiveLoadingBar(bool active);
        UniTask FillLoadingBarWithServicesLoaded();

        void ActiveLoadingWheelScreen(bool active);
        void ActiveAdsBannerPanel(bool activeAdsBannerPanel);
    }


    public class ApplicationScreenView : MonoBehaviour, IApplicationScreenFacade
    {
        [SerializeField]
        private GameObject _loadingWheelScreen;

        [SerializeField]
        private MockBannerPanelView _mockBannerPanelView;

        [SerializeField]
        private LoadingBarView _loadingBarView;

        [Header("SPLASH IMAGE")]
        [SerializeField] private Image _splashImage;

        [Inject]
        private void Construct(ApplicationScreenAdapter applicationScreenAdapter)
        {
            SetDefaults();
            applicationScreenAdapter.SetApplicationScreen(this);
        }

        private void SetDefaults()
        {
            _loadingBarView.SetDefaults();
            _mockBannerPanelView.SetDefaults();
            _loadingWheelScreen.gameObject.SetActive(false);
        }

        public async UniTask ActiveSplashScreen(bool active, bool withAnimation)
        {
            if (active)
            {
                await ShowSplashScreen(withAnimation);
            }
            else
            {
                await HideSplashScreen(withAnimation);
            }
        }

        public void ActiveLoadingWheelScreen(bool active)
        {
            _loadingWheelScreen.gameObject.SetActive(active);
        }

        public void ActiveAdsBannerPanel(bool activeAdsBannerPanel)
        {
            _mockBannerPanelView.ActiveAdsBannerPanel(activeAdsBannerPanel);
        }

        public async UniTask ActiveLoadingBar(bool active)
        {
            if (active)
            {
                _loadingBarView.ShowLoadingBarWithFillingAnimation();
            }
            else
            {
                await _loadingBarView.HideLoadingBarWithFillingAnimation();
            }
        }

        public async UniTask FillLoadingBarWithServicesLoaded()
        {
            await _loadingBarView.FillLoadingBarWithServicesLoaded();
        }

        private async UniTask ShowSplashScreen(bool withAnimation)
        {
            if (_splashImage.gameObject.activeSelf)
                return;
            LeanTween.cancel(_splashImage.gameObject);
            if (!withAnimation)
            {
                _splashImage.color = _splashImage.color.SetAlpha(1);
                _splashImage.gameObject.SetActive(true);
                await UniTask.DelayFrame(1);
            }
            else
            {
                _splashImage.color = _splashImage.color.SetAlpha(0);
                _splashImage.gameObject.SetActive(true);
                LeanTween.value(_splashImage.gameObject, 0, 1f, Consts.TRANSITION_TIME_CONTENT_SCREEN)
                    .setOnUpdate((x) => { _splashImage.color = _splashImage.color.SetAlpha(x); })
                    .setIgnoreTimeScale(true);
                await UniTask.WaitWhile(() => LeanTween.isTweening(_splashImage.gameObject) == true);
            }
        }

        private async UniTask HideSplashScreen(bool withAnimation)
        {
            await _loadingBarView.HideLoadingBarWithFillingAnimation();
            if (!withAnimation)
            {
                _splashImage.gameObject.SetActive(false);
                _splashImage.color = _splashImage.color.SetAlpha(0);
                await UniTask.DelayFrame(1);
            }
            else
            {
                _splashImage.color = _splashImage.color.SetAlpha(1);

                LeanTween.value(_splashImage.gameObject, 1f, 0, Consts.TRANSITION_TIME_CONTENT_SCREEN)
                    .setOnUpdate((x) => { _splashImage.color = _splashImage.color.SetAlpha(x); })
                    .setOnComplete(() => { _splashImage.gameObject.SetActive(false); })
                    .setIgnoreTimeScale(true);
                await UniTask.WaitWhile(() => LeanTween.isTweening(_splashImage.gameObject) == true);
            }
        }
    }
}