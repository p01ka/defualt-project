namespace IdxZero.Services.Ads
{
    public class AdsUtilsStarter
    {
        private readonly IInterstitialShowingResolver _interstitialResolver;
        private readonly IRewardStatusKeeper _rewardStatusKeeper;

        public AdsUtilsStarter(IInterstitialShowingResolver interstitialResolver,
                               IRewardStatusKeeper rewardStatusKeeper)
        {
            _interstitialResolver = interstitialResolver;
            _rewardStatusKeeper = rewardStatusKeeper;
        }

        public void StartAdsUtils()
        {
            _interstitialResolver.SetResolver();
            _rewardStatusKeeper.Init();
        }
    }
}