using System.Collections.Generic;
using IdxZero.Services.Signals;
using UnityEngine;
using Zenject;

namespace IdxZero.Services.MonoStandart
{
    public class ApplicationFocusHandlerManager : MonoBehaviour
    {
        private List<IApplicationFocusHandler> _handlers;
        private SignalBus _signals;

        [Inject]
        public void Construct(List<IApplicationFocusHandler> handlers, SignalBus signals)
        {
            _handlers = handlers;
            _signals = signals;
        }

        public void OnApplicationFocus(bool focused)
        {
            if (_handlers == null) return;
            if (focused)
            {
                _signals.TryFire<ServicesSignals.OnApplicationGainedFocus>();
            }
            else
            {
                _signals.TryFire<ServicesSignals.OnApplicationLostFocus>();
            }
            foreach (var handler in _handlers)
            {
                if (focused)
                {
                    handler.OnGainedFocus();
                }
                else
                {
                    handler.OnLostFocus();
                }
            }
        }
    }
}
