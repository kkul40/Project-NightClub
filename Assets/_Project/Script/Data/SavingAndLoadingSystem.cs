using System.Collections.Generic;
using System.Linq;
using DiscoSystem;
using UnityEngine;

namespace Data
{
    public class SavingAndLoadingSystem : Singleton<SavingAndLoadingSystem>
    {
        private string fileName = "GameData.json";

        private FileDataHandler _fileDataHandler;
        private GameData _gameData = new();
        private List<ISaveLoad> _saveLoads = new();

        public override void Initialize(GameInitializer gameInitializer)
        {
            _fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            _saveLoads = FindObjectsOfType<MonoBehaviour>().OfType<ISaveLoad>().ToList();
            _gameData = _fileDataHandler.Load();
        }

        private void Start()
        {
            LoadGame();
        }

        public void NewGame()
        {
            _gameData = new GameData();
            _fileDataHandler.Save(_gameData);
            Debug.Log("** New Game is Created **");
        }

        public void SaveGame()
        {
            foreach (var save in _saveLoads) save.SaveData(ref _gameData);

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

            foreach (var load in _saveLoads) load.LoadData(_gameData);
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
        }
    }
}