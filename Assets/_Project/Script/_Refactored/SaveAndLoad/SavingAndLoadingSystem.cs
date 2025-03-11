using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using DiscoSystem;
using GameEvents;
using UnityEngine;

namespace SaveAndLoad
{
    // TODO Refactor This in a way that no class Should be updated Automaticly They will all updated from Scene_Initializer
    public class SavingAndLoadingSystem : Singleton<SavingAndLoadingSystem>
    {
        private string fileName = "GameData";

        private FileDataHandler _fileDataHandler;
        private static GameData _gameData = new();
        private List<ISavable> _saveLoads = new();
        private bool isSaveLoadDirty = true;

        private float passedSeconds;

        private List<ISavable> SaveLoads
        {
            get
            {
                if (isSaveLoadDirty)
                {
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
            string version = "111";
            
            _fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName + $"{version}.json");
            _saveLoads = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>().ToList();
            _gameData = _fileDataHandler.Load();
            isSaveLoadDirty = true;

            // TODO Make sure there is only one running
            StartCoroutine(TimerCO());
        }
        
        private IEnumerator TimerCO()
        {
            while (true)
            {
                passedSeconds += Time.deltaTime;
                yield return null;
            }
        }

        public void NewGame()
        {
            _gameData = new GameData();
            _fileDataHandler.Save(_gameData);
            Debug.Log("** New Game is Created **");
        }

        public void SaveGame()
        {
            KEvent_SavingAndLoading.TriggerGameSave(_gameData);

            _gameData.Details.Save(_gameData.Details.PlayTime + passedSeconds);
            passedSeconds = 0;

            _fileDataHandler.Save(_gameData);
            
            
            // foreach (var save in SaveLoads)
            //     save.SaveData(ref _gameData);
            //
            // _gameData.HasBeenSavedBefore = true;
            // _fileDataHandler.Save(_gameData);

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
            
            _fileDataHandler.DeleteData();
        }

        public bool HasBeenSavedBefore()
        {
            if (_gameData == null) return false;
            return _gameData.HasBeenSavedBefore;
        }

        // TODO Application Quite savelemek ister misin diye sor

        public void RegisterForSaveLoad(ISavable savable)
        {
            _saveLoads.Add(savable);
            isSaveLoadDirty = true;
        }
    }
}