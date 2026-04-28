using UnityEngine;
using Zenject;

namespace IdxZero.Services.Localization
{
    [CreateAssetMenu(fileName = "LocalizationSettingsInstaller", menuName = "Installers/LocalizationSettingsInstaller")]
    public class LocalizationSettingsInstaller : ScriptableObjectInstaller<LocalizationSettingsInstaller>
    {
#pragma warning disable 0649
        [SerializeField] private LocalizationSettings _localizationSettings;
#if UNITY_EDITOR
        [SerializeField] private LocalizationConverter _localizationConverter;
#endif 

#pragma warning restore 0649

        public override void InstallBindings()
        {
            Container.BindInstance(_localizationSettings).WhenInjectedInto(typeof(ILocalizationFacade));
        }

#if UNITY_EDITOR
        public void ValidateLocalizations()
        {
            _localizationSettings.ValidateLocalization();
        }

        public void ConvertCsvToJson()
        {
            _localizationConverter.ConvertCsvToJson(_localizationSettings.LocalizationsSchema);
        }

        public void OpenLocalizationDocument()
        {
            if (!string.IsNullOrEmpty(_localizationSettings.LocalizationDocumentLink))
                UnityEngine.Application.OpenURL(_localizationSettings.LocalizationDocumentLink);
            else
                Debug.LogWarning("Localization document link is empty");
        }
#endif
    }
}