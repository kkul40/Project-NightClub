using Data;
using EditorNS.OdinEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Debugging_Tools
{
    public class DebuggingView : OdinEditorWindow
    {
        #region Scenes
        [Button]
        [FoldoutGroup("Working Scene")]
        public void OpenMainMenu()
        {
            LoadScene("Assets/_Project/Scene/MainMenu - Demo.unity");
        }
        
        [Button]
        [FoldoutGroup("Working Scene")]
        public void OpenDisco()
        {
            LoadScene("Assets/_Project/Scene/NightClub-Demo.unity");
        }
        #endregion

        #region Save-Load
        
        [Button]
        [FoldoutGroup("Save And Load")]
        public void NewGame()
        {
            SavingAndLoadingSystem.Instance.NewGame();
            LoadScene(EditorSceneManager.GetActiveScene().path);
        }

        [Button]
        [FoldoutGroup("Save And Load")]
        public void Save()
        {
            SavingAndLoadingSystem.Instance.SaveGame();
        }
        
        [Button]
        [FoldoutGroup("Save And Load")]
        public void Load()
        {
            LoadScene(EditorSceneManager.GetActiveScene().path);
        }

        #endregion

        #region Testing
        

        #endregion
        
        
        [MenuItem("Tools/Debug")]
        private static void OpenWindow()
        {
            GetWindow<DebuggingView>().Show();
        }

        private void LoadScene(string path)
        {
            if (Application.isPlaying)
            {
                SceneManager.LoadScene(path);
                return;
            }
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                EditorSceneManager.OpenScene(path);
            
        }
    }
}