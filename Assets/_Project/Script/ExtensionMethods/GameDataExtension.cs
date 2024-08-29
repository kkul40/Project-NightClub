using System;
using System.Collections.Generic;
using BuildingSystem;
using BuildingSystem.SO;
using PlayerScripts;
using UnityEngine;

namespace Data
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

        public static FloorSaveData ConvertToFloorSaveData(this FloorGridAssignmentData floorGridAssignmentData)
        {
            FloorSaveData floorSaveData = new FloorSaveData();
            
            floorSaveData.CellPosition = floorGridAssignmentData.CellPosition;
            floorSaveData.assignedMaterialID = floorGridAssignmentData.assignedMaterialID;

            return floorSaveData;
        }
        
        [Serializable]
        public class WallSaveData
        {
            // Default = -1
            public Vector3Int CellPosition = -Vector3Int.one;
            public int AssignedMaterialID = -1;
        }

        public static WallSaveData ConvertToWallSaveData(this WallAssignmentData wallAssignmentData)
        {
            WallSaveData wallSaveData = new WallSaveData();
            
            wallSaveData.CellPosition = wallAssignmentData.CellPosition;
            wallSaveData.AssignedMaterialID = wallAssignmentData.assignedMaterialID;

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
            public int InventoryItemID;
            public int Amount;
        }
        
        public static InventorySaveData ConvertToInvetorySaveData(this KeyValuePair<StoreItemSO, int> inventoryItem)
        {
            InventorySaveData invetoryData = new InventorySaveData();
            
            invetoryData.InventoryItemID = inventoryItem.Key.ID;
            invetoryData.Amount = inventoryItem.Value;
            
            return invetoryData;
        }
        
        [Serializable]
        public class PlacementSaveData
        {
            // Default = -1
            public int PropID = -1;
            public Vector3Int PlacedCellPosition = -Vector3Int.one;
            public Vector3 EularAngles = -Vector3.one;
            public Direction Direction = Direction.Down;
        }
        
        public static PlacementSaveData ConvertToPlacementSaveData(this PlacementDataHandler.PlacementData placementData)
        {
            PlacementSaveData placementSaveData = new PlacementSaveData();
                
            placementSaveData.PropID = placementData.ID;
            placementSaveData.PlacedCellPosition = placementData.PlacedCellPosition;
            placementSaveData.EularAngles = placementData.SettedRotationData.rotation.eulerAngles;
            placementSaveData.Direction = placementData.SettedRotationData.direction;
                
            return placementSaveData;
        }
        
        [Serializable]
        public class PlayerCustomizationIndexData
        {
            // Default = 0
            public int playerGenderIndex = 1;
            public int playerHairIndex = 1;
            public int playerBeardIndex = 0;
            public int playerAttachmentIndex = 1;
            public int playerEaringIndex = 1;
        }

        public static PlayerCustomizationIndexData ConvertPlayerCustomizationIndexData(this PlayerCustomization playerCustomization)
        {
            PlayerCustomizationIndexData playerCustomizationIndexData = new PlayerCustomizationIndexData();   
            
            playerCustomizationIndexData.playerGenderIndex = playerCustomization.playerGenderIndex;
            playerCustomizationIndexData.playerHairIndex = playerCustomization.playerHairIndex;
            playerCustomizationIndexData.playerBeardIndex = playerCustomization.playerBeardIndex;
            playerCustomizationIndexData.playerAttachmentIndex = playerCustomization.playerAttachmentIndex;
            playerCustomizationIndexData.playerEaringIndex = playerCustomization.playerEaringIndex;
            
            return playerCustomizationIndexData;
        }
    }
}