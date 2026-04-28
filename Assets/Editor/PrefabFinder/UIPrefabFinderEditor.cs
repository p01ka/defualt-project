using UnityEditor;

namespace IdxZero.Editor
{
    public class UIPrefabFinderEditor : UnityEditor.Editor
    {
        #region UI_PRERABS
        [MenuItem("PrefabFinder/UI/SCREENS/APPLICATION_SCREEN")]
        public static void OpenApplicationScreen()
        {
            string path = "Assets/Prefabs/Application/UI/ApplicationScreen.prefab";
            AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path));
        }

        [MenuItem("PrefabFinder/UI/SCREENS/SCREENS_KEEPER")]
        public static void OpenScreensKeeper()
        {
            string path = "Assets/Prefabs/UI/Screens/ScreensKeeper.prefab";
            AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path));
        }

        [MenuItem("PrefabFinder/UI/SCREENS/SETTINGS_SCREEN")]
        public static void OpenSettingsScreen()
        {
            string path = "Assets/Prefabs/UI/Screens/SettingsScreen.prefab";
            AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path));
        }
        #endregion UI_PRERABS

        #region POPUPS
        // [MenuItem("PrefabFinder/UI/POPUPS/POPUPS_KEEPER")]
        // public static void OpenPopupKeeper()
        // {
        //     string path = "Assets/Prefabs/UI/Popups/PopupsKeeper.prefab";
        //     AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path));
        // }

        #endregion POPUPS
    }
}
