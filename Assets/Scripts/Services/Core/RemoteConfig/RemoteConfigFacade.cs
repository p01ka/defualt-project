using System;

namespace IdxZero.Services.RemoteConfig
{
    public class RemoteConfigFacade : IRemoteConfigDataKeeper, IRemoteConfigService
    {
        private readonly IRemoteConfigStrategy _remoteConfigStrategy;
        private readonly RemoteConfigDefaultValues _remoteConfigDefaultValues;

        public RemoteConfigFacade(IRemoteConfigStrategy remoteConfigStrategy,
            RemoteConfigDefaultValues remoteConfigDefaultValues)
        {
            _remoteConfigStrategy = remoteConfigStrategy;
            _remoteConfigDefaultValues = remoteConfigDefaultValues;
        }

        public void InitializeRemoteConfigService(Action remoteConfigFetchedCallback)
        {
            _remoteConfigStrategy.InitializeStrategy(remoteConfigFetchedCallback);
        }

        public string GetInterstitialShowingDetailsJson()
        {
            string interstitialDetailsJson = _remoteConfigStrategy.GetStringWithKey(RemoteConfigTextKeys.InterstitialDetailsJsonKey,
                _remoteConfigDefaultValues.DefaultInterstitialDetailsJson);
            return interstitialDetailsJson;
        }
    }
}