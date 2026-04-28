using UnityEngine;
using Zenject;

namespace IdxZero.Services.RemoteConfig
{
    [CreateAssetMenu(fileName = "RemoteConfigSettingsInstaller", menuName = "Installers/RemoteConfigSettingsInstaller")]
    public class RemoteConfigSettingsInstaller : ScriptableObjectInstaller<RemoteConfigSettingsInstaller>
    {
#pragma warning disable 0649
        [SerializeField] private RemoteConfigDefaultValues _remoteConfigDefaultValues;

#pragma warning restore 0649

        public override void InstallBindings()
        {
            Container.BindInstance(_remoteConfigDefaultValues);
        }
    }
}