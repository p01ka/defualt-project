using IdxZero.Base.MVP;

namespace IdxZero.Gameplay.UI.GameplayButtonsPanel
{
    public class GameplayButtonsPanelPresenterFactory
    {
        private readonly GameplayButtonsPanelPresenter.Factory _factory;

        public GameplayButtonsPanelPresenterFactory(GameplayButtonsPanelPresenter.Factory factory)
        {
            _factory = factory;
        }

        public BasePresenter Create(IGameplayButtonsPanelView view)
        {
            return _factory.Create(view);
        }
    }
}





