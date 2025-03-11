using System;
using System.Collections.Generic;
using Disco_ScriptableObject;
using ScriptableObjects;

namespace GameEvents
{
    public class KEvent_GameAssetBundle
    {
        public static Action<List<StoreItemSO>> OnGameStoreItemsLoaded;
        public static Action<List<DrinkSO>> OnGameDrinkDataLoaded;

        public static void TriggerStoreItemsLoad(List<StoreItemSO> allGameItems)
        {
            OnGameStoreItemsLoaded?.Invoke(allGameItems);
        }
        
        public static void TriggerDrinkDataLoad(List<DrinkSO> alldrinkData)
        {
            OnGameDrinkDataLoaded?.Invoke(alldrinkData);
        }
    }
}