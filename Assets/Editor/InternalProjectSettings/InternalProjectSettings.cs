using System.IO;
using IdxZero.Application.Settings.Installer;
using IdxZero.Gameplay.Settings;
using IdxZero.MainMenu.Settings.Installer;
using IdxZero.Services.Localization;
using IdxZero.Services.RemoteConfig;
using UnityEditor;
using Zenject;

namespace IdxZero.Editor
{
    public static class InternalProjectSettings
    {
        private const string DefaultSettingsPath = "Assets/Scripts";

        [MenuItem("ProjectSettings/ApplicationSettings")]
        public static void ShowApplicationSettings()
        {
            ShowAssetByType<ApplicationSettingsInstaller>();
        }

        [MenuItem("ProjectSettings/ServicesSettings/LocalizationSettings")]
        public static void ShowServicesSettings()
        {
            ShowAssetByType<LocalizationSettingsInstaller>();
        }

        [MenuItem("ProjectSettings/ServicesSettings/RemoteConfigSettings")]
        public static void ShowRemoteConfigSettings()
        {
            ShowAssetByType<RemoteConfigSettingsInstaller>();
        }

        [MenuItem("ProjectSettings/MainMenuSettings")]
        public static void ShowMainMenuSettings()
        {
            ShowAssetByType<MainMenuSettingsInstaller>();
        }

        [MenuItem("ProjectSettings/GameplaySettings")]
        public static void ShowGameplaySettings()
        {
            ShowAssetByType<GameplaySettingsInstaller>();
        }

        private static void ShowAssetByType<T>() where T : ScriptableObjectInstaller
        {
            string filterString = "t:" + typeof(T).FullName;
            string[] guids = AssetDatabase.FindAssets(filterString, new[] { DefaultSettingsPath });
            if (guids.Length != 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                SelectAssetByPath<T>(path);
            }
        }

        private static void SelectAssetByPath<T>(string path) where T : ScriptableObjectInstaller
        {
            T asset;
            if (File.Exists(path))
            {
                asset = AssetDatabase.LoadAssetAtPath<T>(path);
            }
            else
                return;
            EditorGUIUtility.PingObject(asset);
            Selection.activeObject = asset;
        }
    }
}
