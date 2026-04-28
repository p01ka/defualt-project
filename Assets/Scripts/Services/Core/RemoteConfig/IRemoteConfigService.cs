using System;

namespace IdxZero.Services.RemoteConfig
{
    public interface IRemoteConfigService
    {
        void InitializeRemoteConfigService(Action remoteConfigFetchedCallback);
    }
}