using UnityEngine;

namespace IdxZero.Base.MVP
{
    public abstract class BaseView : MonoBehaviour
    {
        protected BasePresenter Presenter;

        private void OnEnable()
        {
            SubscribeOnEvents();
            Presenter.Initialize();
        }

        private void OnDisable()
        {
            Presenter.Uninitialize();
            UnsubscribeOnEvents();
        }

        private void OnDestroy()
        {
            Presenter?.Dispose();
        }

        protected abstract void SubscribeOnEvents();

        protected abstract void UnsubscribeOnEvents();
    }
}