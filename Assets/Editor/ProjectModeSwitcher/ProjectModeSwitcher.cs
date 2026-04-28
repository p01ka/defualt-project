using System.Linq;
using UnityEditor;
using System.Collections.Generic;

namespace IdxZero.Editor
{
    public class ProjectModeSwitcher
    {
        internal static readonly HashSet<string> SYMBOLS = new HashSet<string> { "RELEASE_MODE" };

        [MenuItem("ProjectMode/APPLICATION_RELEASE_MODE")]
        public static void ActiveReleaseConfig()
        {
            AddMissingSymbols(BuildTarget.Android);
            AddMissingSymbols(BuildTarget.iOS);
            SceneConfigsEditor.SetApplicationConfig();
        }

        [MenuItem("ProjectMode/APPLICATION_DEBUG_MODE")]
        public static void DisableReleaseConfig()
        {
            DeleteMissingSymbols(BuildTarget.Android);
            DeleteMissingSymbols(BuildTarget.iOS);
            SceneConfigsEditor.SetApplicationConfig();
        }

        [MenuItem("ProjectMode/GAMEPLAY_ONLY_DEBUG_MODE")]
        public static void GameplayOnlyDebugMode()
        {
            DeleteMissingSymbols(BuildTarget.Android);
            DeleteMissingSymbols(BuildTarget.iOS);
            SceneConfigsEditor.SetGameplayDebugConfig();
        }

        static void AddMissingSymbols(BuildTarget buildTarget)
        {
            var currentGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentGroup).Split(';').ToList();
            var missing = SYMBOLS.Except(defines).ToList();
            defines.AddRange(missing);

            if (missing.Count > 0)
                PlayerSettings.SetScriptingDefineSymbolsForGroup(currentGroup, string.Join(";", defines));
        }

        static void DeleteMissingSymbols(BuildTarget buildTarget)
        {
            var currentGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentGroup).Split(';').ToList();
            var deleted = defines.Except(SYMBOLS).ToList();

            PlayerSettings.SetScriptingDefineSymbolsForGroup(currentGroup, string.Join(";", deleted));
        }
    }
}