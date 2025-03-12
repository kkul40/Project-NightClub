using System;
using System.Collections.Generic;
using SaveAndLoad;
using SerializableTypes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [Serializable]
    public class GameData
    {
        public GameDataExtension.Details Details;
        public bool HasBeenSavedBefore = false;
        public GameDataExtension.PlayerCustomizationIndexData SavedPlayerCustomizationIndexData;
        public Vector2Int SavedMapSize;
        public bool IsWallOnX;
        public int WallDoorIndex;
        public List<GameDataExtension.WallSaveData> SavedWallDatas;
        public SerializableDictionary<Vector3Int, GameDataExtension.FloorSaveData> SavedFloorDatas;
        public List<GameDataExtension.PlacementSaveData> SavedPlacementDatas;
        public GameDataExtension.InventorySaveData SavedInventoryData;
        
        public GameDataExtension.GameSettingsData GameSettingsData;

        // Default GameData
        public GameData()
        {
            Details = new GameDataExtension.Details();
            
            // Player Customization Data
            SavedPlayerCustomizationIndexData = new GameDataExtension.PlayerCustomizationIndexData();

            // Map Size
            SavedMapSize = new Vector2Int(11, 11);

            // Wall Door Index
            IsWallOnX = true;
            WallDoorIndex = SavedMapSize.x <= 6 ? SavedMapSize.x - 1 : 6;

            // Wall Data
            var defaltMaterialID = -1;
            SavedWallDatas = new List<GameDataExtension.WallSaveData>();

            for (var x = 1; x <= SavedMapSize.x; x++)
                SavedWallDatas.Add(GameDataExtension.ConvertToWallSaveData(new Vector3Int(x, 0, 0), defaltMaterialID));

            for (var y = 1; y <= SavedMapSize.y; y++)
                SavedWallDatas.Add(GameDataExtension.ConvertToWallSaveData(new Vector3Int(0, 0, y), defaltMaterialID));

            // Floor Tile Data
            SavedFloorDatas = new SerializableDictionary<Vector3Int, GameDataExtension.FloorSaveData>();
            for (var x = 0; x < ConstantVariables.MaxMapSizeX; x++)
            for (var y = 0; y < ConstantVariables.MaxMapSizeY; y++)
                SavedFloorDatas.Add(new Vector3Int(x, 0, y), new GameDataExtension.FloorSaveData());

            // Placement Data
            SavedPlacementDatas = new List<GameDataExtension.PlacementSaveData>();

            // Inventory Data
            SavedInventoryData = new GameDataExtension.InventorySaveData();
            SavedInventoryData.Balance = 500;

            // Game Settings Data
            GameSettingsData = new GameDataExtension.GameSettingsData();
        }
    }
}