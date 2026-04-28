using Cysharp.Threading.Tasks;
using Zenject;

namespace IdxZero.Services.RateUs
{
    public class RateUsFacade : IRateUsFacade
    {
        private readonly IRateUsStrategy _rateUsStrategy;
        private readonly SignalBus _signals;

        private bool _isPending;

        public RateUsFacade(IRateUsStrategy rateUsStrategy,
                            SignalBus signals)
        {
            _rateUsStrategy = rateUsStrategy;
            _signals = signals;
        }

        public bool IsPending()
        {
            return _isPending;
        }

        public async UniTask RateUs()
        {
            // _signals.TryFire(new ApplicationScreenSignals.OnShowPreloadingScreen());
            _isPending = true;
            await _rateUsStrategy.RateUsByStrategy();
            _isPending = false;
            // _signals.TryFire(new ApplicationScreenSignals.OnHidePreloadingScreen());
        }
    }
}