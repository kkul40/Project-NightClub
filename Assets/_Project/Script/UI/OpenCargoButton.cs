using System.Collections.Generic;
using System.Linq;
using BuildingSystem.SO;
using Data;

namespace UI
{
    public class OpenCargoButton : UIButton
    {
        public override void OnClick()
        {
            var storeDataCarrier = new StoreDataCarrier();
            storeDataCarrier.EUISlot = DiscoData.eUISlot.InventorySlot;
            storeDataCarrier.StoreItemSos = DiscoData.Instance.inventory.Items.Keys.ToList();

            UIStoreManager.Instance.GetUiStoreInventoryView.GenerateInventory(storeDataCarrier);
        }
    }
}