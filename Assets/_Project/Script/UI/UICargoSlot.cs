using Data;
using Disco_Building;
using Disco_ScriptableObject;
using TMPro;
using UI.GamePages.GameButtons;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UICargoSlot : MonoBehaviour, IUISlot
    {
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
            FindObjectOfType<OpenCargoButtonBase>().OnClick();
            BuildingManager.Instance.StopBuild();
        }

        public void OnClick()
        {
            if (DiscoData.Instance.inventory.Items.ContainsKey(StoreItemSo))
                BuildingManager.Instance.StartBuild(StoreItemSo, CallBackOnPlace:() => Action);
        }

        private void Action()
        {
            DiscoData.Instance.inventory.RemoveItem(StoreItemSo);
            Refresh();
        }
    }
}