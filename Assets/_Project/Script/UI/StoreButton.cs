﻿using System.Collections.Generic;
using System.Linq;
using BuildingSystem.SO;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StoreButton : UIButton
    {
        [SerializeField] private string path;
        private List<StoreItemSO> _storeItemSos;

        private StoreDataCarrier _storeDataCarrier = new();

        private void Awake()
        {
            _storeDataCarrier.StoreItemSos =
                Resources.LoadAll<StoreItemSO>("ScriptableObjects/StoreItems/" + path).ToList();
            _storeDataCarrier.EUISlot = DiscoData.eUISlot.ItemSlot;

            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        public override void OnClick()
        {
            UIStoreManager.Instance.GetUiStoreInventoryView.GenerateInventory(_storeDataCarrier);
        }
    }
}