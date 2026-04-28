using Zenject;

namespace IdxZero.Services.MonoStandart
{
    public class MonoStandartMethodsInstaller : Installer<MonoStandartMethodsInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ApplicationFocusHandlerManager>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("FocusHandler")
                .AsSingle()
                .NonLazy();

        }
    }
}