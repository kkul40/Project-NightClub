using BuildingSystemFolder;
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
        public PlacablePropSo PlacablePropSo;
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

        public void SetUp(PlacablePropSo placablePropSo)
        {
            this.PlacablePropSo = placablePropSo;
            image.sprite = PlacablePropSo.icon;
            textMeshPro.text = PlacablePropSo.name;
        }

        public void OnClick()
        {
            BuildingSystem.Instance.StartPlacement(PlacablePropSo);
        }
    }
}