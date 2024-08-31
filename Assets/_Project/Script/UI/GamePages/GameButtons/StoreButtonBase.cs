using System.Collections.Generic;
using System.Linq;
using BuildingSystem.SO;
using Data;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class StoreButtonBase : UIButtonBase
    {
        [SerializeField] private UIStorePage storePage;
        [SerializeField] private string path;
        private List<StoreItemSO> _storeItemSos;

        private StoreDataCarrier _storeDataCarrier = new();

        protected override void Start()
        {
            // TODO Addressable Kullan!!
            _storeDataCarrier.StoreItemSos =
                Resources.LoadAll<StoreItemSO>("ScriptableObjects/StoreItems/" + path).ToList();
            _storeDataCarrier.EUISlot = eUISlot.ItemSlot;

            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        public override void OnClick()
        {
            storePage.GenerateInventory(this, _storeDataCarrier);
        }
    }
}