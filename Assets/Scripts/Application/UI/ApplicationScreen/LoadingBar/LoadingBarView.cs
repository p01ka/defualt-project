using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace IdxZero.Application.UI.ApplicationScreen.LoadingBar
{
    public class LoadingBarView : MonoBehaviour
    {
        [SerializeField] private Image _amountLoadingImage;
        [SerializeField] private GameObject _loadingBarContainer;
        private const float ServicesMaxFillAmount = .6f;
        private const float DefaultFillingAnimationTime = 10f;
        private const float FillToEndAnimationTime = 0.5f;

        public void SetDefaults()
        {
            if (LeanTween.isTweening(_amountLoadingImage.gameObject))
                LeanTween.cancel(_amountLoadingImage.gameObject);

            _loadingBarContainer.SetActive(false);
        }

        public void ShowLoadingBarWithFillingAnimation()
        {
            if (LeanTween.isTweening(_amountLoadingImage.gameObject))
                return;

            _amountLoadingImage.fillAmount = 0;
            RunFillingAnimation(ServicesMaxFillAmount);
        }

        public async UniTask FillLoadingBarWithServicesLoaded()
        {
            if (LeanTween.isTweening(_amountLoadingImage.gameObject))
                LeanTween.cancel(_amountLoadingImage.gameObject);

            var startValue = _amountLoadingImage.fillAmount;
            var endValue = ServicesMaxFillAmount;
            LeanTween.value(_amountLoadingImage.gameObject, startValue, endValue, FillToEndAnimationTime)
                .setOnUpdate(value =>
                {
                    _amountLoadingImage.fillAmount = value;
                });
            await UniTask.WaitWhile(() => LeanTween.isTweening(_amountLoadingImage.gameObject) == true);
            await UniTask.DelayFrame(1);
            RunFillingAnimation();
        }

        public async UniTask HideLoadingBarWithFillingAnimation()
        {
            if (_loadingBarContainer.activeSelf)
            {
                if (LeanTween.isTweening(_amountLoadingImage.gameObject))
                    LeanTween.cancel(_amountLoadingImage.gameObject);

                var startValue = _amountLoadingImage.fillAmount;

                LeanTween.value(_amountLoadingImage.gameObject, startValue, 1, FillToEndAnimationTime)
                    .setOnUpdate(value =>
                    {
                        _amountLoadingImage.fillAmount = value;
                    })
                    .setOnComplete(() =>
                    {
                        if (_loadingBarContainer.activeSelf)
                            _loadingBarContainer.gameObject.SetActive(false);
                    })
                    .setIgnoreTimeScale(true);
                await UniTask.WaitWhile(() => LeanTween.isTweening(_amountLoadingImage.gameObject) == true);
            }
            else
            {
                await UniTask.DelayFrame(1);
            }
            _loadingBarContainer.SetActive(false);
        }

        private void RunFillingAnimation(float endValue = 1f)
        {
            var startValue = _amountLoadingImage.fillAmount;
            _loadingBarContainer.SetActive(true);
            LeanTween.value(_amountLoadingImage.gameObject, startValue, endValue, DefaultFillingAnimationTime)
                .setOnUpdate(value =>
                {
                    _amountLoadingImage.fillAmount = value;
                })
                .setIgnoreTimeScale(true);
        }
    }
}