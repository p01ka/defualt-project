// UNCOMMENT TO USE MUTATOR
// using System;
// using MagnusSdk.Mutator;
// using UnityEngine;

// namespace IdxZero.Services.RemoteConfig
// {
//     public class MutatorRemoteConfigStrategy : IRemoteConfigStrategy
//     {
//         MutatorConfig _currentMutatorConfig;

//         public async void InitializeStrategy(Action onInitializedCallback)
//         {
//             try
//             {
//                 Mutator.Initialize();
//                 await Mutator.FetchConfig(300);
//                 _currentMutatorConfig = await Mutator.Activate();
//                 onInitializedCallback?.Invoke();
//             }
//             catch (Exception e)
//             {
//                 Debug.LogWarning(e);
//                 onInitializedCallback?.Invoke();
//             }
//         }

//         public bool GetBoolWithKey(string key, bool defaultValue)
//         {
//             return GetBoolWithDefaultValue(key, defaultValue);
//         }

//         public float GetFloatWithKey(string key, float defaultValue)
//         {
//             return GetFloatValueWithDefaultValue(key, defaultValue);
//         }

//         public int GetIntWithKey(string key, int defaultValue)
//         {
//             return GetIntegerWithDefaultValue(key, defaultValue);
//         }

//         public string GetStringWithKey(string key, string defaultValue)
//         {
//             return GetStringWithDefaultValue(key, defaultValue);
//         }

//         #region PRIVATE

//         private float GetFloatValueWithDefaultValue(string key, float defaultValue)
//         {
//             float floatValue;
//             if (_currentMutatorConfig != null && _currentMutatorConfig.HasValue(key))
//             {
//                 string stringValue = _currentMutatorConfig.GetValue(key).StringValue;
//                 floatValue = ParseStringToFloat(stringValue, defaultValue);
//             }
//             else
//             {
//                 floatValue = defaultValue;
//             }

//             return floatValue;
//         }

//         private bool GetBoolWithDefaultValue(string key, bool defaultValue)
//         {
//             bool booleanValue;
//             if (_currentMutatorConfig != null && _currentMutatorConfig.HasValue(key))
//             {
//                 string stringValue = _currentMutatorConfig.GetValue(key).StringValue;
//                 booleanValue = ParseStringToBool(stringValue, defaultValue);
//             }
//             else
//             {
//                 booleanValue = defaultValue;
//             }

//             return booleanValue;
//         }

//         private string GetStringWithDefaultValue(string key, string defaultValue)
//         {
//             string stringValue;
//             if (_currentMutatorConfig != null && _currentMutatorConfig.HasValue(key))
//             {
//                 stringValue = _currentMutatorConfig.GetValue(key).StringValue;
//             }
//             else
//             {
//                 stringValue = defaultValue;
//             }

//             return stringValue;
//         }

//         private int GetIntegerWithDefaultValue(string key, int defaultValue)
//         {
//             int integerValue;
//             if (_currentMutatorConfig != null && _currentMutatorConfig.HasValue(key))
//             {
//                 string stringValue = _currentMutatorConfig.GetValue(key).StringValue;
//                 integerValue = ParseStringToInt(stringValue, defaultValue);
//             }
//             else
//             {
//                 integerValue = defaultValue;
//             }

//             return integerValue;
//         }

//         private T GetEnumTypeWithDefaultValue<T>(string key, T defaultValue) where T : struct, Enum
//         {
//             T enumValue = default;

//             if (_currentMutatorConfig != null && _currentMutatorConfig.HasValue(key))
//             {
//                 string enumString = _currentMutatorConfig.GetValue(key).StringValue;

//                 if (Enum.TryParse(enumString, true, out T resultEnum))
//                 {
//                     enumValue = resultEnum;
//                 }
//                 else
//                 {
//                     enumValue = defaultValue;
//                 }
//             }
//             else
//             {
//                 enumValue = defaultValue;
//             }

//             return enumValue;
//         }

//         private bool ParseStringToBool(string boolString, bool defaultValue)
//         {
//             bool parsedBool = defaultValue;
//             boolString = boolString.ToLower();
//             if (bool.TryParse(boolString, out var parsedValue))
//             {
//                 parsedBool = parsedValue;
//             }

//             return parsedBool;
//         }

//         private int ParseStringToInt(string intString, int defaultValue)
//         {
//             int index = defaultValue;
//             if (int.TryParse(intString, out var parsedIndex))
//             {
//                 index = parsedIndex;
//             }

//             return index;
//         }

//         private float ParseStringToFloat(string floatString, float defaultValue)
//         {
//             float floatValue = defaultValue;
//             if (float.TryParse(floatString, out var parsedFloat))
//             {
//                 floatValue = parsedFloat;
//             }

//             return floatValue;
//         }
//         #endregion PRIVATE
//     }
// }