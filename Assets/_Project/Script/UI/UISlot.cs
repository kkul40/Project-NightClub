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
        public TextMeshProUGUI textMeshPro;

        private void Update()
        {
            textMeshPro.font = textStyleSo.FontAsset;
            textMeshPro.fontSize = textStyleSo.FontSize;
        }

        private void OnValidate()
        {
            textMeshPro.font = textStyleSo.FontAsset;
            textMeshPro.fontSize = textStyleSo.FontSize;
        }

        public void Init(StoreItemSO placableItemDataSo)
        {
            this.storeItemSo = placableItemDataSo;
            image.sprite = this.storeItemSo.Icon;
            textMeshPro.text = this.storeItemSo.name;
        }

        public void OnClick()
        {
            BuildingManager.Instance.StartBuild(storeItemSo);
        }
    }
}