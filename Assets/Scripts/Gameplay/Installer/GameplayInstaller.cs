using Gameplay.UI.Screens;
using IdxZero.Base.Attributes;
using IdxZero.Base.Installers;
using IdxZero.Gameplay.States;
using IdxZero.Gameplay.UI.GameplayButtonsPanel;
using UnityEngine;
using Zenject;

namespace IdxZero.Gameplay.Installers
{
    public class GameplayInstaller : MonoInstaller, IDebuggableInstaller
    {
        [ShowOnly]
        [SerializeField]
        private bool _isDebugOn;

        public bool IsDebugOn { get => _isDebugOn; set => _isDebugOn = value; }

        public override void InstallBindings()
        {
            if (_isDebugOn)
            {
                GameplayDebugInstaller.Install(Container);
            }
            InstallUI();
            InstallSignals();
            InstallStates();
        }

        private void InstallUI()
        {
            GameplayButtonsPanelInstaller.Install(Container);
            SettingsScreenInstaller.Install(Container);
        }

        private void InstallSignals()
        {
        }

        private void InstallStates()
        {
            GameplayStateMachineStatesInstaller.Install(Container);
        }
    }
}