using System;
using CharacterCustomization.UI;
using Data;
using PropBehaviours;
using SerializableTypes;
using UnityEngine;

namespace SaveAndLoad
{
    public static class GameDataExtension
    {
        
        [Serializable]
        public class FloorSaveData
        {
            // Default = -1
            public Vector3Int CellPosition = -Vector3Int.one;
            public int assignedMaterialID = -1;
        }

        public static FloorSaveData ConvertToFloorSaveData(this FloorData floorData)
        {
            FloorSaveData floorSaveData = new FloorSaveData();
            
            floorSaveData.CellPosition = floorData.CellPosition;
            floorSaveData.assignedMaterialID = floorData.assignedMaterialID;

            return floorSaveData;
        }
        
        [Serializable]
        public class WallSaveData
        {
            // Default = -1
            public Vector3Int CellPosition = -Vector3Int.one;
            public int AssignedMaterialID = -1;
        }

        public static WallSaveData ConvertToWallSaveData(this WallData wallData)
        {
            WallSaveData wallSaveData = new WallSaveData();
            
            wallSaveData.CellPosition = wallData.CellPosition;
            wallSaveData.AssignedMaterialID = wallData.assignedMaterialID;

            return wallSaveData;
        }

        public static WallSaveData ConvertToWallSaveData(Vector3Int vector3Int, int defaltMaterialID)
        {
            WallSaveData wallSaveData = new WallSaveData();
            
            wallSaveData.CellPosition = vector3Int;
            wallSaveData.AssignedMaterialID = defaltMaterialID;
            
            return wallSaveData;
        }


        [Serializable]
        public class InventorySaveData
        {
            public class Inventory
            {
                public int ItemsID;
                public int ItemAmount;
            }
            
            public int Balance;
            public SerializableDictionary<int, int> Items;

            public InventorySaveData()
            {
                Balance = 99999999;
                Items = new SerializableDictionary<int, int>();
            }
        }
        
        // public static InventorySaveData.Inventory ConvertToInvetorySaveData(this KeyValuePair<StoreItemSO, int> inventoryItem)
        // {
        //     InventorySaveData.Inventory InventoryItem = new InventorySaveData.Inventory();
        //
        //     InventoryItem.ItemsID = inventoryItem.Key.ID;
        //     InventoryItem.ItemAmount = inventoryItem.Value;
        //     
        //     return InventoryItem;
        // }
        
        [Serializable]
        public class PlacementSaveData
        {
            // Default = -1
            public int PropID = -1;
            public Vector3 PlacedPosition = -Vector3.one;
            public Vector3 EularAngles = -Vector3.one;
        }

        public static PlacementSaveData ConverToPlacementSaveData(this IPropUnit unit)
        {
            PlacementSaveData saveData = new PlacementSaveData();

            saveData.PropID = unit.ID;
            saveData.PlacedPosition = unit.WorldPos;
            saveData.EularAngles = unit.transform.rotation.eulerAngles;
            
            return saveData;
        }
        
        // TODO Use The Tuple Variable You Created in DiscoData named PlacedItems
        // public static PlacementSaveData ConvertToPlacementSaveData(this PlacementDataHandler.PlacementData placementData)
        // {
        //     PlacementSaveData placementSaveData = new PlacementSaveData();
        //         
        //     placementSaveData.PropID = placementData.ID;
        //     placementSaveData.PlacedCellPosition = placementData.PlacedCellPosition;
        //     placementSaveData.EularAngles = placementData.SettedRotationData.rotation.eulerAngles;
        //     placementSaveData.Direction = placementData.SettedRotationData.direction;
        //         
        //     return placementSaveData;
        // }
        
        [Serializable]
        public class PlayerCustomizationIndexData
        {
            public int playerGenderIndex = 1;
            public int playerHeadIndex = 1;
            public int playerHairIndex = 0;
            public int playerAccessoriesIndex = 0;
            public int playerTopIndex = 0;
            public int playerBottomIndex = 0;
            public int playerShoesIndex = 0;
        }

        public static PlayerCustomizationIndexData ConvertPlayerCustomizationIndexData(this PlayerCustomizationUI.PlayerEquipments playerEquipments)
        {
            PlayerCustomizationIndexData playerCustomizationIndexData = new PlayerCustomizationIndexData();   
            
            playerCustomizationIndexData.playerGenderIndex = playerEquipments.PlayerGender == eGenderType.Male ? 0 : 1;
            playerCustomizationIndexData.playerHeadIndex = playerEquipments.EquipedItems[PlayerCustomizationUI.BodyPart.Head].index;
            playerCustomizationIndexData.playerHairIndex = playerEquipments.EquipedItems[PlayerCustomizationUI.BodyPart.Hair].index;
            playerCustomizationIndexData.playerAccessoriesIndex = playerEquipments.EquipedItems[PlayerCustomizationUI.BodyPart.Accessories].index;
            playerCustomizationIndexData.playerTopIndex = playerEquipments.EquipedItems[PlayerCustomizationUI.BodyPart.Top].index;
            playerCustomizationIndexData.playerBottomIndex = playerEquipments.EquipedItems[PlayerCustomizationUI.BodyPart.Bottom].index;
            playerCustomizationIndexData.playerShoesIndex = playerEquipments.EquipedItems[PlayerCustomizationUI.BodyPart.Shoes].index;
            
            return playerCustomizationIndexData;
        }

        [Serializable]
        public class GameSettingsData
        {
            public bool isEdgeScrollingEnabled;
            public bool isSnappyCameraEnabled;
            public float MusicVolume = 1;
            public float SoundVolume = 1;
        }
    }
}