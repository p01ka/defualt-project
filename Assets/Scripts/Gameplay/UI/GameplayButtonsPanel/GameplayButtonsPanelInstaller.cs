using IdxZero.Base.MVP;
using Zenject;

namespace IdxZero.Gameplay.UI.GameplayButtonsPanel
{
    public class GameplayButtonsPanelInstaller : Installer<GameplayButtonsPanelInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<IGameplayButtonsPanelView, BasePresenter, GameplayButtonsPanelPresenter.Factory>()
                     .To<GameplayButtonsPanelPresenter>()
                     .WhenInjectedInto<GameplayButtonsPanelPresenterFactory>();

            Container.Bind<GameplayButtonsPanelPresenterFactory>()
                .ToSelf()
                .AsSingle()
                .WhenInjectedInto(typeof(IGameplayButtonsPanelView));
        }
    }
}





