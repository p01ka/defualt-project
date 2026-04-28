using UnityEditor;
using UnityEngine;

namespace IdxZero.Services.Localization.Editor
{
    [CustomEditor(typeof(LocalizationSettingsInstaller))]
    public class LocalizationSettingsInstallerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            LocalizationSettingsInstaller myScript = (LocalizationSettingsInstaller)target;

            if (GUILayout.Button("Validate localizations"))
            {
                myScript.ValidateLocalizations();
            }

            if (GUILayout.Button("Convert to JSON"))
            {
                myScript.ConvertCsvToJson();
            }

            if (GUILayout.Button("Open localization document"))
            {
                myScript.OpenLocalizationDocument();
            }
        }
    }
}
