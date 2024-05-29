using System;
using BuildingSystem;
using BuildingSystem.SO;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class UISlot : MonoBehaviour
    {
        public TextStyleSo textStyleSo;
        public StoreItemSO storeItemSo;

        public Image image;
        [FormerlySerializedAs("textMeshPro")] public TextMeshProUGUI priceText;

        private void Update()
        {
            priceText.font = textStyleSo.FontAsset;
            priceText.fontSize = textStyleSo.FontSize;
        }

        private void OnValidate()
        {
            priceText.font = textStyleSo.FontAsset;
            priceText.fontSize = textStyleSo.FontSize;
        }

        public void Init(StoreItemSO storeItemSo)
        {
            this.storeItemSo = storeItemSo;
            image.sprite = storeItemSo.Icon;
            priceText.text = this.storeItemSo.Price.ToString();
        }

        public void OnClick()
        {
            BuildingManager.Instance.StartBuild(storeItemSo);
        }
    }
}