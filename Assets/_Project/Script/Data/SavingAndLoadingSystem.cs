using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Data
{
    public class SavingAndLoadingSystem : MonoBehaviour
    {
        private List<ISaveLoad> _saveLoads = new();
        private string path = Application.persistentDataPath + "/PlayerGameData.kdata";

        private GameData _currentGameData = GameData.GetDefaultSaveData();

        public GameData GetGameData => _currentGameData;

        public SavingAndLoadingSystem(bool CreateDefaultSave)
        {
            Debug.Log(path);
            LoadGame();
        }

        public GameData SaveGame()
        {
            var data = new GameData();

            data.SavedMapSize = DiscoData.Instance.mapData.CurrentMapSize;
            data.WallDoorIndexOnX = DiscoData.Instance.mapData.WallDoorIndex;

            var temp = DiscoData.Instance.mapData.WallDatas.ToList();
            for (var i = 0; i < temp.Count; i++)
                data.SavedWallDatas.Add(new GameData.WallSaveData(temp[i].CellPosition, temp[i].assignedMaterialID));


            var json = JsonUtility.ToJson(data);
            File.WriteAllText(path, json);

            PrintOut(data);

            _currentGameData = data;
            Debug.Log("...Game is Saved...");

            return data;
        }

        public GameData LoadGame()
        {
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<GameData>(json);

                PrintOut(data);

                _currentGameData = data;
                Debug.Log("...Game is Loaded...");
                return data;
            }

            _currentGameData = GameData.GetDefaultSaveData();
            PrintOut(_currentGameData);

            return _currentGameData;
        }

        private void PrintOut(GameData gameData)
        {
            // Debug.Log("Loaded Map Size : " + CurrentSaveData.SavedMapSize);
            // Debug.Log("Loaded Wall count : " + CurrentSaveData.SavedWallDatas.Count);
        }
    }
}