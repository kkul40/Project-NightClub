using System;
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
            this.StoreItemSo = storeDataCarrier.ChosedStoreItemSo;
            image.sprite = StoreItemSo.Icon;
            priceText.text = StoreItemSo.Price.ToString();
        }

        public void OnClick()
        {
            BuildingManager.Instance.StartBuild(StoreItemSo);
        }
    }
}