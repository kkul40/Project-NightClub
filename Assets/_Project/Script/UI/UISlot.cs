using System;
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

        [FormerlySerializedAs("placableItemSo")]
        public PlacableItemDataSo placableItemDataSo;

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

        public void Init(PlacableItemDataSo placableItemDataSo)
        {
            this.placableItemDataSo = placableItemDataSo;
            image.sprite = this.placableItemDataSo.icon;
            textMeshPro.text = this.placableItemDataSo.name;
        }

        public void OnClick()
        {
            BuildingManager.Instance.StartPlacement(placableItemDataSo);
        }
    }
}