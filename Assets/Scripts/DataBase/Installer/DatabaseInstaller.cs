using Zenject;

namespace IdxZero.DataBase.Installer
{
    public class DatabaseInstaller : Installer<DatabaseInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind(typeof(IUserDatabaseManager)).To<PlayerPrefsUserDatabaseManager>().AsSingle();
        }
    }
}