using System;
using IdxZero.Utils;
using Tayx.Graphy;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace IdxZero.Application.Installers
{
    public class DebugInstaller : Installer<DebugInstaller>
    {
        private readonly Settings _settings;

        public DebugInstaller(Settings settings)
        {
            _settings = settings;
        }

        public override void InstallBindings()
        {
            InstallDebugConsole();
            InstallGraphy();
        }

        private void InstallDebugConsole()
        {
            Container.Bind<DebugConsoleOutput>()
                     .ToSelf()
                     .FromNewComponentOnNewGameObject()
                     .AsSingle()
                     .NonLazy();
        }

        private void InstallGraphy()
        {
            Container.Bind<GraphyManager>()
                     .FromComponentInNewPrefab(_settings.GraphyPrefab)
                     .AsSingle()
                     .NonLazy();
        }

        [Serializable]
        public class Settings
        {
            [FormerlySerializedAsAttribute("GraphyPrefab")]
            public GameObject GraphyPrefab;
        }
    }
}