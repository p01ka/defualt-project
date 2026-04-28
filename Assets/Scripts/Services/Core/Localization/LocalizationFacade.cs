using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using IdxZero.Utils;
using TMPro;
using UnityEngine;

namespace IdxZero.Services.Localization
{
    public class LocalizationFacade : ILocalizationFacade
    {
        private readonly IReadOnlyDictionary<CultureInfo, LocalizationSettings.ParsedLocalizationInfo> _localizations;
        private readonly HashSet<LocalizationSettings.FontTypeByPresetsDict> _allPresets;
        private readonly LocalizationSettings _localizationSettings;

        private CultureInfo _currentLanguage;

        private readonly Dictionary<Material, FontPreset> _materialToFontPreset;
        private readonly Dictionary<Material, FontType> _materialToFontType;
        private TMP_FontAsset _currentFontAsset;

        public LocalizationFacade(LocalizationSettings localizationSettings)
        {
            _localizationSettings = localizationSettings;
            _materialToFontPreset = new Dictionary<Material, FontPreset>();
            _currentLanguage = CUtils.GetCurrentCultureInfo(LocalizationConsts.UNITY_SYSTEM_LANGUAGE_WITH_CULTURE_CODE);

            if (!LocalizationConsts.AVAILABLE_LANGUAGES.ToList().Contains(_currentLanguage.Name))
                _currentLanguage = CultureInfo.GetCultureInfo("en-US");

            localizationSettings.InitializeWithLanguage(_currentLanguage);

            _localizations = localizationSettings.LocalizationsParsed;
            _allPresets = localizationSettings.AllPresets;

            _materialToFontPreset = new Dictionary<Material, FontPreset>();
            _materialToFontType = new Dictionary<Material, FontType>();

            _currentFontAsset = _localizations[_currentLanguage].PresetsDict.FirstOrDefault().Value.FontAsset;
        }

        public string GetText(string key)
        {
            return GetText(_currentLanguage, key);
        }

        public void SetTextWithKey(string key, TMP_Text tmpText)
        {
            var newText = GetText(_currentLanguage, key);
            SetText(newText, tmpText);
        }

        public void SetText(string text, TMP_Text tmpText)
        {
            tmpText.text = text;
            SetFontPreset(tmpText);
        }

        public void SetFontPreset(TMP_Text tmpText)
        {
            FindFontInfo(tmpText, out var fontType, out var fontPreset);

            if (fontPreset == FontPreset.NONE || fontType == FontType.FONT_NONE)
            {
                Debug.LogWarning($"Can't find enum font preset for material {tmpText.fontSharedMaterial}");
                return;
            }

            var presetsDict = _localizations[_currentLanguage].PresetsDict[fontType];
            if (!presetsDict.FontPresetToMaterialDict.ContainsKey(fontPreset))
            {
                Debug.LogWarning($"No Preset for {fontPreset} in {presetsDict.FontAsset}");
                fontPreset = FontPreset.DEFAULT;
            }
            tmpText.fontSharedMaterial = presetsDict.FontPresetToMaterialDict[fontPreset];
            tmpText.font = presetsDict.FontAsset;
        }

        private string GetText(CultureInfo language, string key)
        {
            if (!_localizations[language].LocalizationDictionary.ContainsKey(key))
            {
                Debug.Log("KEY NOT FOUND " + key);
            }

            return _localizations[language].LocalizationDictionary[key];
        }

        private void FindFontInfo(TMP_Text tmpText, out FontType fontType, out FontPreset fontPreset)
        {
            fontType = FontType.FONT_NONE;
            fontPreset = FontPreset.NONE;
            var fontMaterial = tmpText.fontSharedMaterial;
            if (!_materialToFontType.ContainsKey(fontMaterial))
            {
                foreach (var presetsDict in _allPresets)
                {
                    foreach (var (type, fontPresetToMaterial) in presetsDict)
                    {
                        foreach (var (preset, material) in fontPresetToMaterial.FontPresetToMaterialDict)
                        {
                            if (material != fontMaterial) continue;
                            _materialToFontType.Add(material, type);
                            _materialToFontPreset.Add(material, preset);
                            fontType = type;
                            fontPreset = preset;
                            return;
                        }
                    }
                }
            }
            else
            {
                fontType = _materialToFontType[fontMaterial];
                fontPreset = _materialToFontPreset[fontMaterial];
            }
        }

        public bool IsCurrentFontSupportCharacter(char[] characters)
        {
            int charCount = characters.Length;
            for (int i = 0; i < charCount; i++)
            {
                char character = characters[i];
                bool hasCharacter = _currentFontAsset.HasCharacter(character);
                if (!hasCharacter)
                    return false;
            }
            return true;
        }
    }
}