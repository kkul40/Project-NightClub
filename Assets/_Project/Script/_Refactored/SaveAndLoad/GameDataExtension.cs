using System;
using System.Collections.Generic;
using Data;
using Disco_ScriptableObject;
using UnityEngine;
using CharacterCustomization.UI;
using NPCBehaviour;

namespace ExtensionMethods
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