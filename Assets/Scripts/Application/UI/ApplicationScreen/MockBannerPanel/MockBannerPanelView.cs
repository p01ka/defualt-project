using UnityEngine;

namespace IdxZero.Application.UI.ApplicationScreen.MockBannerPanel
{
    public class MockBannerPanelView : MonoBehaviour
    {
        [SerializeField] private RectTransform _mockBannerPanel;
        [SerializeField] private RectTransform _mockBannerPanelObj;

        private bool _mockBannerPanelAdjusted;

        public void SetDefaults()
        {
            _mockBannerPanelObj.gameObject.SetActive(false);
#if UNITY_IOS
            _mockBannerPanelObj.gameObject.AddComponent<Crystal.SafeArea>();
#endif
        }

        public void ActiveAdsBannerPanel(bool activeAdsBannerPanel)
        {
            _mockBannerPanelObj.gameObject.SetActive(activeAdsBannerPanel);
            if (!_mockBannerPanelAdjusted && activeAdsBannerPanel)
            {
                AdjustAdsBannerByDeviceType();
                _mockBannerPanelAdjusted = true;
            }
        }

        private void AdjustAdsBannerByDeviceType()
        {
            _mockBannerPanel.anchorMax = new Vector2(1,
            Utils.CUtils.GetAdsPanelAnchorMax(_mockBannerPanelObj.anchorMax.y - _mockBannerPanelObj.anchorMin.y));
#if UNITY_ANDROID
            _mockBannerPanel.anchorMin = new Vector2(0, 0);
#elif UNITY_IOS
            _mockBannerPanel.anchorMin = new Vector2(0, -0.1f);
#endif
        }
    }
}