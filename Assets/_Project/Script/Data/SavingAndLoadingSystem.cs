using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
    public class SavingAndLoadingSystem : Singleton<SavingAndLoadingSystem>
    {
        private string fileName = "GameData";
        
        private FileDataHandler _fileDataHandler;
        private GameData _gameData = new GameData();
        private HashSet<ISaveLoad> _saveLoads = new HashSet<ISaveLoad>();

        private void Awake()
        {
            _fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            _saveLoads = MonoBehaviour.FindObjectsOfType<MonoBehaviour>().OfType<ISaveLoad>().ToHashSet();
        }

        private void Start()
        {
            LoadGame();
        }

        public void NewGame()
        {
            _gameData = new GameData();

            Debug.Log("** New Game Created **");
        }

        public void SaveGame()
        {
            foreach (var save in _saveLoads)
            {
                save.SaveData(ref _gameData);
            }
            
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

            foreach (var load in _saveLoads)
            {
                load.LoadData(_gameData);
            }
            
            Debug.Log("** Game Is Loaded **");
            
            Debug.Log("Total Savables : " + _saveLoads.Count);
        }

        public void RegisterForSaveLoad(ISaveLoad saveLoad)
        {
            _saveLoads.Add(saveLoad);
            Debug.Log("Registereed");
        }

        // public static GameData.WallSaveData ConvertToSaveData(WallAssignmentData wallAssignmentData)
        // {
        //     return new GameData.WallSaveData(wallAssignmentData.CellPosition, wallAssignmentData.assignedMaterialID);
        // }
        //
        // public static WallAssignmentData ConvertToAssignmentData(GameData.WallSaveData wallSaveData)
        // {
        //     return new WallAssignmentData(wallSaveData.CellPosition, wallSaveData.AssignedMaterialID);
        // }
    }
}