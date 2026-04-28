using Zenject;
using IdxZero.Services.Firebase;
using IdxZero.Services.ServicesStarter;
using IdxZero.Services.Analytics;
using IdxZero.Services.NativeDialog;
using IdxZero.Services.Notification;
using IdxZero.Services.UserProperties;
using IdxZero.Services.Ads;
using IdxZero.Services.RateUs;
using IdxZero.Services.Signals;
using IdxZero.Services.MonoStandart;
using IdxZero.Services.RemoteConfig;
using IdxZero.Services.ActivityChecker;
using IdxZero.Services.ScreenSession;
using IdxZero.Services.Localization;
using IdxZero.Services.Permissions;
using IdxZero.Services.Attribution;

namespace IdxZero.Services.Installers
{
    public class ServicesInstaller : Installer<ServicesInstaller>
    {
        public override void InstallBindings()
        {
            InstallFirebase();
            InstallServicesStarter();
            InstallAnalytics();
            InstallLocalization();
            InstallPermissionChecker();
            InstallNativeDialog();
            InstallNotifications();
            InstallUserPropertiesFacade();
            InstallAttributionService();
            InstallAds();
            InstallScreenSessionSetter();
            InstallActivityChecker();
            InstallSignals();
            InstallRateUs();
            InstallMonoStandartMethods();
            InstallRemoteConfig();
        }

        private void InstallFirebase()
        {
            Container.Bind<FirebaseInitializer>().ToSelf().AsSingle();
        }

        private void InstallServicesStarter()
        {
            Container.Bind(typeof(IServicesStarter)).To<ServicesStarter.ServicesStarter>().AsSingle();
            Container.Bind<PurchasingServiceInitializer>().ToSelf().AsSingle();
        }

        private void InstallAnalytics()
        {
            Container.Bind(typeof(IAnalyticsFacade)).To<AnalyticsFacade>().AsSingle().NonLazy();

            Container.Bind(typeof(IAnalyticsStrategy))
                .To<MockAnalyticsStrategy>()
                .AsSingle()
                .WhenInjectedInto<AnalyticsFacade>();
        }

        private void InstallLocalization()
        {
            LocalizationInstaller.Install(Container);
        }

        private void InstallPermissionChecker()
        {
            Container.Bind(typeof(IPermissionChecker)).To<UserPermissionChecker>().AsSingle();
        }

        private void InstallNativeDialog()
        {
            Container.Bind(typeof(IMobileNativeMessage)).To<MobileNativeMessage>().AsSingle();
        }

        private void InstallNotifications()
        {
            Container.Bind(typeof(INotificationFacade)).To<NotificationFacade>().AsSingle();
#if UNITY_EDITOR
            Container.Bind(typeof(INotificationKeeper)).To<EditorNotificationKeeper>().AsSingle();
#elif UNITY_ANDROID
            Container.Bind(typeof(INotificationKeeper),typeof(IApplicationFocusHandler)).To<AndroidNotificationKeeper>().AsSingle();
#elif UNITY_IOS
            Container.Bind(typeof(INotificationKeeper), typeof(IApplicationFocusHandler)).To<IOSNotificationKeeper>().AsSingle();
#endif
        }

        private void InstallUserPropertiesFacade()
        {
            Container.Bind(typeof(IUserPropertiesFacade)).To<UserPropertiesFacade>().AsSingle();
            Container.Bind(typeof(IUserPropertiesStrategy))
                .To<MockUserPopertiesStrategy>()
                .AsSingle()
                .WhenInjectedInto(typeof(IUserPropertiesFacade));
        }

        private void InstallAttributionService()
        {
            Container.Bind<IAttributionService>().To<MockAttributionService>().AsSingle();
        }

        private void InstallAds()
        {
            Container.BindInterfacesTo<AdsFacade>().AsSingle().NonLazy();

            Container.Bind(typeof(IAdsStrategy))
                .To<MockAdsStrategy>()
                .AsSingle()
                .WhenInjectedInto(typeof(IAdsFacade));

            Container.BindInterfacesAndSelfTo<InterstitialShowingResolver>()
                    .AsSingle();

            Container.BindInterfacesTo<RewardStatusKeeper>().AsSingle();
            Container.Bind<AdsUtilsStarter>().ToSelf().AsSingle();
        }

        private void InstallActivityChecker()
        {
            Container.Bind(typeof(IUserActivityChecker)).To<UserActivityChecker>().AsSingle();
        }

        private void InstallScreenSessionSetter()
        {
            Container.Bind(typeof(IScreenSessionSetter)).To<ScreenSessionSetter>().AsSingle();
        }

        private void InstallRateUs()
        {
            Container.Bind<IRateUsFacade>().To<RateUsFacade>().AsSingle();
#if UNITY_EDITOR
            Container.Bind<IRateUsStrategy>().To<EditorRateUsStrategy>().AsSingle()
                .WhenInjectedInto(typeof(IRateUsFacade));
#elif UNITY_ANDROID
            Container.Bind<IRateUsStrategy>().To<AndroidRateUsStrategy>().AsSingle()
                .WhenInjectedInto(typeof(IRateUsFacade));
#elif UNITY_IOS
            Container.Bind<IRateUsStrategy>().To<IOSRateUsStrategy>().AsSingle()
                .WhenInjectedInto(typeof(IRateUsFacade));
#endif
        }

        private void InstallSignals()
        {
            Container.DeclareSignal<ServicesSignals.OnAdsShowStarted>().OptionalSubscriber();
            Container.DeclareSignal<ServicesSignals.OnAdsShowClosed>().OptionalSubscriber();
            Container.DeclareSignal<ServicesSignals.OnShowAdsDebugger>().OptionalSubscriber();
            Container.DeclareSignal<ServicesSignals.OnApplicationGainedFocus>().OptionalSubscriber();
            Container.DeclareSignal<ServicesSignals.OnApplicationLostFocus>().OptionalSubscriber();
        }

        private void InstallMonoStandartMethods()
        {
            MonoStandartMethodsInstaller.Install(Container);
        }

        private void InstallRemoteConfig()
        {
            RemoteConfigInstaller.Install(Container);
        }
    }
}