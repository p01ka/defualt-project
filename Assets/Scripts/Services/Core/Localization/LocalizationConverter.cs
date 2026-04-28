#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyBox;
using Newtonsoft.Json;
using UnityEngine;
using UnityEditor;
using yutokun;

namespace IdxZero.Services.Localization
{
    [Serializable]
    public class LocalizationConverter
    {
        [SerializeField] private TextAsset _csvFile;

        public void ConvertCsvToJson(TextAsset schema)
        {
            if (_csvFile == null)
            {
                Debug.LogError("CSV file is not serialized");
                return;
            }

            LocalizationCreator.CreateJsons(_csvFile.text, schema);
        }
    }

    public static class LocalizationCreator
    {
        private static readonly Dictionary<string, Dictionary<string, string>> LanguageToKeyValuePairs = new Dictionary<string, Dictionary<string, string>>();
        private static readonly List<string> Keys = new List<string>();

        public static void CreateJsons(string text, TextAsset schema)
        {
            Debug.Log("Starting parsing");
            if (!ParseFile(text)) return;
            Debug.Log("Parsing finished, validating keys");
            if (!ValidateKeys(schema)) return;
            Debug.Log("Keys validate. Creating JSONs");
            SaveToJson();
            Debug.Log("JSONs created");
        }

        private static bool ValidateKeys(TextAsset schema)
        {
            var schemaDict = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(schema.text);
            var errors = "";
            var schemaKeys = schemaDict["data"].ToList();

            foreach (var key in schemaKeys.Where(key => !Keys.Contains(key)))
            {
                if (!key.IsNullOrEmpty())
                    errors += $"Key \"{key}\" presents in schema and can't be found in CSV file\n";
            }

            foreach (var key in Keys.Where(key => !schemaKeys.Contains(key)))
            {
                if (!key.IsNullOrEmpty())
                    errors += $"Key \"{key}\" presents in CSV file and can't be found in schema\n";
            }

            if (string.IsNullOrEmpty(errors)) return true;

            Debug.LogError(errors);
            return false;
        }

        private static bool ParseFile(string text)
        {
            LanguageToKeyValuePairs.Clear();
            Keys.Clear();

            var table = CSVParser.LoadFromString(text);

            if (table.Count <= 1)
            {
                Debug.LogError("CSV file with no values");
                return false;
            }

            var languages = new List<string>();
            var languagesLine = table[0];

            for (int i = 0; i < languagesLine.Count; ++i)
            {
                if (languages.Contains(languagesLine[i]))
                {
                    Debug.LogErrorFormat("There are two columns for language: {0}", languagesLine[i]);
                    return false;
                }

                languages.Add(languagesLine[i]);
                if (!languagesLine[i].IsNullOrEmpty())
                    LanguageToKeyValuePairs.Add(languagesLine[i], new Dictionary<string, string>());
                else if (i != 0)
                {
                    break;
                }
            }

            for (int i = 1; i < table.Count; ++i)
            {
                var currentLine = table[i];

                var currentKey = currentLine[0];
                if (!Keys.Contains(currentKey))
                {
                    Keys.Add(currentKey);
                }
                else
                {
                    Debug.LogErrorFormat("Table contains not unique key: {0}", currentKey);
                    return false;
                }

                if (currentKey.IsNullOrEmpty()) continue;

                for (int j = 1; j < languages.Count; ++j)
                {
                    LanguageToKeyValuePairs[languages[j]].Add(currentKey, currentLine[j]);
                }
            }

            return true;
        }

        private static void SaveToJson()
        {
            foreach (var pair in LanguageToKeyValuePairs)
            {
                var curLan = pair.Key;
                var curDict = pair.Value;

                var writer =
                    new StreamWriter(
                        "Assets/Scripts/Services/Core/Localization/Resources/localization_" + curLan + ".json",
                        false);
                var finalJson = "{\n\"lang\": \""
                                + curLan
                                + "\",\n\"data\": "
                                + JsonConvert.SerializeObject(curDict)
                                + "\n}";
                writer.WriteLine(finalJson);
                writer.Close();
            }
            AssetDatabase.Refresh();
        }
    }
}
#endif