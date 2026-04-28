using Zenject;

namespace IdxZero.Services.Localization
{
    public class LocalizationInstaller : Installer<LocalizationInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind(typeof(ILocalizationFacade)).To<LocalizationFacade>().AsSingle().NonLazy();
        }
    }
}