using System;
using System.Collections.Generic;
using CharacterCustomization.UI;
using PropBehaviours;
using SaveAndLoad;
using SerializableTypes;
using UnityEngine;

namespace Data.New
{
    [Serializable]
    public class NewGameData
    {
        public string fileName;
        public PlayerCustomizationIndexData playerCustomizationData;
        public SavableMapData mapData;
        // TODO : Inventory Data Ekle

        public NewGameData()
        {
            playerCustomizationData = new PlayerCustomizationIndexData();
            mapData = new SavableMapData();
        }

    }
    
    [Serializable]
    public class PlayerCustomizationIndexData
    {
        public int playerGenderIndex;
        public int playerHeadIndex;
        public int playerHairIndex;
        public int playerAccessoriesIndex;
        public int playerTopIndex;
        public int playerBottomIndex;
        public int playerShoesIndex;

        public PlayerCustomizationIndexData()
        {
            playerGenderIndex = 0;
            playerHeadIndex = 0;
            playerHairIndex = 0;
            playerAccessoriesIndex = 0;
            playerTopIndex = 0;
            playerBottomIndex = 0;
            playerShoesIndex = 0;
        }
        
        public PlayerCustomizationIndexData(PlayerCustomizationUI.PlayerEquipments playerEquipments)
        {
            playerGenderIndex = playerEquipments.PlayerGender == eGenderType.Male ? 0 : 1;
            playerHeadIndex = playerEquipments.EquipedItems[PlayerCustomizationUI.BodyPart.Head].index;
            playerHairIndex = playerEquipments.EquipedItems[PlayerCustomizationUI.BodyPart.Hair].index;
            playerAccessoriesIndex = playerEquipments.EquipedItems[PlayerCustomizationUI.BodyPart.Accessories].index;
            playerTopIndex = playerEquipments.EquipedItems[PlayerCustomizationUI.BodyPart.Top].index;
            playerBottomIndex = playerEquipments.EquipedItems[PlayerCustomizationUI.BodyPart.Bottom].index;
            playerShoesIndex = playerEquipments.EquipedItems[PlayerCustomizationUI.BodyPart.Shoes].index;
        }
    }

    [Serializable]
    public class SavableMapData
    {
        [Serializable]
        public class WallSaveData
        {
            // Default = -1
            public Vector3Int cellPosition = -Vector3Int.one;
            public int assignedMaterialID = -1;

            public WallSaveData(Vector3Int cellPosition, int materialID)
            {
                this.cellPosition = cellPosition;
                assignedMaterialID = materialID;
            }

            public static WallSaveData Convert(WallData wallData)
            {
                WallSaveData output = new WallSaveData(wallData.CellPosition, wallData.assignedMaterialID);
                return output;
            }
        }
        
        [Serializable]
        public class FloorSaveData
        {
            // Default = -1
            public Vector3Int cellPosition = -Vector3Int.one;
            public int assignedMaterialID = -1;

            public FloorSaveData(Vector3Int cellPosition, int materialID)
            {
                this.cellPosition = cellPosition;
                assignedMaterialID = materialID;
            }

            public static FloorSaveData Convert(FloorData floorData)
            {
                FloorSaveData output = new FloorSaveData(floorData.CellPosition, floorData.assignedFloorTile.assignedMaterialID);
                return output;
            }
        }
        
        [Serializable]
        public class PlacementSaveData
        {
            // Default = -1
            public int PropID = -1;
            public Vector3 PlacedPosition = -Vector3.one;
            public Vector3 EularAngles = -Vector3.one;

            public static PlacementSaveData Convert(IPropUnit unit)
            {
                PlacementSaveData output = new PlacementSaveData();
                output.PropID = unit.ID;
                output.PlacedPosition = unit.transform.position;
                output.EularAngles = unit.transform.rotation.eulerAngles;

                return output;
            }
        }
        
        public bool isWallOnX;
        public int wallDoorIndex;
        public Vector2Int mapSize;
        public List<WallSaveData> wallDatas;
        public SerializableDictionary<Vector3Int, FloorSaveData> floorDatas;
        public List<PlacementSaveData> placementDatas;

        public SavableMapData()
        {
            isWallOnX = true;
            mapSize = new Vector2Int(11, 11);
            wallDoorIndex = mapSize.x <= 6 ? mapSize.x - 1 : 6;

            wallDatas = new List<WallSaveData>();
            
            var defaltMaterialID = -1;
            for (var x = 1; x <= mapSize.x; x++)
                wallDatas.Add(new WallSaveData(new Vector3Int(x,0,0), defaltMaterialID));

            for (var y = 1; y <= mapSize.y; y++)
                wallDatas.Add(new WallSaveData(new Vector3Int(0, 0, y), defaltMaterialID));
            
            floorDatas = new SerializableDictionary<Vector3Int, FloorSaveData>();
            for (var x = 0; x < ConstantVariables.MaxMapSizeX; x++)
            for (var y = 0; y < ConstantVariables.MaxMapSizeY; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, 0, y);
                floorDatas.Add(cellPos, new FloorSaveData(cellPos, defaltMaterialID));
            }
            
            placementDatas = new List<PlacementSaveData>();
        }
    }
}