using System;
using System.Collections.Generic;
using System.Linq;
using BuildingSystem.SO;
using Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class OpenCargoButton : UIButton
    {
        [SerializeField] private UIStorePage storePage;
        private StoreDataCarrier _storeDataCarrier= new StoreDataCarrier();

        private void OnEnable()
        {
            DiscoData.Instance.inventory.OnInventoryChanged += UpdateInventory;
            UpdateInventory();
        }

        private void OnDisable()
        {
            DiscoData.Instance.inventory.OnInventoryChanged -= UpdateInventory;
        }

        private void UpdateInventory()
        {
            _storeDataCarrier.EUISlot = DiscoData.eUISlot.InventorySlot;
            _storeDataCarrier.inventory = DiscoData.Instance.inventory.Items;
            _storeDataCarrier.StoreItemSos = DiscoData.Instance.inventory.Items.Keys.ToList();
        }

        public override void OnClick()
        {
            storePage.GenerateInventory(this, _storeDataCarrier);
        }
    }
}