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

        private StoreDataCarrier _storeDataCarrier = new();

        protected override void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void LoadAssets()
        {
            if (_storeDataCarrier.StoreItemSos.Count != 0) return;
            
            // TODO Addressable Kullan!!
            _storeDataCarrier.StoreItemSos = Resources.LoadAll<StoreItemSO>("ScriptableObjects/StoreItems/" + path).ToList();
            _storeDataCarrier.EUISlot = eUISlot.ItemSlot;
            
            if(_storeDataCarrier.StoreItemSos.Count != 0)
                _storeDataCarrier.StoreItemSos.Sort((a, b) => string.Compare(a.name, b.name));
        }
        

        public override void OnClick()
        {
            LoadAssets();
            storePage.GenerateInventory(this, _storeDataCarrier);
        }
    }
}