using System;
using System.Collections.Generic;
using BuildingSystem.SO;
using UnityEditor;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class Inventory
    {
        public Dictionary<StoreItemSO, int> Items;

        public event Action OnInventoryChanged;

        public Inventory(GameData gameData)
        {
            Items = new Dictionary<StoreItemSO, int>();
            
            foreach (var data in gameData.SavedInventoryDatas)
            {
                Items.Add(DiscoData.Instance.FindItemByID(data.InventoryItemID), data.Amount);
            }
        }

        public void AddItem(StoreItemSO storeItemSo)
        {
            if (Items.ContainsKey(storeItemSo))
            {
                Items[storeItemSo] += 1;
                OnInventoryChanged?.Invoke();
                return;
            }

            Items.Add(storeItemSo, 1);
            OnInventoryChanged?.Invoke();
        }

        public void RemoveItem(StoreItemSO storeItemSo)
        {
            if (Items.ContainsKey(storeItemSo))
            {
                if (Items[storeItemSo] - 1 == 0)
                {
                    Items.Remove(storeItemSo);
                    OnInventoryChanged?.Invoke();
                    return;
                }

                Items[storeItemSo] -= 1;
                OnInventoryChanged?.Invoke();
            }
        }

        private void WriteAllItems()
        {
            foreach (var item in Items) Debug.Log(item.Key + " - " + item.Value);
        }
    }
}