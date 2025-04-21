using System;
using System.Building_System.GameEvents;
using System.Collections.Generic;
using Disco_ScriptableObject;
using DiscoSystem;
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
            
            
            GameEvent.Subscribe<Event_MoneyAdded>(AddMoney);
            GameEvent.Subscribe<Event_RemoveMoney>(RemoveMoney);
            
            GameEvent.Subscribe<Event_AddItem>(AddItem);
            GameEvent.Subscribe<Event_RemoveItem>(RemoveItem);
            
            GameEvent.Trigger(new Event_BalanceUpdated(Balance));
            GameEvent.Trigger(new Event_InventoryItemsUpdated(Items));
        }
        
        private void AddMoney(Event_MoneyAdded moneyEvent)
        {
            Balance += moneyEvent.Amount;
            GameEvent.Trigger(new Event_BalanceUpdated(Balance));
            GameEvent.Trigger(new Event_Sfx(SoundFXType.MoneyAdd));
        }

        private void RemoveMoney(Event_RemoveMoney removeMoneyEventRemove)
        {
            bool hasMoney = HasEnoughMoney(removeMoneyEventRemove.Amount);

            if (hasMoney)
            {
                Balance -= removeMoneyEventRemove.Amount;
                GameEvent.Trigger(new Event_BalanceUpdated(Balance));
                
                if(removeMoneyEventRemove.PlaySfx)
                    GameEvent.Trigger(new Event_Sfx(SoundFXType.MoneyRemove));
            }
        }

        public bool HasEnoughMoney(int balanceCheckAmount)
        {
            if (Balance < balanceCheckAmount) return false;
            return true;
        }

        private void AddItem(Event_AddItem itemEvent)
        {
            if (Items.ContainsKey(itemEvent.Item))
            {
                Items[itemEvent.Item] += 1;
                GameEvent.Trigger(new Event_InventoryItemsUpdated(Items));
                return;
            }

            Items.Add(itemEvent.Item, 1);
            GameEvent.Trigger(new Event_InventoryItemsUpdated(Items));
        }

        private void RemoveItem(Event_RemoveItem itemEvent)
        {
            if (Items.ContainsKey(itemEvent.Item))
            {
                if (Items[itemEvent.Item] - 1 == 0)
                {
                    Items.Remove(itemEvent.Item);
                    GameEvent.Trigger(new Event_InventoryItemsUpdated(Items));

                    return;
                }

                Items[itemEvent.Item] -= 1;
                GameEvent.Trigger(new Event_InventoryItemsUpdated(Items));
            }
        }

        private void WriteAllItems()
        {
            foreach (var item in Items) Debug.Log(item.Key + " - " + item.Value);
        }

        public void Dispose()
        {
        }
    }
}