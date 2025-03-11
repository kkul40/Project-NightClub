using System;
using System.Collections.Generic;
using Disco_ScriptableObject;
using GameEvents;
using UnityEditor;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class Inventory : IDisposable
    {
        public int Balance;
        public Dictionary<StoreItemSO, int> Items;

        public Inventory(GameData gameData)
        {
            Balance = gameData.SavedInventoryData.Balance;
            Items = new Dictionary<StoreItemSO, int>();

            foreach (var data in gameData.SavedInventoryData.Items)
                Items.Add(DiscoData.Instance.FindAItemByID(data.Key), data.Value);
            
            KEvent_Inventory.TriggerMoneyChange(Balance);
            KEvent_Inventory.TriggerInventoryUpdate(Items);

            KEvent_Inventory.OnStoreItemAdded += AddItem;
            KEvent_Inventory.OnStoreItemRemoved += RemoveItem;
        }
        
        public void Dispose()
        {
            KEvent_Inventory.OnStoreItemAdded -= AddItem;
            KEvent_Inventory.OnStoreItemRemoved -= RemoveItem;
        }

        public void AddMoney(int moneyAmount)
        {
            Balance += moneyAmount;
            KEvent_Inventory.TriggerMoneyChange(Balance);
        }

        public bool RemoveMoney(int moneyAmount)
        {
            bool hasMoney = HasEnoughMoney(moneyAmount);

            if (hasMoney)
            {
                Balance -= moneyAmount;
                KEvent_Inventory.TriggerMoneyChange(Balance);
            }

            return hasMoney;
        }

        public bool HasEnoughMoney(int balanceCheckAmount)
        {
            if (Balance < balanceCheckAmount) return false;

            return true;
        }

        private void AddItem(StoreItemSO storeItemSo)
        {
            if (Items.ContainsKey(storeItemSo))
            {
                Items[storeItemSo] += 1;
                KEvent_Inventory.TriggerInventoryUpdate(Items);
                return;
            }

            Items.Add(storeItemSo, 1);
            KEvent_Inventory.TriggerInventoryUpdate(Items);
        }

        private void RemoveItem(StoreItemSO storeItemSo)
        {
            if (Items.ContainsKey(storeItemSo))
            {
                if (Items[storeItemSo] - 1 == 0)
                {
                    Items.Remove(storeItemSo);
                    KEvent_Inventory.TriggerInventoryUpdate(Items);

                    return;
                }

                Items[storeItemSo] -= 1;
                KEvent_Inventory.TriggerInventoryUpdate(Items);
            }
        }

        private void WriteAllItems()
        {
            foreach (var item in Items) Debug.Log(item.Key + " - " + item.Value);
        }
    }
}