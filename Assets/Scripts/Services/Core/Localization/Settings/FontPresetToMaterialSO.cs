using System;
using RotaryHeart.Lib.SerializableDictionary;
using TMPro;
using UnityEngine;

namespace IdxZero.Services.Localization
{
    [CreateAssetMenu(menuName = "Localization/PresetsToMaterialsSO", fileName = "PresetsToMaterialsSO")]
    public class FontPresetToMaterialSO : ScriptableObject
    {
        public TMP_FontAsset FontAsset;
        public FontPresetToMaterialDict FontPresetToMaterialDict;
    }

    [Serializable]
    public class FontPresetToMaterialDict : SerializableDictionaryBase<FontPreset, Material>
    {
    }
}