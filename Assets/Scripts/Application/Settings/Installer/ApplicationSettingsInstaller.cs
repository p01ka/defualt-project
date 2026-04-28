using IdxZero.Application.Installers;
using UnityEngine;
using Zenject;

namespace IdxZero.Application.Settings.Installer
{
    [CreateAssetMenu(fileName = "ApplicationSettingsInstaller", menuName = "Installers/ApplicationSettingsInstaller")]
    public class ApplicationSettingsInstaller : ScriptableObjectInstaller<ApplicationSettingsInstaller>
    {
#pragma warning disable 0649
        [SerializeField]
        private MainConfig _mainConfig;

        [SerializeField]
        private DebugInstaller.Settings _debugSettings;

#pragma warning restore 0649

        public override void InstallBindings()
        {
            Container.BindInstance(_mainConfig);
            Container.BindInstance(_debugSettings);
        }
    }
}