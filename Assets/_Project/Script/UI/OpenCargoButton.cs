using System.Collections.Generic;
using BuildingSystem.SO;
using Data;

namespace UI
{
    public class OpenCargoButton : UIButton
    {
        public override void OnClick()
        {
            StoreDataCarrier storeDataCarrier = new StoreDataCarrier();
            storeDataCarrier.inventory = DiscoData.Instance.inventory.Items;
            storeDataCarrier.EUISlot = DiscoData.eUISlot.Cargo;

            foreach (var key in storeDataCarrier.inventory.Keys)
            {
                storeDataCarrier.StoreItemSos.Add(key);
            }
            
            UIStoreManager.Instance.GetUiStoreInventoryView.GenerateInventory(storeDataCarrier);
        }
    }
}