using System;
using System.Collections.Generic;
using ExtensionMethods;
using SerializableTypes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [Serializable]
    public class GameData
    {
        public bool HasBeenSavedBefore = false;
        public GameDataExtension.PlayerCustomizationIndexData SavedPlayerCustomizationIndexData;
        public Vector2Int SavedMapSize;
        public bool IsWallOnX;
        public int WallDoorIndex;
        public List<GameDataExtension.WallSaveData> SavedWallDatas;
        public SerializableDictionary<Vector3Int, GameDataExtension.FloorSaveData> SavedFloorDatas;
        public List<GameDataExtension.PlacementSaveData> SavedPlacementDatas;
        public List<GameDataExtension.InventorySaveData> SavedInventoryDatas;
        public GameDataExtension.GameSettingsData GameSettingsData;

        // Default GameData
        public GameData()
        {
            // Player Customization Data
            SavedPlayerCustomizationIndexData = new GameDataExtension.PlayerCustomizationIndexData();

            // Map Size
            SavedMapSize = new Vector2Int(11, 11);

            // Wall Door Index
            IsWallOnX = false;
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
            SavedInventoryDatas = new List<GameDataExtension.InventorySaveData>();

            // Game Settings Data
            GameSettingsData = new GameDataExtension.GameSettingsData();
        }
    }
}