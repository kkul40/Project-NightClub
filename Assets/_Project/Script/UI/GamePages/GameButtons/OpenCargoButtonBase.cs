using System.Linq;
using Data;
using UnityEngine;

namespace UI.GamePages.GameButtons
{
    public class OpenCargoButtonBase : UIButtonBase
    {
        [SerializeField] private UIStorePage storePage;
        private StoreDataCarrier _storeDataCarrier = new();

        protected override void Start()
        {
            UpdateInventory();
        }

        private void OnEnable()
        {
            DiscoData.Instance.inventory.OnInventoryChanged += UpdateInventory;
        }

        private void OnDisable()
        {
            DiscoData.Instance.inventory.OnInventoryChanged -= UpdateInventory;
        }

        private void UpdateInventory()
        {
            _storeDataCarrier.EUISlot = eUISlot.InventorySlot;
            _storeDataCarrier.inventory = DiscoData.Instance.inventory.Items;
            _storeDataCarrier.StoreItemSos = DiscoData.Instance.inventory.Items.Keys.ToList();
        }

        public override void OnClick()
        {
            storePage.GenerateInventory(this, _storeDataCarrier);
        }
    }
}