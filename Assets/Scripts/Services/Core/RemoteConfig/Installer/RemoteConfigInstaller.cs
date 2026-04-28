using Zenject;

namespace IdxZero.Services.RemoteConfig
{
    public class RemoteConfigInstaller : Installer<RemoteConfigInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind(typeof(IRemoteConfigDataKeeper), typeof(IRemoteConfigService))
                  .To<RemoteConfigFacade>()
                  .AsSingle();

            Container.Bind(typeof(IRemoteConfigStrategy))
                     .To<MockRemoteConfigStrategy>()
                     .AsSingle();
        }
    }
}