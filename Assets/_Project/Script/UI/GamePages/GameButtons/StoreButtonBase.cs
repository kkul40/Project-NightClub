using System.Collections.Generic;
using System.Linq;
using Data;
using Disco_ScriptableObject;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GamePages.GameButtons
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