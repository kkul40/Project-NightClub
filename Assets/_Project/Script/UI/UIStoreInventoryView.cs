using System;
using System.Collections.Generic;
using BuildingSystem.SO;
using Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

namespace UI
{
    public class UIStoreInventoryView : MonoBehaviour
    {
        [SerializeField] private GameObject UISlotPrefab;
        [SerializeField] private GameObject UICargoSlotPrefab;
        [SerializeField] private GameObject UIExtenderSlotPrefab;
        [SerializeField] private Transform SlotHolder;

        [SerializeField] private Transform ButtonHolder;

        [SerializeField] private Button NextBtn;
        [SerializeField] private Button PreviousBtn;

        private IUISlot[] _uiSlots;

        private StoreDataCarrier _storeDataCarrier = new();

        private const int listSize = 5;
        public int pointer;

        public int SetPointer
        {
            get => pointer;
            set
            {
                NextBtn.gameObject.SetActive(true);
                PreviousBtn.gameObject.SetActive(true);

                if (value == 0) pointer = 0;

                if (value >= 0 && value < _storeDataCarrier.StoreItemSos.Count) pointer = value;

                if (pointer <= 0) PreviousBtn.gameObject.SetActive(false);

                if (pointer + listSize >= _storeDataCarrier.StoreItemSos.Count) NextBtn.gameObject.SetActive(false);
            }
        }

        private void Awake()
        {
            NextBtn.onClick.AddListener(Next);
            PreviousBtn.onClick.AddListener(Previous);

            _uiSlots = new IUISlot[listSize];
        }

        public void GenerateInventory(StoreDataCarrier storeDataCarrier)
        {
            _storeDataCarrier = storeDataCarrier;
            SetPointer = 0;

            switch (storeDataCarrier.EUISlot)
            {
                case eUISlot.ItemSlot:
                    ButtonHolder.gameObject.SetActive(true);
                    break;
                case eUISlot.InventorySlot:
                case eUISlot.ExtentionSlot:
                    ButtonHolder.gameObject.SetActive(false);
                    break;
            }

            InstantiateSlots();
            Load();
        }

        private void InstantiateSlots()
        {
            _uiSlots = new IUISlot[listSize];

            for (var i = SlotHolder.childCount - 1; i >= 0; i--) Destroy(SlotHolder.GetChild(i).gameObject);

            for (var i = 0; i < _uiSlots.Length; i++)
            {
                GameObject temp = null;
                switch (_storeDataCarrier.EUISlot)
                {
                    case eUISlot.ItemSlot:
                        temp = Instantiate(UISlotPrefab, SlotHolder);
                        break;
                    case eUISlot.InventorySlot:
                        temp = Instantiate(UICargoSlotPrefab, SlotHolder);
                        break;
                    case eUISlot.ExtentionSlot:
                        temp = Instantiate(UIExtenderSlotPrefab, SlotHolder);
                        break;
                }

                _uiSlots[i] = temp.GetComponent<IUISlot>();
                _uiSlots[i].mGameobject.SetActive(false);
            }
        }

        private void Load()
        {
            for (var i = 0; i < listSize; i++)
            {
                if (i + pointer < _storeDataCarrier.StoreItemSos.Count)
                {
                    _uiSlots[i].mGameobject.SetActive(true);
                    _storeDataCarrier.ChosedStoreItemSo = _storeDataCarrier.StoreItemSos[i + pointer];
                    _uiSlots[i].Init(_storeDataCarrier);
                    continue;
                }

                _uiSlots[i].mGameobject.SetActive(false);
            }
        }

        public void Next()
        {
            SetPointer += listSize;
            Load();
        }

        public void Previous()
        {
            SetPointer -= listSize;
            Load();
        }
    }
}