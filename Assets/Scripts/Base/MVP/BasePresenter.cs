namespace IdxZero.Base.MVP
{
    public abstract class BasePresenter
    {
        public void Initialize()
        {
            SubscribeOnViewEvents();
        }

        public void Uninitialize()
        {
            UnsubscribeOnViewEvents();
        }

        public virtual void Dispose()
        {
        }

        protected abstract void SubscribeOnViewEvents();

        protected abstract void UnsubscribeOnViewEvents();
    }
}