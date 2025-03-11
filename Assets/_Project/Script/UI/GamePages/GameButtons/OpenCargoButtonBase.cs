// using System.Collections.Generic;
// using System.Linq;
// using Data;
// using Disco_ScriptableObject;
// using GameEvents;
// using UnityEngine;
//
// namespace UI.GamePages.GameButtons
// {
//     public class OpenCargoButtonBase : UIButtonBase
//     {
//         [SerializeField] private UIStorePage storePage;
//         private StoreDataCarrier _storeDataCarrier = new();
//
//         private void OnEnable()
//         {
//             KEvent_Inventory.OnInventoryUpdated += UpdateInventory;
//         }
//
//         private void OnDisable()
//         {
//             KEvent_Inventory.OnInventoryUpdated -= UpdateInventory;
//         }
//
//         private void UpdateInventory(Dictionary<StoreItemSO, int> inventoryItems)
//         {
//             _storeDataCarrier.EUISlot = eUISlot.InventorySlot;
//             _storeDataCarrier.inventory = inventoryItems;
//             _storeDataCarrier.StoreItemSos = inventoryItems.Keys.ToList();
//         }
//
//         public override void OnClick()
//         {
//             storePage.GenerateInventory(this, _storeDataCarrier);
//         }
//     }
// }