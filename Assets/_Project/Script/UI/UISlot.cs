using BuildingSystemFolder;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UISlot : MonoBehaviour
    {
        public TextStyleSo textStyleSo;
        public ItemSo ItemSo;
        public Image image;
        public TextMeshProUGUI textMeshPro;

        private void OnValidate()
        {
            textMeshPro.font = textStyleSo.FontAsset;
            textMeshPro.fontSize = textStyleSo.FontSize;
        }

        private void Update()
        {
            textMeshPro.font = textStyleSo.FontAsset;
            textMeshPro.fontSize = textStyleSo.FontSize;
        }

        public void SetUp(ItemSo itemSo)
        {
            this.ItemSo = itemSo;
            image.sprite = ItemSo.icon;
            textMeshPro.text = ItemSo.name;
        }

        public void OnClick()
        {
            BuildingSystem.Instance.StartPlacement(ItemSo);
        }
    }
}