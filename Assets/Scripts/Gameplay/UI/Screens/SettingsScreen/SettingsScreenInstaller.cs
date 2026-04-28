using IdxZero.Base.MVP;
using Zenject;

namespace Gameplay.UI.Screens
{
    public class SettingsScreenInstaller : Installer<SettingsScreenInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<ISettingsScreenView, BasePresenter, SettingsScreenPresenter.Factory>()
                .To<SettingsScreenPresenter>()
                .WhenInjectedInto<SettingsScreenPresenterFactory>();

            Container.Bind<SettingsScreenPresenterFactory>()
                .ToSelf()
                .AsSingle()
                .WhenInjectedInto(typeof(ISettingsScreenView));

            InstallSignals();
        }

        private void InstallSignals()
        {
            Container.DeclareSignal<SettingsPanelSignals.OnOpenSettingsPanel>();
        }
    }

    public class SettingsPanelSignals
    {
        public class OnOpenSettingsPanel
        {
        }
    }
}