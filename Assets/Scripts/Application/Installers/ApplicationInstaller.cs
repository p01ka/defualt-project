using System;
using IdxZero.Services.Installers;
using IdxZero.DataBase.Installer;
using IdxZero.Application.States.Base;
using Zenject;
using IdxZero.Base.States;
using IdxZero.Application.States;
using IdxZero.Application.Signals;
using IdxZero.Application.Model;
using IdxZero.Application.Model.UserStatus;
using IdxZero.Utils;
using IdxZero.Application.UI.ApplicationScreen;

namespace IdxZero.Application.Installers
{
    public class ApplicationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallApplication();
            InstallStates();
            InstallSignals();
            InstallModel();
            InstallUtils();
            InstallApplicationScreen();

            ServicesInstaller.Install(Container);
            DatabaseInstaller.Install(Container);
        }

        private void InstallApplication()
        {
            Container.Bind(typeof(IInitializable), typeof(IDisposable))
               .To<ApplicationStatesController>()
               .AsSingle();

            Container.Bind<ApplicationConditionManager>()
                .ToSelf()
                .AsSingle()
                .WhenInjectedInto<ApplicationStatesController>();
        }

        private void InstallStates()
        {
            Container.BindFactory<IBaseState, LoadingState.Factory>()
                .To<LoadingState>()
                .WhenInjectedInto<ApplicationStatesFactory>();

            Container.BindFactory<IBaseState, MainMenuState.Factory>()
                .To<MainMenuState>()
                .WhenInjectedInto<ApplicationStatesFactory>();

            Container.BindFactory<IBaseState, GamePlayState.Factory>()
                .To<GamePlayState>()
                .WhenInjectedInto<ApplicationStatesFactory>();

            Container.Bind<ApplicationStatesFactory>().ToSelf().AsSingle();
        }

        private void InstallSignals()
        {
            SignalBusInstaller.Install(Container);

            InstallApplicationSignals();
            InstallMainMenuStateSignals();
        }

        private void InstallApplicationSignals()
        {
            Container.DeclareSignal<ApplicationSignals.OnApplicationLoaded>().OptionalSubscriber();
            Container.DeclareSignal<ApplicationSignals.OnStartGameplay>().OptionalSubscriber();
            Container.DeclareSignal<ApplicationSignals.OnBackToPreviousState>().OptionalSubscriber();

        }

        private void InstallMainMenuStateSignals()
        {
            Container.DeclareSignal<MainMenuStateSignals.OnShowMainMenu>().OptionalSubscriber();
            Container.DeclareSignal<MainMenuStateSignals.OnHideMainMenu>().OptionalSubscriber();
            Container.DeclareSignal<MainMenuStateSignals.OnActiveMainMenuInteraction>().OptionalSubscriber();
        }

        private void InstallModel()
        {
            Container.Bind<ApplicationModel>().ToSelf().AsSingle();
            Container.Bind(typeof(IGameplayTransactionModelGetter), typeof(IGameplayTransactionModelSetter))
                .To<GameplayTransactionModel>()
                .AsSingle();
            Container.Bind(typeof(IUserStatusGetter), typeof(IUserStatusSetter)).To<UserStatusModel>().AsSingle();
        }

        private void InstallUtils()
        {
            Container.Bind<UnityMainThreadDispatcher>()
               .ToSelf()
               .FromNewComponentOnNewGameObject()
               .WithGameObjectName("UnityMainThreadDispatcher")
               .AsSingle()
               .NonLazy();
        }

        private void InstallApplicationScreen()
        {
            Container.Bind<ApplicationScreenAdapter>().AsSingle();
        }
    }
}