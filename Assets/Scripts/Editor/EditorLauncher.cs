using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor
{
    public class EditorLauncher 
    {
#if UNITY_EDITOR
        [InitializeOnLoad]
        public class EditorInit
        {
            static EditorInit()
            {
                string pathOfFirstScene = EditorBuildSettings.scenes[0].path;
                var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
                EditorSceneManager.playModeStartScene = sceneAsset;
                Debug.Log(pathOfFirstScene + " was set as default play mode scene");
            }
        }
#endif
    }
}
