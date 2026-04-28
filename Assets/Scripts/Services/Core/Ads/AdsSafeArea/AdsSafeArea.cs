using System;
using Crystal;
using Cysharp.Threading.Tasks;
using IdxZero.Utils;
using UnityEngine;
using Zenject;

namespace IdxZero.Services.Ads
{
    public class AdsSafeArea : SafeAreaBase, IDisposable
    {
        private IAdsBannerShowingGetter _bannerShowingGetter;
        private Vector2 _bannerAnchorHeight;

        [Inject]
        private void Construct(IAdsBannerShowingGetter bannerShowingGetter)
        {
            _bannerShowingGetter = bannerShowingGetter;
            _bannerShowingGetter.OnBannerShowingChanged += CalculateBannerHeight;

            CalculateBannerHeight();
        }

        protected override void ApplySafeArea(Rect r)
        {
            LastSafeArea = r;
        }

        private async void CalculateBannerHeight()
        {
            _bannerAnchorHeight = _bannerShowingGetter.IsBannerShowing
                ? new Vector2(1, CUtils.GetAdsPanelAnchorMax(1f))
                : Vector2.zero;

            BaseRefresh();
            await UniTask.DelayFrame(1);
            Refresh();
        }

        public void ForceDisableBanner()
        {
            _bannerShowingGetter.OnBannerShowingChanged -= CalculateBannerHeight;
            _bannerAnchorHeight = Vector2.zero;
        }

        private void LateUpdate()
        {
            if (CurrentFramesCount < UpdateFramesCount)
                Refresh();
        }

        private void Refresh()
        {
            var r = LastSafeArea;

            // Ignore x-axis?
            if (!ConformX)
            {
                r.x = 0;
                r.width = Screen.width;
            }

            // Ignore y-axis?
            if (!ConformY)
            {
                r.y = 0;
                r.height = Screen.height;
            }

            // Check for invalid screen startup state on some Samsung devices (see below)
            if (Screen.width > 0 && Screen.height > 0)
            {
                // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
                Vector2 anchorMin = r.position;
                Vector2 anchorMax = r.position + r.size;
                anchorMin.x /= Screen.width;
                anchorMin.y /= Screen.height;
                anchorMax.x /= Screen.width;
                anchorMax.y /= Screen.height;

                anchorMin.y = Mathf.Max(anchorMin.y, _bannerAnchorHeight.y);
                if (_ignoreTopSafeArea)
                {
                    anchorMax.y = 1;
                }

                // Fix for some Samsung devices (e.g. Note 10+, A71, S20) where Refresh gets called twice and the first time returns NaN anchor coordinates
                // See https://forum.unity.com/threads/569236/page-2#post-6199352
                if (anchorMin.x >= 0 && anchorMin.y >= 0 && anchorMax.x >= 0 && anchorMax.y >= 0)
                {
                    if (Panel == null) return;
                    Panel.anchorMin = anchorMin;
                    Panel.anchorMax = anchorMax;
                }
            }
        }

        public void Dispose()
        {
            _bannerShowingGetter.OnBannerShowingChanged -= CalculateBannerHeight;
        }
    }
}