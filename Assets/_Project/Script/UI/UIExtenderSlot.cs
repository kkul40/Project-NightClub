using System;
using BuildingSystem.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIExtenderSlot : MonoBehaviour, IUISlot
    {
        public GameObject mGameobject { get; private set; }
        public StoreItemSO StoreItemSo { get; private set; }

        private ExtendItemSo _extendItemSo;
        public Image image;
        public TextMeshProUGUI text;

        private void Awake()
        {
            mGameobject = transform.gameObject;
        }

        public void Init(StoreDataCarrier storeDataCarrier)
        {
            StoreItemSo = storeDataCarrier.ChosedStoreItemSo;
            image.sprite = StoreItemSo.Icon;
            image.enabled = false;
            
            _extendItemSo = StoreItemSo as ExtendItemSo;
            text.text = _extendItemSo.ExtendX + " x " + _extendItemSo.ExtendY;
        }

        public void OnClick()
        {
            for (var i = 0; i < _extendItemSo.ExtendX; i++)
                MapGeneratorSystem.Instance.ExpendMapOnX();

            for (var i = 0; i < _extendItemSo.ExtendY; i++)
                MapGeneratorSystem.Instance.ExpendMapOnY();
        }
    }
}