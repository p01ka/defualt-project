using UnityEditor;
using System.IO;

namespace IdxZero.Editor
{
    public class CustomMenuEntries
    {
        public const string PATH_TO_MONOBEHAVIOUR_TEMPLATE = @"Assets\Editor\Templates\Texts\NewMonoBehaivour.cs.txt";
        public const string PATH_TO_MVP_SCRIPT_TEMPLATE = @"Assets\Editor\Templates\Texts\NewMVPView.cs.txt";
        public const string PATH_TO_STATE_MACHINE_TEMPLATE = @"Assets\Editor\Templates\Texts\NewStateMachine.cs.txt";
        public const string PATH_TO_CSHARP_TEMPLATE = @"Assets\Editor\Templates\Texts\NewCSharpScript.cs.txt";

        [MenuItem("Assets/MonoBehaviour Script")]
        public static void CreateMonobehaviorScript(MenuCommand cmd)
        {
            if (Selection.activeObject == null) return;
            string path = GetCreatedAssetPath();
            CreateScriptAsset(PATH_TO_MONOBEHAVIOUR_TEMPLATE, Path.Combine(path, "NewMonoBehaviourScript.cs"));
            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/MVP C# Script")]
        public static void CreateMVPScript(MenuCommand cmd)
        {
            if (Selection.activeObject == null) return;
            string path = GetCreatedAssetPath();
            CreateScriptAsset(PATH_TO_MVP_SCRIPT_TEMPLATE, Path.Combine(path, "NewMVPScript.cs"));
            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/State machine C# Script")]
        public static void CreateStateMachineScript(MenuCommand cmd)
        {
            if (Selection.activeObject == null) return;
            string path = GetCreatedAssetPath();
            CreateScriptAsset(PATH_TO_STATE_MACHINE_TEMPLATE, Path.Combine(path, "NewStateMachineScript.cs"));
            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/CSharp Script")]
        public static void CreateCSHARPScript(MenuCommand cmd)
        {
            if (Selection.activeObject == null) return;
            string path = GetCreatedAssetPath();
            CreateScriptAsset(PATH_TO_CSHARP_TEMPLATE, Path.Combine(path, "NewCSharpScript.cs"));
            AssetDatabase.Refresh();
        }

        private static string GetCreatedAssetPath()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (File.Exists(path))
                path = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(path)) path = "Assets/";
            return path;
        }

        private static void CreateScriptAsset(string templatePath, string destName)
        {
#if UNITY_IOS
        templatePath = templatePath.Replace(@"\", "/");
#endif

            System.Reflection.MethodInfo info =

            typeof(UnityEditor.ProjectWindowUtil)
                .GetMethod("CreateScriptAsset", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            if (info == null)
            {
                info = typeof(ProjectWindowUtil)
                 .GetMethod("CreateScriptAssetFromTemplateFile", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            }

            if (info != null)
            {
                info.Invoke(null, new object[] { templatePath, destName });
            }
        }
    }
}
