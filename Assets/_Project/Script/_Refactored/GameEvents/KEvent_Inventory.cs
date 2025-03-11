using System;
using System.Collections.Generic;
using Disco_ScriptableObject;

namespace GameEvents
{
    public static class KEvent_Inventory
    {
        public static event Action<int> OnMoneyChanged;
        public static event Action<StoreItemSO> OnStoreItemAdded;
        public static event Action<StoreItemSO> OnStoreItemRemoved;
        public static event Action<Dictionary<StoreItemSO, int>> OnInventoryUpdated; 
        public static void TriggerMoneyChange(int balance)
        {
            OnMoneyChanged?.Invoke(balance);
        }

        public static void TriggerStoreItemAdd(StoreItemSO storeItemSo)
        {
            OnStoreItemAdded?.Invoke(storeItemSo);
        }
        
        public static void TriggerStoreItemRemove(StoreItemSO storeItemSo)
        {
            OnStoreItemRemoved?.Invoke(storeItemSo);
        }

        public static void TriggerInventoryUpdate(Dictionary<StoreItemSO, int> items)
        {
            OnInventoryUpdated?.Invoke(items);
        }
    }
}