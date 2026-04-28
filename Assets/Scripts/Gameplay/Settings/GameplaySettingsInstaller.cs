using UnityEngine;
using Zenject;

// ReSharper disable InconsistentNaming

namespace IdxZero.Gameplay.Settings
{
    [CreateAssetMenu(fileName = "GameplaySettingsInstaller", menuName = "Installers/GameplaySettingsInstaller")]
    public class GameplaySettingsInstaller : ScriptableObjectInstaller<GameplaySettingsInstaller>
    {
#pragma warning disable 0649
        [SerializeField]
        private States.GameplayStateMachineConditionManager.Settings _gameplayStateMachineConditionManager;
#pragma warning restore 0649

        public override void InstallBindings()
        {
            Container.BindInstance(_gameplayStateMachineConditionManager).WhenInjectedInto<States.GameplayStateMachineConditionManager>();
        }
    }
}