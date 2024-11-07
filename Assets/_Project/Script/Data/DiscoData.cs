using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Disco_ScriptableObject;
using ExtensionMethods;
using PropBehaviours;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [DisallowMultipleComponent]
    public class DiscoData : Singleton<DiscoData>, ISaveLoad
    {
        // Verileri Once Data ya yaz daha sonra Modeli datadaki veriye gore guncelle!
        public PlacementDataHandler placementDataHandler => MapGeneratorSystem.Instance.placementDataHandler;
        public MapData MapData => MapGeneratorSystem.Instance.MapData;
        public Inventory inventory = new();
        public Dictionary<int ,StoreItemSO> AllInGameItems { get; private set; }
        public Dictionary<int, DrinkSO> AllInGameDrinks { get; private set; }

        public List<IPropUnit> GetPropList => placementDataHandler.GetPropList;

        public override void Initialize(GameInitializer gameInitializer)
        {
            AllInGameItems = new Dictionary<int, StoreItemSO>();
            AllInGameDrinks = new Dictionary<int, DrinkSO>();
            
            var allGameItems = Resources.LoadAll<StoreItemSO>("ScriptableObjects/").ToHashSet();
            foreach (var gItems in allGameItems)
                AllInGameItems.Add(gItems.ID, gItems);

            var allDrinkItems = Resources.LoadAll<DrinkSO>("ScriptableObjects/").ToHashSet();
            foreach (var dItem in allDrinkItems)
                AllInGameDrinks.Add(dItem.ID, dItem);
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

        public StoreItemSO FindAItemByID(int ID)
        {
            if(AllInGameItems.ContainsKey(ID))
                return AllInGameItems[ID];
            
            return null;
        }

        public DrinkSO FindDrinkByID(int ID)
        {
            if (AllInGameDrinks.ContainsKey(ID))
                return AllInGameDrinks[ID];

            return null;
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