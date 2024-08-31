using System;
using System.Linq;
using BuildingSystem.SO;
using Data;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class OpenMapExtentionButtonBase : UIButtonBase
    {
        [SerializeField] private UIStorePage storePage;
        private StoreDataCarrier _storeDataCarrier = new();

        protected override void Start()
        {
            _storeDataCarrier.EUISlot = eUISlot.ExtentionSlot;
            _storeDataCarrier.StoreItemSos = Resources.LoadAll<StoreItemSO>("ScriptableObjects/Extender").ToList();
        }

        public override void OnClick()
        {
            storePage.GenerateInventory(this, _storeDataCarrier);
        }
    }
}