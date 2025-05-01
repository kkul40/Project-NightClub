using System;
using System.Collections;
using System.Collections.Generic;
using Disco_ScriptableObject;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Data
{
    [CreateAssetMenu(fileName = "Game Bundle", menuName = "Persist/GameBundle")]
    public class GameBundle : ScriptableObject, IAsyncInitializable
    {
        private static GameBundle instance;
        public static GameBundle Instance
        {
            get
            {
                if (instance == null)
                    instance = Resources.Load<GameBundle>("Persistent/Game Bundle");

                return instance;
            }
        }

        [SerializeField] private AssetLabelReference _storeItemReference;
        [SerializeField] private AssetLabelReference _drinkDataReference;
        
        private AsyncOperationHandle<IList<StoreItemSO>> _storeItemHandle;
        private AsyncOperationHandle<IList<DrinkSO>> _drinkItemHandle;

        public Dictionary<int, StoreItemSO> AllInGameItems { get; private set; } = new();
        public Dictionary<int, DrinkSO> AllInGameDrinks { get; private set; } = new();

        public IEnumerator InitializeAsync()
        {
            AllInGameItems = new Dictionary<int, StoreItemSO>();
            AllInGameDrinks = new Dictionary<int, DrinkSO>();
            
            // Load Store Items
            var itemHandle = Addressables.LoadAssetsAsync<StoreItemSO>(_storeItemReference.labelString, null);
            yield return itemHandle;

            if (itemHandle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var item in itemHandle.Result)
                {
                    if (!AllInGameItems.ContainsKey(item.ID))
                        AllInGameItems.Add(item.ID, item);
                    else
                        Debug.LogWarning($"Duplicate StoreItemSO ID detected: {item.ID}");
                }
            }
            else
            {
                Debug.LogError("Failed to load store items.");
            }

            // Load Drink Data
            var drinkHandle = Addressables.LoadAssetsAsync<DrinkSO>(_drinkDataReference.labelString, null);
            yield return drinkHandle;

            if (drinkHandle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var drink in drinkHandle.Result)
                {
                    if (!AllInGameDrinks.ContainsKey(drink.ID))
                        AllInGameDrinks.Add(drink.ID, drink);
                    else
                        Debug.LogWarning($"Duplicate StoreItemSO ID detected: {drink.ID}");
                }
            }
            else
            {
                Debug.LogError("Failed to load drink data.");
            }
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
        
        private void OnDisable()
        {
            if(_storeItemHandle.IsValid())
                _storeItemHandle.Release();
            
            if(_drinkItemHandle.IsValid())
                _drinkItemHandle.Release();
        }
    }
}
