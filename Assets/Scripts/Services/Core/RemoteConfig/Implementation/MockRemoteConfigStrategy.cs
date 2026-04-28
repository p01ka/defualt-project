using System;

namespace IdxZero.Services.RemoteConfig
{
    public class MockRemoteConfigStrategy : IRemoteConfigStrategy
    {
        private readonly RemoteConfigDefaultValues _remoteConfigDefaultValues;

        public MockRemoteConfigStrategy(RemoteConfigDefaultValues remoteConfigDefaultValues)
        {
            _remoteConfigDefaultValues = remoteConfigDefaultValues;
        }

        public void InitializeStrategy(Action onInitializedCallback)
        {
            onInitializedCallback?.Invoke();
        }

        public string GetStringWithKey(string key, string defaultValue)
        {
            return defaultValue;
        }

        public bool GetBoolWithKey(string key, bool defaultValue)
        {
            return defaultValue;
        }

        public int GetIntWithKey(string key, int defaultValue)
        {
            return defaultValue;
        }

        public float GetFloatWithKey(string key, float defaultValue)
        {
            return defaultValue;
        }
    }
}