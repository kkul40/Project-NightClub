using System;
using System.Building_System.GameEvents;
using System.Collections.Generic;
using System.Linq;
using Disco_ScriptableObject;
using PropBehaviours;
using SaveAndLoad;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Data
{
    [DisallowMultipleComponent]
    public class DiscoData : MonoBehaviour
    {
        public static DiscoData Instance;
        
        [SerializeField] private AssetLabelReference _storeItemReference;
        [SerializeField] private AssetLabelReference _drinkDataReference;

        private AsyncOperationHandle<IList<StoreItemSO>> _storeItemHandle;
        private AsyncOperationHandle<IList<DrinkSO>> _drinkItemHandle;
        
        // Verileri Once Data ya yaz daha sonra Modeli datadaki veriye gore guncelle!
        public MapData MapData;
        public Inventory inventory;
        public Dictionary<int, StoreItemSO> AllInGameItems { get; private set; } = new();
        public Dictionary<int, DrinkSO> AllInGameDrinks { get; private set; }
        
        //            instanceID   StoreID created-obj   Pos      Rot
        public Dictionary<int, Tuple<int, Transform, Vector3, Quaternion>> PlacedItems;

        public void Initialize(GameData gameData)
        {
            Instance = this;
            AllInGameItems = new Dictionary<int, StoreItemSO>();
            AllInGameDrinks = new Dictionary<int, DrinkSO>();
           
            //
            PlacedItems = new Dictionary<int, Tuple<int, Transform, Vector3, Quaternion>>();
            //
            
            Addressables.LoadAssetsAsync<StoreItemSO>(_storeItemReference.labelString).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    foreach (var item in handle.Result)
                    {
                        AllInGameItems.Add(item.ID, item);
                    }

                    KEvent_GameAssetBundle.TriggerStoreItemsLoad(handle.Result.ToList());
                }
            };

            Addressables.LoadAssetsAsync<DrinkSO>(_drinkDataReference.labelString).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    foreach (var drink in handle.Result)
                    {
                        AllInGameDrinks.Add(drink.ID, drink);
                    }
                    
                    KEvent_GameAssetBundle.TriggerDrinkDataLoad(handle.Result.ToList());
                }
            };

            // _storeItemHandle = Addressables.LoadAssetsAsync<StoreItemSO>(_storeItemReference.labelString, addressable =>
            // {
            //     AllInGameItems.Add(addressable.ID, addressable);
            // });
            // _storeItemHandle.Completed += handle => KEvent_GameAssetBundle.TriggerStoreItemsLoad(AllInGameItems.Values.ToList());
            //
            // _drinkItemHandle = Addressables.LoadAssetsAsync<DrinkSO>(_drinkDataReference.labelString, addressables =>
            // {
            //     AllInGameDrinks.Add(addressables.ID, addressables);
            // });
            // _drinkItemHandle.Completed += handle => KEvent_GameAssetBundle.TriggerDrinkDataLoad(AllInGameDrinks.Values.ToList());
            
             
            MapData = new MapData(gameData);
            inventory = new Inventory(gameData);
        }


        private void OnDisable()
        {
            if(_storeItemHandle.IsValid())
                _storeItemHandle.Release();
            
            if(_drinkItemHandle.IsValid())
                _drinkItemHandle.Release();
            
            inventory.Dispose();
            MapData.Dispose();
        }

        // public void Load()
        // {
        //     var allGameItems = Resources.LoadAll<StoreItemSO>("ScriptableObjects/").ToList();
        //     foreach (var gItems in allGameItems)
        //         AllInGameItems.Add(gItems.ID, gItems);
        //
        //     var allDrinkItems = Resources.LoadAll<DrinkSO>("ScriptableObjects/").ToList();
        //     foreach (var dItem in allDrinkItems)
        //         AllInGameDrinks.Add(dItem.ID, dItem);
        //     
        //     KEvent_GameAssetBundle.TriggerStoreItemsLoad(allGameItems);
        // }


        public void SaveData(ref GameData gameData)
        {
            gameData.SavedInventoryData = new GameDataExtension.InventorySaveData();

            foreach (var pair in inventory.Items)
                gameData.SavedInventoryData.Items.Add(pair.Key.ID, pair.Value);
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
            List<T> output = new List<T>();
            foreach (var value in PlacedItems.Values)
            {
                if (value.Item2.TryGetComponent(out IPropUnit unit))
                {
                    if (unit is T t)
                    {
                        output.Add(t);
                    }
                }
            }

            return output;
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
        public const int PathFinderGridSize = 5;
        public const int DoorHeight = 2;
        public const int FloorLayerID = 7;
        public const int WalllayerID = 8;
    }
}