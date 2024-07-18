using System;
using System.Collections.Generic;
using BuildingSystem;
using BuildingSystem.SO;
using PlayerScripts;
using SerializableTypes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [Serializable]
    public class GameData
    {
        public PlayerCustomizationIndexData SavedPlayerCustomizationIndexData;
        public Vector2Int SavedMapSize;
        public int WallDoorIndexOnX;
        public List<WallSaveData> SavedWallDatas;
        public SerializableDictionary<Vector3Int, FloorSaveData> SavedFloorDatas;

        [FormerlySerializedAs("SavedPlacementData")] public List<PlacementSaveData> SavedPlacementDatas;
        

        //CTOR

        public GameData()
        {
            // Player Customization Data
            SavedPlayerCustomizationIndexData = new PlayerCustomizationIndexData();

            // Map Size
            SavedMapSize = new Vector2Int(11, 11);

            // Wall Door Index
            WallDoorIndexOnX = SavedMapSize.x <= 6 ? SavedMapSize.x - 1 : 6;

            // Wall Data
            var defaltMaterialID = -1;
            SavedWallDatas = new List<WallSaveData>();

            for (var x = 1; x <= SavedMapSize.x; x++)
                SavedWallDatas.Add(new WallSaveData(new Vector3Int(x, 0, 0), defaltMaterialID));

            for (var y = 1; y <= SavedMapSize.y; y++)
                SavedWallDatas.Add(new WallSaveData(new Vector3Int(0, 0, y), defaltMaterialID));

            // Floor Tile Data
            SavedFloorDatas = new SerializableDictionary<Vector3Int, FloorSaveData>();
            for (var x = 0; x < ConstantVariables.MaxMapSizeX; x++)
            for (var y = 0; y < ConstantVariables.MaxMapSizeY; y++)
                SavedFloorDatas.Add(new Vector3Int(x, 0, y), new FloorSaveData());
            
            // Placement Data
            SavedPlacementDatas = new List<PlacementSaveData>();
        }

        [Serializable]
        public class WallSaveData
        {
            // Default = -1
            public Vector3Int CellPosition = -Vector3Int.one;
            public int AssignedMaterialID = -1;

            public WallSaveData(Vector3Int cellPosition, int assignedMaterialID)
            {
                CellPosition = cellPosition;
                AssignedMaterialID = assignedMaterialID;
            }

            public WallSaveData(WallAssignmentData wallAssignmentData)
            {
                CellPosition = wallAssignmentData.CellPosition;
                AssignedMaterialID = wallAssignmentData.assignedMaterialID;
            }
        }

        [Serializable]
        public class FloorSaveData
        {
            // Default = -1
            public Vector3Int CellPosition = -Vector3Int.one;
            public int assignedMaterialID = -1;

            public FloorSaveData()
            {
            }

            public FloorSaveData(FloorGridAssignmentData floorGridAssignmentData)
            {
                CellPosition = floorGridAssignmentData.CellPosition;
                assignedMaterialID = floorGridAssignmentData.assignedMaterialID;
            }
        }

        [Serializable]
        public class PlayerCustomizationIndexData
        {
            // Default = 0
            public int playerGenderIndex = 0;
            public int playerHairIndex = 0;
            public int playerBeardIndex = 0;
            public int playerAttachmentIndex = 0;
            public int playerEaringIndex = 0;

            public PlayerCustomizationIndexData()
            {
            }

            public PlayerCustomizationIndexData(PlayerCustomizationLoader playerCustomizationUI)
            {
                playerGenderIndex = playerCustomizationUI.playerGenderIndex;
                playerHairIndex = playerCustomizationUI.playerHairIndex;
                playerBeardIndex = playerCustomizationUI.playerBeardIndex;
                playerAttachmentIndex = playerCustomizationUI.playerAttachmentIndex;
                playerEaringIndex = playerCustomizationUI.playerEaringIndex;
            }
        }

        [Serializable]
        public class PlacementSaveData
        {
            // Default = -1
            public int PropID = -1;
            public Vector3Int PlacedCellPosition = -Vector3Int.one;
            public Vector3 EularAngles = -Vector3.one;
            public Direction Direction = Direction.Down;

            public PlacementSaveData()
            {
            }

            public PlacementSaveData(PlacementDataHandler.NewPlacementData placementData)
            {
                PropID = placementData.ID;
                PlacedCellPosition = placementData.PlacedCellPosition;
                EularAngles = placementData.SettedRotationData.rotation.eulerAngles;
                Direction = placementData.SettedRotationData.direction;
            }
        }
    }
}