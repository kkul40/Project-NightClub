using System;
using System.Collections.Generic;
using CharacterCustomization.UI;
using PropBehaviours;
using SerializableTypes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data.New
{
    [Serializable]
    public class NewGameData
    {
        public string fileName;
        [FormerlySerializedAs("playerCustomizationData")] public Save_PlayerEquipmentsData savePlayerCustomizationData;
        public Save_MapData mapData;
        // TODO : Inventory Data Ekle

        public NewGameData()
        {
            savePlayerCustomizationData = new Save_PlayerEquipmentsData();
            mapData = new Save_MapData();
        }

    }
    
    [Serializable]
    public class Save_PlayerEquipmentsData
    {
        public int playerGenderIndex;
        public int playerHeadIndex;
        public int playerHairIndex;
        public int playerAccessoriesIndex;
        public int playerTopIndex;
        public int playerBottomIndex;
        public int playerShoesIndex;

        public Save_PlayerEquipmentsData()
        {
            playerGenderIndex = 0;
            playerHeadIndex = 0;
            playerHairIndex = 0;
            playerAccessoriesIndex = 0;
            playerTopIndex = 0;
            playerBottomIndex = 0;
            playerShoesIndex = 0;
        }
        
        public Save_PlayerEquipmentsData(PlayerCustomizationUI.PlayerEquipments playerEquipments)
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
    public class Save_MapData
    {
        [Serializable]
        public class Save_WallData
        {
            public int assignedMaterialID = -1;

            public Save_WallData(int materialID)
            {
                assignedMaterialID = materialID;
            }
        }
        
        [Serializable]
        public class Save_FloorData
        {
            // Default = -1
            public Vector3Int cellPosition = -Vector3Int.one;
            public int assignedMaterialID = -1;

            public Save_FloorData(Vector3Int cellPosition, int materialID)
            {
                this.cellPosition = cellPosition;
                assignedMaterialID = materialID;
            }

            public static Save_FloorData Convert(FloorData floorData)
            {
                Save_FloorData output = new Save_FloorData(floorData.CellPosition, floorData.assignedFloorTile.assignedMaterialID);
                return output;
            }
        }
        
        [Serializable]
        public class Save_PlacementData
        {
            // Default = -1
            public int PropID = -1;
            public Vector3 PlacedPosition = -Vector3.one;
            public Vector3 EularAngles = -Vector3.one;

            public static Save_PlacementData Convert(IPropUnit unit)
            {
                Save_PlacementData output = new Save_PlacementData();
                output.PropID = unit.ID;
                output.PlacedPosition = unit.transform.position;
                output.EularAngles = unit.transform.rotation.eulerAngles;

                return output;
            }
        }
        
        public bool isWallOnX;
        public int wallDoorIndex;
        public Vector2Int mapSize;
        public SerializableDictionary<Vector3Int, Save_WallData> wallDatas;
        public SerializableDictionary<Vector3Int, Save_FloorData> floorDatas;
        public List<Save_PlacementData> placementDatas;

        public Save_MapData()
        {
            isWallOnX = true;
            mapSize = new Vector2Int(11, 11);
            wallDoorIndex = mapSize.x <= 6 ? mapSize.x - 1 : 6;

            wallDatas = new SerializableDictionary<Vector3Int, Save_WallData>();
            
            var defaltMaterialID = -1;
            for (var x = 1; x <= mapSize.x; x++)
            {
                Vector3Int cellPos = new Vector3Int(x, 0, 0);
                wallDatas.Add(cellPos, new Save_WallData(defaltMaterialID));
            }

            for (var y = 1; y <= mapSize.y; y++)
            {
                Vector3Int cellPos = new Vector3Int(0, 0, y);
                wallDatas.Add(cellPos, new Save_WallData(defaltMaterialID));
            }
            
            floorDatas = new SerializableDictionary<Vector3Int, Save_FloorData>();
            for (var x = 0; x < ConstantVariables.MaxMapSizeX; x++)
            for (var y = 0; y < ConstantVariables.MaxMapSizeY; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, 0, y);
                floorDatas.Add(cellPos, new Save_FloorData(cellPos, defaltMaterialID));
            }
            
            placementDatas = new List<Save_PlacementData>();
        }
    }
}