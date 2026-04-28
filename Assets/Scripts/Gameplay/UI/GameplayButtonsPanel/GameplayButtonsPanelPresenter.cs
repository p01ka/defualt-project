using Gameplay.UI.Screens;
using IdxZero.Application.Signals;
using IdxZero.Base.MVP;
using Zenject;

namespace IdxZero.Gameplay.UI.GameplayButtonsPanel
{
    public class GameplayButtonsPanelPresenter : BasePresenter
    {
        private readonly IGameplayButtonsPanelView _view;
        private readonly SignalBus _signals;

        public GameplayButtonsPanelPresenter(IGameplayButtonsPanelView view, SignalBus signals)
        {
            _view = view;
            _signals = signals;
        }

        private void TryToExitFromGameplay()
        {
            _signals.TryFire<ApplicationSignals.OnBackToPreviousState>();
        }
        
        private void OpenSettingsPanel()
        {
            _signals.TryFire<SettingsPanelSignals.OnOpenSettingsPanel>();
        }

        protected override void SubscribeOnViewEvents()
        {
            _view.OnBackButtonPressed += TryToExitFromGameplay;
            _view.OnSettingsButtonPressed += OpenSettingsPanel;
        }

        protected override void UnsubscribeOnViewEvents()
        {
            _view.OnBackButtonPressed -= TryToExitFromGameplay;
            _view.OnSettingsButtonPressed -= OpenSettingsPanel;
        }

        #region Factory

        public class Factory : PlaceholderFactory<IGameplayButtonsPanelView, BasePresenter>
        {
        }

        #endregion Factory
    }
}





