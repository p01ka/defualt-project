using UnityEditor;
using UnityEngine;

namespace IdxZero.Editor
{
    public class HelperButtonsEditor : UnityEditor.Editor
    {
        [MenuItem("HelperButtons/START_GAMEPLAY_WITH_CLEARING_PREFS")]
        public static void StartGameplayWithClearingPrefs()
        {
            PlayerPrefs.DeleteAll();
            EditorApplication.EnterPlaymode();
        }
    }
}
