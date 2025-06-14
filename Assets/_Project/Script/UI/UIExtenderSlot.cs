using System;
using _Initializer;
using Disco_ScriptableObject;
using DiscoSystem;
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

            _extendItemSo = StoreItemSo as ExtendItemSo;
            text.text = _extendItemSo.ExtendX + " x " + _extendItemSo.ExtendY;
        }

        public void OnClick()
        {
            for (var i = 0; i < _extendItemSo.ExtendX; i++)
                ServiceLocator.Get<MapGeneratorSystem>().ExpendX();

            for (var i = 0; i < _extendItemSo.ExtendY; i++)
                ServiceLocator.Get<MapGeneratorSystem>().ExpendY();
        }
    }
}