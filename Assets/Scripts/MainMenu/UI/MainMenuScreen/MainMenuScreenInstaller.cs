using IdxZero.Base.MVP;
using Zenject;

namespace IdxZero.MainMenu.UI.MainMenuScreen
{
    public class MainMenuScreenInstaller : Installer<MainMenuScreenInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<IMainMenuScreenView, BasePresenter, MainMenuScreenPresenter.Factory>()
                     .To<MainMenuScreenPresenter>()
                     .WhenInjectedInto<MainMenuScreenPresenterFactory>();

            Container.Bind<MainMenuScreenPresenterFactory>()
                .ToSelf()
                .AsSingle()
                .WhenInjectedInto(typeof(IMainMenuScreenView));
        }
    }

}





