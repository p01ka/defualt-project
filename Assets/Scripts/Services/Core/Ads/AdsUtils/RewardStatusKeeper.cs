using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace IdxZero.Services.Ads
{
    public interface IRewardStatusKeeper
    {
        void Init();
        bool IsRewardEnabled { get; }
        event Action<bool> OnRewardEnabledStatusChanged;
    }

    public class RewardStatusKeeper : IRewardStatusKeeper, IDisposable
    {
        private readonly IAdsFacade _adsFacade;
        private CancellationTokenSource _cancellationTokenSource;

        public bool IsRewardEnabled { get; private set; }
        public event Action<bool> OnRewardEnabledStatusChanged;

        private const float CheckRewardedVideoDelay = 2f;

        public RewardStatusKeeper(IAdsFacade adsFacade)
        {
            _adsFacade = adsFacade;
        }

        public void Init()
        {
            IsRewardEnabled = _adsFacade.IsRewardedVideoEnable();
            _cancellationTokenSource = new CancellationTokenSource();
            UpdateReward(_cancellationTokenSource.Token).Forget();
        }

        private async UniTask UpdateReward(CancellationToken cancellationToken)
        {
            bool isFirstRequest = true;

            while (!cancellationToken.IsCancellationRequested)
            {
                if (isFirstRequest)
                {
                    isFirstRequest = false;
                }
                else
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(CheckRewardedVideoDelay), cancellationToken: cancellationToken);
                }
                _adsFacade.TryToLoadRewardedVideo();
                var isRewardEnableNewStatus = _adsFacade.IsRewardedVideoEnable();
                if (IsRewardEnabled == isRewardEnableNewStatus) continue;
                IsRewardEnabled = isRewardEnableNewStatus;
                await UniTask.SwitchToMainThread();
                OnRewardEnabledStatusChanged?.Invoke(IsRewardEnabled);
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}