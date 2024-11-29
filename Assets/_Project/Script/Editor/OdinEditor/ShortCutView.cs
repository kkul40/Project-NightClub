using Data;
using Root;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.OdinEditor
{
    public class ShortCutView : OdinEditorWindow
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
            LoadScene(SceneManager.GetActiveScene().path);
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
            LoadScene(SceneManager.GetActiveScene().path);
        }

        #endregion

        #region Testing

        [FoldoutGroup("Testing")]
        public bool ShowPlacement;
        
        [FoldoutGroup("Testing")]
        public bool ShowPathFinderNode;
        
        [FoldoutGroup("Testing")]
        public bool ShowAvaliableLeanPositions;
        #endregion

        private void Update()
        {
            if (!Application.isPlaying) return;

            KDebug.Instance.showPlacements = ShowPlacement;
            KDebug.Instance.showPathFinder = ShowPathFinderNode;
            KDebug.Instance.showAvaliableLeanPosition = ShowAvaliableLeanPositions;
        }

        [MenuItem("Tools/Debug")]
        private static void OpenWindow()
        {
            GetWindow<ShortCutView>().Show();
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