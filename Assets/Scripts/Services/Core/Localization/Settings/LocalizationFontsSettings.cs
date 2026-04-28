using System;
using RotaryHeart.Lib.SerializableDictionary;

namespace IdxZero.Services.Localization
{
    [Serializable]
    public class LocalizationFontsSettings
    {
        public CultureInfoKeyToPresets Presets;
    }

    [Serializable]
    public class CultureInfoKeyToPresets : SerializableDictionaryBase<string, FontPresetToMaterialSO>
    {
    }
}