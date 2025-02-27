using System;
using System.Collections.Generic;
using System.Linq;
using Disco_ScriptableObject;
using DiscoSystem;
using ExtensionMethods;
using PropBehaviours;
using ScriptableObjects;
using UnityEngine;

namespace Data
{
    [DisallowMultipleComponent]
    public class DiscoData : Singleton<DiscoData>, ISaveLoad
    {
        // Verileri Once Data ya yaz daha sonra Modeli datadaki veriye gore guncelle!
        public MapData MapData;
        public Inventory inventory = new();
        public Dictionary<int ,StoreItemSO> AllInGameItems { get; private set; }
        public Dictionary<int, DrinkSO> AllInGameDrinks { get; private set; }
        
        //            instanceID   StoreID created-obj   Pos      Rot
        public Dictionary<int, Tuple<int, Transform, Vector3, Quaternion>> PlacedItems;

        public void Initialize(GameData gameData)
        {
            LoadData(gameData);
            
            AllInGameItems = new Dictionary<int, StoreItemSO>();
            AllInGameDrinks = new Dictionary<int, DrinkSO>();
            
            //
            PlacedItems = new Dictionary<int, Tuple<int, Transform, Vector3, Quaternion>>();
            //
            
            var allGameItems = Resources.LoadAll<StoreItemSO>("ScriptableObjects/").ToHashSet();
            foreach (var gItems in allGameItems)
                AllInGameItems.Add(gItems.ID, gItems);

            var allDrinkItems = Resources.LoadAll<DrinkSO>("ScriptableObjects/").ToHashSet();
            foreach (var dItem in allDrinkItems)
                AllInGameDrinks.Add(dItem.ID, dItem);
        }

        public SavePriority Priority { get; } = SavePriority.VeryHigh;

        public void LoadData(GameData gameData)
        {
            MapData = new MapData(gameData);
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
        
        public List<T> GetPlacedPropsByType<T>()
        {
            // TODO Fill This
            return null;
        }

        public List<IPropUnit> GetPropList()
        {
            List<IPropUnit> output = new List<IPropUnit>();
            foreach (var value in PlacedItems.Values)
            {
                if (value.Item2.TryGetComponent(out IPropUnit unit))
                {
                    output.Add(unit);
                }
            }

            return output;
        }
        // {
        //     // TODO Fill This
        //     return null;
        // }
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
        public const int DoorHeight = 2;
        public const int FloorLayerID = 7;
        public const int WalllayerID = 8;
    }
}