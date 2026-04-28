using IdxZero.Loading.Core;
using Zenject;

namespace IdxZero.Loading.Installer
{
    public class LoadingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallGameLoader();
        }

        private void InstallGameLoader()
        {
            Container.Bind(typeof(IInitializable)).To<GameLoader>().AsSingle();
        }
    }
}