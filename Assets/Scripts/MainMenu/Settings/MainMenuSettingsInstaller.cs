using UnityEngine;
using Zenject;

namespace IdxZero.MainMenu.Settings.Installer
{
    [CreateAssetMenu(fileName = "MainMenuSettingsInstaller", menuName = "Installers/MainMenuSettingsInstaller")]
    public class MainMenuSettingsInstaller : ScriptableObjectInstaller<MainMenuSettingsInstaller>
    {
#pragma warning disable 0649

#pragma warning restore 0649

        public override void InstallBindings()
        {
        }
    }
}