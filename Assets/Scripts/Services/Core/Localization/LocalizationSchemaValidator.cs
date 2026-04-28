using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace IdxZero.Services.Localization
{
    public static class LocalizationSchemaValidator
    {
        public static void Validate(
            Dictionary<CultureInfo, LocalizationSettings.ParsedLocalizationInfo> localizations,
            string[] requiredKeys,
            ref string error)
        {
            foreach (var valuePair in localizations)
                if (!ScrambledEquals(requiredKeys, valuePair.Value.LocalizationDictionary.Keys, out var errors))
                    AddErrors(ref error, errors, valuePair.Key);
        }

        private static void AddErrors(ref string error, Dictionary<string, int> errors, CultureInfo cultureName)
        {
            foreach (var errorsKey in errors.Keys)
            {
                string er = string.Empty;
                if (errors[errorsKey] < 0)
                    er = $"Key \"{errorsKey}\" does not match the scheme.\r\n";
                else if (errors[errorsKey] > 0)
                    er = $"Key \"{errorsKey}\" not found in \"{cultureName.EnglishName}\" localization.\r\n";

                if (!String.IsNullOrEmpty(er))
                {
                    error += er;
                }
            }
        }

        private static bool ScrambledEquals<T>(
            IEnumerable<T> reference,
            IEnumerable<T> comparable,
            out Dictionary<T, int> result)
        {
            var cnt = new Dictionary<T, int>();
            //filling reference
            foreach (var s in reference)
                if (cnt.ContainsKey(s))
                    cnt[s]++;
                else
                    cnt.Add(s, 1);

            //sieving
            foreach (var s in comparable)
                if (cnt.ContainsKey(s))
                    cnt[s]--;
                else
                    cnt.Add(s, -1);

            var keys = cnt.Keys.ToList();
            for (var i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                if (cnt[key] == 0)
                    cnt.Remove(key);
            }

            result = cnt;
            return cnt.Count == 0;
        }
    }
}