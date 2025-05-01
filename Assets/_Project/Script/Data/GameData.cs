using System;
using System.Collections.Generic;
using SaveAndLoad;
using SerializableTypes;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class GameData
    {
        public SaveDetails saveDetails;
        public bool hasBeenSavedBefore = false;
        public GameDataExtension.PlayerCustomizationIndexData savedPlayerCustomizationIndexData;
        public Vector2Int savedMapSize;
        public bool isWallOnX;
        public int wallDoorIndex;
        public List<GameDataExtension.WallSaveData> savedWallDatas;
        public SerializableDictionary<Vector3Int, GameDataExtension.FloorSaveData> savedFloorDatas;
        public List<GameDataExtension.PlacementSaveData> savedPlacementDatas;
        public GameDataExtension.InventorySaveData savedInventoryData;
        public GameDataExtension.GameSettingsData gameSettingsData;

        // Default GameData
        public GameData()
        {
            saveDetails = new SaveDetails();
            
            // Player Customization Data
            savedPlayerCustomizationIndexData = new GameDataExtension.PlayerCustomizationIndexData();

            // Map Size
            savedMapSize = new Vector2Int(11, 11);

            // Wall Door Index
            isWallOnX = true;
            wallDoorIndex = savedMapSize.x <= 6 ? savedMapSize.x - 1 : 6;

            // Wall Data
            var defaltMaterialID = -1;
            savedWallDatas = new List<GameDataExtension.WallSaveData>();

            for (var x = 1; x <= savedMapSize.x; x++)
                savedWallDatas.Add(GameDataExtension.ConvertToWallSaveData(new Vector3Int(x, 0, 0), defaltMaterialID));

            for (var y = 1; y <= savedMapSize.y; y++)
                savedWallDatas.Add(GameDataExtension.ConvertToWallSaveData(new Vector3Int(0, 0, y), defaltMaterialID));

            // Floor Tile Data
            savedFloorDatas = new SerializableDictionary<Vector3Int, GameDataExtension.FloorSaveData>();
            for (var x = 0; x < ConstantVariables.MaxMapSizeX; x++)
            for (var y = 0; y < ConstantVariables.MaxMapSizeY; y++)
                savedFloorDatas.Add(new Vector3Int(x, 0, y), new GameDataExtension.FloorSaveData());

            // Placement Data
            savedPlacementDatas = new List<GameDataExtension.PlacementSaveData>();

            // Inventory Data
            savedInventoryData = new GameDataExtension.InventorySaveData();

            // Game Settings Data
            gameSettingsData = new GameDataExtension.GameSettingsData();
        }
    }
    
    [Serializable]
    public class SaveDetails
    {
        public string fileName;
        public string parentName;
        public string creationDate;
        public string lastSaveDate;
        public float playTime;

        public SaveDetails CreateNew(string name)
        {
            fileName = name;
            parentName = "";
            creationDate = GetCurrentDate() + " : " + GetCurrentTime();
            lastSaveDate = creationDate;
            playTime = 0f;

            return this;
        }
        
        public SaveDetails Save(float playeTimeInSeconds)
        {
            lastSaveDate = GetCurrentDate();
            playTime = playeTimeInSeconds;
    
            return this;
        }
        
        public static string GetCurrentDate()
        {
            DateTimeOffset Date = DateTime.Now;

            int month = Date.Month;
            int day = Date.Day;
            int year = Date.Year;
        
            return $"{month}/{day}/{year}";
        }

        public static string GetCurrentTime()
        {
            DateTimeOffset Date = DateTime.Now;

            int hour = Date.Hour;
            int minute = Date.Minute;
            int second = Date.Second;

            return $"{hour}:{minute}";
        }
        
        public string ConvertToHour(float seconds)
        {
            int converted = (int)seconds;
            int hour = converted / 3600;
            int min = (converted / 60) % 60;
            int sec = converted % 60;

            return $"{hour}:{min}:{sec}";
        }
    }
}