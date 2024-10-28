using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using BuildingSystem.SO;
using PropBehaviours;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [DisallowMultipleComponent]
    public class DiscoData : Singleton<DiscoData>, ISaveLoad
    {
        public PlacementDataHandler placementDataHandler => MapGeneratorSystem.Instance.placementDataHandler;
        public MapData MapData => MapGeneratorSystem.Instance.MapData;
        public Inventory inventory = new();
        public HashSet<StoreItemSO> AllInGameItems { get; private set; }

        public List<IPropUnit> GetPropList => placementDataHandler.GetPropList;

        public override void Initialize()
        {
            AllInGameItems = Resources.LoadAll<StoreItemSO>("ScriptableObjects/").ToHashSet();
        }

        public void LoadData(GameData gameData)
        {
            inventory = new Inventory(gameData);
            Debug.Log("Disco Data Loaded");
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.SavedInventoryDatas.Clear();

            foreach (var pair in inventory.Items)
                gameData.SavedInventoryDatas.Add(pair.ConvertToInvetorySaveData());
        }

        public StoreItemSO FindItemByID(int ID)
        {
            return AllInGameItems.FirstOrDefault(x => x.ID == ID);
        }
    }
    
    public enum eDanceStyle
    {
        Default,
        Hiphop
    }

    public enum eUISlot
    {
        ItemSlot,
        InventorySlot,
        ExtentionSlot
    }
    
    public enum ePlacementLayer
    {
        BaseSurface, // General BaseSurface Placement
        FloorProp, // Objects placed on the Floor
        WallProp, // Objects placed ont the Wall
        Null
    }
    
    public enum eMaterialLayer
    {
        FloorMaterial,
        WallMaterial,
        Null
    }

    public class ConstantVariables
    {
        public const int MaxMapSizeX = 50;
        public const int MaxMapSizeY = 50;
        public const int PathFinderGridSize = 4;
        public const int FloorLayerID = 7;
        public const int WalllayerID = 8;
    }
}