using System;
using System.Collections.Generic;
using System.Globalization;
using MyBox;
using Newtonsoft.Json;
using RotaryHeart.Lib.SerializableDictionary;
using TMPro;
using UnityEngine;

namespace IdxZero.Services.Localization
{
    [Serializable]
    public class LocalizationSettings
    {
        [SerializeField] private TextAsset _localizationsSchema;
        [SerializeField] private CultureInfoCodeToLocalizationInfo _currentCultureInfoCodeToLocalizationInfo;
        [SerializeField] private string _defaultCultureInfoLocalizationPath;
        [SerializeField][TextArea] private string _localizationDocumentLink;
        [SerializeField][TextArea] private string _localizationInfoLink;

        private Dictionary<CultureInfo, ParsedLocalizationInfo> _localizationsParsed;
        private HashSet<FontTypeByPresetsDict> _allPresets;

        public TextAsset LocalizationsSchema => _localizationsSchema;

        public string LocalizationDocumentLink => _localizationDocumentLink;

        public IReadOnlyDictionary<CultureInfo, ParsedLocalizationInfo> LocalizationsParsed
        {
            get => _localizationsParsed;
        }

        public HashSet<FontTypeByPresetsDict> AllPresets
        {
            get
            {
                if (_allPresets == null)
                {
                    _allPresets = new HashSet<FontTypeByPresetsDict>();
                    foreach (var pair in _currentCultureInfoCodeToLocalizationInfo)
                    {
                        _allPresets.Add(pair.Value.PresetsDict);
                    }
                }

                return _allPresets;
            }
        }

        public void ValidateLocalization()
        {
            var localizationsParsed = new Dictionary<CultureInfo, ParsedLocalizationInfo>();

            foreach (var localizationKey in LocalizationConsts.AVAILABLE_LANGUAGES)
            {
                _currentCultureInfoCodeToLocalizationInfo.TryGetValue(localizationKey,
                    out var info);
                if (info?.Path == null || info.Path.IsNullOrEmpty())
                {
                    Debug.LogWarning($"Localization json with {localizationKey} language not found");
                }
                else
                {
                    var localizationJson = Resources.Load<TextAsset>(info.Path);
                    if (localizationJson == null)
                    {
                        Debug.LogWarning($"Localization json with {localizationKey} language not found");
                    }
                    else
                    {
                        var localization = JsonConvert.DeserializeObject<LocalizationJsonSchema>(localizationJson.text);
                        if (!localizationsParsed.ContainsKey(CultureInfo.GetCultureInfo(localization.Lang)))
                        {
                            localizationsParsed[CultureInfo.GetCultureInfo(localization.Lang)] =
                                new ParsedLocalizationInfo();
                        }

                        localizationsParsed[CultureInfo.GetCultureInfo(localization.Lang)].LocalizationDictionary =
                            localization.Data;
                    }
                }
            }

            var schemaDict = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(LocalizationsSchema.text);
            var errors = string.Empty;
            LocalizationSchemaValidator.Validate(localizationsParsed, schemaDict["data"], ref errors);
            if (!string.IsNullOrEmpty(errors))
                Debug.LogWarning(errors);
            else
                Debug.Log("Localizations validated");
        }

        public void InitializeWithLanguage(CultureInfo currentLanguage)
        {
            ClearOrCreateLocalization();

            _currentCultureInfoCodeToLocalizationInfo.TryGetValue(currentLanguage.Name, out var info);
            TextAsset localizationJson;
            if (info?.Path == null || info.Path.IsNullOrEmpty())
                localizationJson = Resources.Load<TextAsset>(_defaultCultureInfoLocalizationPath);
            else
            {
                localizationJson = Resources.Load<TextAsset>(info.Path)
                                   ?? Resources.Load<TextAsset>(_defaultCultureInfoLocalizationPath);
            }

            var localization =
                JsonConvert.DeserializeObject<LocalizationJsonSchema>(localizationJson.text);

            if (!_localizationsParsed.ContainsKey(CultureInfo.GetCultureInfo(localization.Lang)))
            {
                _localizationsParsed[CultureInfo.GetCultureInfo(localization.Lang)] = new ParsedLocalizationInfo();
            }

            _localizationsParsed[CultureInfo.GetCultureInfo(localization.Lang)].LocalizationDictionary =
                localization.Data;
            _localizationsParsed[CultureInfo.GetCultureInfo(localization.Lang)].PresetsDict =
                _currentCultureInfoCodeToLocalizationInfo[CultureInfo.GetCultureInfo(localization.Lang).Name].PresetsDict;

            var schemaDict = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(LocalizationsSchema.text);
            string errors = String.Empty;
            LocalizationSchemaValidator.Validate(_localizationsParsed, schemaDict["data"], ref errors);
            if (!String.IsNullOrEmpty(errors))
                Debug.LogError(errors);
        }

        private void ClearOrCreateLocalization()
        {
            if (_localizationsParsed == null)
                _localizationsParsed = new Dictionary<CultureInfo, ParsedLocalizationInfo>();
            else
                _localizationsParsed.Clear();
        }

        [Serializable]
        public class LocalizationJsonSchema
        {
            [JsonProperty("lang")] public string Lang;
            [JsonProperty("data")] public Dictionary<string, string> Data;
        }

        public class ParsedLocalizationInfo
        {
            public Dictionary<string, string> LocalizationDictionary;
            public FontTypeByPresetsDict PresetsDict;
        }

        [Serializable]
        public class CultureInfoCodeToLocalizationInfo : SerializableDictionaryBase<string, LocalizationInfo>
        {
        }

        [Serializable]
        public class LocalizationInfo
        {
            public string Path;
            public FontTypeByPresetsDict PresetsDict;
        }

        [Serializable]
        public class FontTypeByPresetsDict : SerializableDictionaryBase<FontType, FontPresetToMaterialSO>
        {
        }
    }
}