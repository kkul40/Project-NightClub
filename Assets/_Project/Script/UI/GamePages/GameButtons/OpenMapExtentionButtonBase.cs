// using System.Linq;
// using Data;
// using Disco_ScriptableObject;
// using UnityEngine;
//
// namespace UI.GamePages.GameButtons
// {
//     public class OpenMapExtentionButtonBase : UIButtonBase
//     {
//         [SerializeField] private UIStorePage storePage;
//         private StoreDataCarrier _storeDataCarrier = new();
//
//         protected override void Start()
//         {
//             _storeDataCarrier.EUISlot = eUISlot.ExtentionSlot;
//             _storeDataCarrier.StoreItemSos = Resources.LoadAll<StoreItemSO>("ScriptableObjects/Extender").ToList();
//         }
//
//         public override void OnClick()
//         {
//             storePage.GenerateInventory(this, _storeDataCarrier);
//         }
//     }
// }