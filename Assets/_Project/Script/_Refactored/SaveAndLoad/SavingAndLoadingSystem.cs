using System.Collections.Generic;
using System.Linq;
using Data;
using DiscoSystem;
using UnityEngine;

namespace SaveAndLoad
{
    // TODO Refactor This in a way that no class Should be updated Automaticly They will all updated from Scene_Initializer
    public class SavingAndLoadingSystem : Singleton<SavingAndLoadingSystem>
    {
        private string fileName = "GameData.json";

        private FileDataHandler _fileDataHandler;
        private static GameData _gameData = new();
        private List<ISaveLoad> _saveLoads = new();
        private bool isSaveLoadDirty = true;

        private List<ISaveLoad> SaveLoads
        {
            get
            {
                if (isSaveLoadDirty)
                {
                    _saveLoads = _saveLoads.OrderByDescending(t => t.Priority).ToList();
                    isSaveLoadDirty = false;
                }
                return _saveLoads;
            }
        }
        
        public static GameData GameData
        {
            get { return _gameData; }
            set { _gameData = value; }
        }

        public void Initialize()
        {
            _fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            _saveLoads = FindObjectsOfType<MonoBehaviour>().OfType<ISaveLoad>().ToList();
            _gameData = _fileDataHandler.Load();
            isSaveLoadDirty = true;
            
            NewGame();
        }

        public void NewGame()
        {
            _gameData = new GameData();
            _fileDataHandler.Save(_gameData);
            Debug.Log("** New Game is Created **");
        }

        public void SaveGame()
        {
            foreach (var save in SaveLoads)
                save.SaveData(ref _gameData);

            _gameData.HasBeenSavedBefore = true;
            _fileDataHandler.Save(_gameData);

            Debug.Log("** Game Is Saved **");
        }

        public void LoadGame()
        {
            _gameData = _fileDataHandler.Load();

            if (_gameData == null)
            {
                Debug.Log("No data was found. Default data is loading...");
                NewGame();
                _gameData = _fileDataHandler.Load();
            }
            
            foreach (var load in SaveLoads)
                load.LoadData(_gameData);
            
            Debug.Log("** Game Is Loaded **");
        }

        [ContextMenu("New Save File")]
        private void NewSaveFile()
        {
            var temp = new FileDataHandler(Application.persistentDataPath, fileName);
            temp.DeleteData();
        }

        public bool HasBeenSavedBefore()
        {
            if (_gameData == null) return false;
            return _gameData.HasBeenSavedBefore;
        }

        // TODO Application Quite savelemek ister misin diye sor

        public void RegisterForSaveLoad(ISaveLoad saveLoad)
        {
            _saveLoads.Add(saveLoad);
            isSaveLoadDirty = true;
        }
    }
}