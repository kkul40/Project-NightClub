using Disco_Building;
using Disco_ScriptableObject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UISlot : MonoBehaviour, IUISlot
    {
        public GameObject mGameobject { get; private set; }
        public StoreItemSO StoreItemSo { get; private set; }

        public Image image;
        public TextMeshProUGUI priceText;

        private void Awake()
        {
            mGameobject = transform.gameObject;
        }

        // private void Update()
        // {
        //     priceText.font = textStyleSo.FontAsset;
        //     priceText.fontSize = textStyleSo.FontSize;
        // }
        //
        // private void OnValidate()
        // {
        //     priceText.font = textStyleSo.FontAsset;
        //     priceText.fontSize = textStyleSo.FontSize;
        // }

        public void Init(StoreDataCarrier storeDataCarrier)
        {
            StoreItemSo = storeDataCarrier.ChosedStoreItemSo;
            image.sprite = StoreItemSo.Icon;

            if (StoreItemSo.Price == 0)
                priceText.text = "Free";
            else
                priceText.text = StoreItemSo.Price.ToString();
        }

        public void OnClick()
        {
            BuildingManager.Instance.StartBuild(StoreItemSo);
        }
    }
}