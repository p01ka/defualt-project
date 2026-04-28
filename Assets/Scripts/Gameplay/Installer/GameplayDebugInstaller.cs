using UnityEngine;
using Zenject;

namespace IdxZero.Gameplay.Installers
{
    public class GameplayDebugInstaller : Installer<GameplayDebugInstaller>
    {
        public override void InstallBindings()
        {
            InstallEventSystem();
        }

        private static void InstallEventSystem()
        {
            var debugGo = new GameObject("GAME PLAY DEBUG");
            debugGo.AddComponent<UnityEngine.EventSystems.EventSystem>();
            debugGo.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
    }
}