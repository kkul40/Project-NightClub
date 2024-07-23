using System.Linq;
using BuildingSystem.SO;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class OpenMapExtentionButton : UIButton
    {
        public override void OnClick()
        {
            var storeDataCarrier = new StoreDataCarrier();
            storeDataCarrier.EUISlot = DiscoData.eUISlot.ExtentionSlot;

            storeDataCarrier.StoreItemSos = Resources.LoadAll<StoreItemSO>("ScriptableObjects/Extender").ToList();

            UIStoreManager.Instance.GetUiStoreInventoryView.GenerateInventory(storeDataCarrier);
        }
    }
}