using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening.Plugins.Core.PathCore;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Data
{
    public class SavingAndLoadingSystem : Singleton<SavingAndLoadingSystem>
    {
        private string fileName = "GameData";

        private FileDataHandler _fileDataHandler;
        private GameData _gameData = new();
        private List<ISaveLoad> _saveLoads = new();

        private void Awake()
        {
            if (Instance != this)
            {
                Destroy(this);
                return;
            }

            DontDestroyOnLoad(this);
            _fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            _saveLoads = FindObjectsOfType<MonoBehaviour>().OfType<ISaveLoad>().ToList();
        }

        private void Start()
        {
            LoadGame();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("Scene loaded");
            _saveLoads = FindObjectsOfType<MonoBehaviour>().OfType<ISaveLoad>().ToList();
            LoadGame();
        }

        public void OnSceneUnloaded(Scene scene)
        {
            Debug.Log("Scene unloaded");
            SaveGame();
        }

        public void NewGame()
        {
            _gameData = new GameData();
            Debug.Log("** New Game is Created **");
        }

        public void SaveGame()
        {
            foreach (var save in _saveLoads) save.SaveData(ref _gameData);

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

        private void OnApplicationQuit()
        {
            // SaveGame();
        }

        public void RegisterForSaveLoad(ISaveLoad saveLoad)
        {
            _saveLoads.Add(saveLoad);
        }
    }
}