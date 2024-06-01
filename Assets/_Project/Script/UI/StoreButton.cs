using System.Collections.Generic;
using System.Linq;
using BuildingSystem.SO;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StoreButton : UIButton
    {
        [SerializeField] private string path;
        private List<StoreItemSO> _storeItemSos;

        private void Awake()
        {
            _storeItemSos = Resources.LoadAll<StoreItemSO>("ScriptableObjects/StoreItems/" + path).ToList();
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        public override void OnClick()
        {
            UIStoreManager.Instance.GenerateInventory(_storeItemSos);
        }
    }

    public class StoreOKButton : UIButton
    {
        public override void OnClick()
        {
            
        }
    }
}