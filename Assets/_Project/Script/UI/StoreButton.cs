using System.Collections.Generic;
using System.Linq;
using BuildingSystem.SO;
using Data;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class StoreButton : UIButton
    {
        [SerializeField] private UIStoreMenu storeMenu;
        [SerializeField] private string path;
        private List<StoreItemSO> _storeItemSos;

        private StoreDataCarrier _storeDataCarrier = new();

        private void Awake()
        {
            // TODO Addressable Kullan!!
            _storeDataCarrier.StoreItemSos =
                Resources.LoadAll<StoreItemSO>("ScriptableObjects/StoreItems/" + path).ToList();
            _storeDataCarrier.EUISlot = DiscoData.eUISlot.ItemSlot;

            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        public override void OnClick()
        {
            storeMenu.GenerateInventory(this, _storeDataCarrier);
        }
    }
}