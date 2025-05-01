using Data;
using Data.New;
using Root;
using SaveAndLoad;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.StartMenu
{
    public class SelectLoadFilePage : Menu_UI_Page
    {
       [SerializeField] private Transform contentHolder;
       [SerializeField] private Transform saveDataPrefab;
        public override void Initialize()
        {
            base.Initialize();

            RefreshList();
        }

        private void RefreshList()
        {
            for (int i = contentHolder.childCount - 1; i >= 0; i--)
                Destroy(contentHolder.GetChild(i).gameObject);
            
            var saveNames = SaveLoadSystem.Instance.GetList();

            foreach (var sName in saveNames)
            {
                var prefab = Instantiate(saveDataPrefab, contentHolder);
                UISaveData saveData = prefab.GetComponent<UISaveData>();
                saveData.Init(SaveLoadSystem.Instance.GetGameData(sName));
                saveData.OnDelete += DeleteGame;
                saveData.OnClick += ClickGame;
            }
        }


        [Button]
        public void CreateNewData(string name)
        {
            NewGameData data = new NewGameData();
            data.fileName = name;

            SaveLoadSystem.Instance.CurrentGameData = data;
            
            SaveLoadSystem.Instance.SaveGame();
            
            RefreshList();
        }

        private void DeleteGame(string fileName)
        {
            SaveLoadSystem.Instance.DeleteGame(fileName);
            
            RefreshList();
        }

        private void ClickGame(string fileName)
        {
            SaveLoadSystem.Instance.SelectGame(fileName);
            SceneLoader.Instance.LoadScene(1);
        }
    }
}