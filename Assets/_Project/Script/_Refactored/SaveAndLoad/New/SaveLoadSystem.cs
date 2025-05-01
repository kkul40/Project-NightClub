using System.Collections.Generic;
using Data.New;
using DiscoSystem.Building_System.GameEvents;
using UnityEngine;

namespace SaveAndLoad
{
    [CreateAssetMenu(fileName = "Save Load System", menuName = "Persist/SaveLoadSystem")]
    public class SaveLoadSystem : ScriptableObject
    {
        private static SaveLoadSystem instance;
        private static bool _isInitialized = false;
        public static SaveLoadSystem Instance
        {
            get
            {
                if (instance == null)
                    instance = Resources.Load<SaveLoadSystem>("Persistent/Save Load System");

                if (!_isInitialized)
                {
                    _dataService = new FileDataService(new JsonSerializer());
                    _isInitialized = true;
                }
                return instance;
            }
        }

        // *** *** *** *** *** *** *** *** *** *** *** *** *** //
        public NewGameData CurrentGameData;
        private static IDataService _dataService;
        
        public void NewGame(NewGameData gameData)
        {
            CurrentGameData = gameData;
            // TODO : Add Details to gamedata;
            CurrentGameData.fileName = "Empty";
        }
        
        public void SaveGame()
        {
            GameEvent.Trigger(new Event_OnGameSave(ref CurrentGameData));
            _dataService.Save(CurrentGameData);
        }

        public void SaveGameAs(string newFileName)
        {
            GameEvent.Trigger(new Event_OnGameSave(ref CurrentGameData));
            CurrentGameData.fileName = newFileName;
            _dataService.Save(CurrentGameData);
        }

        public void LoadGame(string gameName)
        {
            CurrentGameData = _dataService.Load(gameName);
        }

        public void SelectGame(string gameName)
        {
            CurrentGameData = _dataService.Load(gameName);
        }

        public void ReloadGame()
        {
            LoadGame(CurrentGameData.fileName);
        }

        public void DeleteGame(string gameName)
        {
            _dataService.Delete(gameName);
        }

        public NewGameData GetCurrentData()
        {
            if (CurrentGameData == null) return null;
            return CurrentGameData;
        }

        public NewGameData GetGameData(string name)
        {
            return _dataService.GetData(name);
        }

        public IEnumerable<string> GetList()
        {
            return _dataService.GetSaveFiles;
        }
    }
}