using _Initializer;
using Data.New;
using Root;
using SaveAndLoad;
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
            SaveLoadSystem.Instance.NewGame(new NewGameData());
            LoadScene(SceneManager.GetActiveScene().path);
        }

        [Button]
        [FoldoutGroup("Save And Load")]
        public void Save()
        {
            SaveLoadSystem.Instance.SaveGame();
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
        public bool ShowEmployeeNodes;
        
        [FoldoutGroup("Testing")]
        public bool ShowAvaliableLeanPositions;
        
        [FoldoutGroup("Testing")]
        public bool ShowActivityNodes;
        #endregion

        private void Update()
        {
            if (!Application.isPlaying) return;

            ServiceLocator.Get<KDebug>().showPlacements = ShowPlacement;
            ServiceLocator.Get<KDebug>().showPathFinder = ShowPathFinderNode;
            ServiceLocator.Get<KDebug>().showEmployeeFinder = ShowEmployeeNodes;
            ServiceLocator.Get<KDebug>().showAvaliableLeanPosition = ShowAvaliableLeanPositions;
            ServiceLocator.Get<KDebug>().showActivityNodes = ShowActivityNodes;
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