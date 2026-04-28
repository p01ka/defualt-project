using Zenject;

namespace IdxZero.Services.InAppPurchasing
{
    public class InAppInstaller : Installer<InAppInstaller>
    {
        public override void InstallBindings()
        {
#if USE_INAPP_PURCHASING
            Container.Bind(typeof(IInAppManager)).To<InAppManager>().FromNewComponentOnNewGameObject().AsSingle();
            Container.Bind<ISubscriptionValidator>().To<SubscriptionValidator>().AsSingle();
            Container.Bind<InAppProceeder>().ToSelf().AsSingle();
            Container.Bind<InAppInitializer>().ToSelf().AsSingle();
            Container.Bind<SubscriptionExpireTimeKeeper>().ToSelf().AsSingle();
#endif
        }
    }
}
