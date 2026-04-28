using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IdxZero.Services.ActivityChecker
{
    public interface IUserActivityChecker
    {
        event Action OnTimeout;
        void InitializeActivityChecker(int longUserInactivityTimeSpan);
        void ActiveChecker(bool active);
    }

    public class UserActivityChecker : IUserActivityChecker
    {
        public event Action OnTimeout;
        private bool _isInitialized;

        private bool _isActive;

        private float _longUserInactivityTimeSpan;
        private float _lastActivityTime;

        public void InitializeActivityChecker(int longUserInactivityTimeSpan)
        {
            _longUserInactivityTimeSpan = (float)longUserInactivityTimeSpan;
            _lastActivityTime = Time.unscaledTime;
            _isInitialized = true;
            TimingRoutine();
        }

        public void ActiveChecker(bool active)
        {
            _isActive = active;
            if (active)
            {
                _lastActivityTime = Time.unscaledTime;
            }
        }

        private async void TimingRoutine()
        {
            while (true)
            {
                if (_isInitialized && _isActive)
                {
                    bool isUpdateWasDetected = false;
#if UNITY_EDITOR
                    isUpdateWasDetected = UnityEngine.Input.anyKey;
#else
                    isUpdateWasDetected = UnityEngine.Input.touchCount > 0;
#endif
                    if (isUpdateWasDetected)
                    {
                        _lastActivityTime = Time.unscaledTime;
                    }
                    else
                    {
                        float currentTime = Time.unscaledTime;
                        if (currentTime - _lastActivityTime > _longUserInactivityTimeSpan)
                        {
                            _lastActivityTime = currentTime;
                            OnTimeout?.Invoke();
                        }
                    }
                }
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }

    }
}