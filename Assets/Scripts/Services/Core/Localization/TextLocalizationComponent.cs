using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace IdxZero.Services.Localization
{
    public class TextLocalizationComponent : MonoBehaviour
    {
        [SerializeField] private KeyToTexts[] _staticLocalizationDictionary;
        [SerializeField] private TMP_Text[] _fontChangeDictionary;

        private ILocalizationFacade _localization;

        [Inject]
        private void Construct(ILocalizationFacade localizationFacade)
        {
            _localization = localizationFacade;

            foreach (var pair in _staticLocalizationDictionary)
            {
                foreach (var text in pair.Texts)
                {
                    _localization.SetTextWithKey(pair.Key, text);
                }
            }

            foreach (var text in _fontChangeDictionary)
            {
                _localization.SetFontPreset(text);
            }
        }
    }

    [Serializable]
    public class KeyToTexts
    {
        public string Key;
        public TMP_Text[] Texts;
    }
}