using BuildingSystem;
using BuildingSystem.SO;
using Data;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class UICargoSlot : MonoBehaviour, IUISlot
    {
        public TextStyleSo textStyleSo;
        public GameObject mGameobject { get; private set; }
        public StoreItemSO StoreItemSo { get; private set; }

        public Image image;
        public TextMeshProUGUI amountText;

        private void Awake()
        {
            mGameobject = transform.gameObject;
        }

        private void OnEnable()
        {
            // DiscoData.Instance.inventory.OnInventoryChanged += Update;
        }

        private void OnDisable()
        {
            // DiscoData.Instance.inventory.OnInventoryChanged -= Update;
        }

        public void Init(StoreDataCarrier storeDataCarrier)
        {
            StoreItemSo = storeDataCarrier.ChosedStoreItemSo;
            image.sprite = StoreItemSo.Icon;
            amountText.text = storeDataCarrier.inventory[StoreItemSo].ToString();
        }

        private void Refresh()
        {
            if (DiscoData.Instance.inventory.Items.ContainsKey(StoreItemSo))
            {
                amountText.text = DiscoData.Instance.inventory.Items[StoreItemSo].ToString();
                return;
            }

            amountText.text = "0";
            FindObjectOfType<OpenCargoButton>().OnClick();
            BuildingManager.Instance.StopBuild();
        }

        public void OnClick()
        {
            if (DiscoData.Instance.inventory.Items.ContainsKey(StoreItemSo))
                BuildingManager.Instance.StartBuild(StoreItemSo, () => Action);
        }

        private void Action()
        {
            DiscoData.Instance.inventory.RemoveItem(StoreItemSo);
            Refresh();
        }
    }
}