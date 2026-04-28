using System;
using UnityEngine;

namespace IdxZero.Services.RemoteConfig
{
    public interface IRemoteConfigStrategy
    {
        void InitializeStrategy(Action onInitializedCallback);

        string GetStringWithKey(string key, string defaultValue);
        bool GetBoolWithKey(string key, bool defaultValue);
        int GetIntWithKey(string key, int defaultValue);
        float GetFloatWithKey(string key, float defaultValue);
    }

    public interface IRemoteConfigDataKeeper
    {
        string GetInterstitialShowingDetailsJson();
    }

    [Serializable]
    public class RemoteConfigDefaultValues
    {
        [TextArea(8, 20)] public string DefaultInterstitialDetailsJson;
    }
}