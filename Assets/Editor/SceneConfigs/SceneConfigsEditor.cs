using IdxZero.Base.Installers;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IdxZero.Editor
{
    public static class SceneConfigsEditor
    {
        public static void SetGameplayDebugConfig()
        {
            UnloadAllScenesExcept("Gameplay", "Assets/Scenes/Gameplay.unity", true);
        }

        public static void SetApplicationConfig()
        {
            UnloadAllScenesExcept("Application", "Assets/Scenes/Application.unity");
        }

        private static Scene UnloadAllScenesExcept(string sceneName, string scenePath, bool isDebug = false)
        {
            Scene openedScene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            if (isDebug)
            {
                TryToSetDebugMode(openedScene, true);
            }
            var projectContext = Resources.Load("ProjectContext", typeof(GameObject));
            if (projectContext != null)
            {
                (projectContext as GameObject).SetActive(!isDebug);
            }

            int c = EditorSceneManager.sceneCount;

            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) { }
            for (int i = 0; i < c; i++)
            {
                UnityEngine.SceneManagement.Scene scene = EditorSceneManager.GetSceneAt(i);
                if (scene.name != sceneName)
                {
                    TryToSetDebugMode(scene, false);
                    EditorSceneManager.CloseScene(scene, false);
                }
            }

            return openedScene;
        }

        private static void TryToSetDebugMode(Scene openedScene, bool isDebugActive)
        {
            if (!openedScene.isLoaded) return;
            var rootObjects = openedScene.GetRootGameObjects();
            int rootObjectCount = rootObjects.Length;
            for (int i = 0; i < rootObjectCount; i++)
            {
                GameObject gameObject = rootObjects[i];
                var debugableInstaller = gameObject.GetComponent<IDebuggableInstaller>();
                if (debugableInstaller != null)
                {
                    debugableInstaller.IsDebugOn = isDebugActive;
                    EditorSceneManager.SaveScene(openedScene);
                }
            }
        }
    }
}
