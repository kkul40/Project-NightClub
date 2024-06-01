using System;
using System.Collections.Generic;
using BuildingSystem.SO;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIStoreInventory : MonoBehaviour
    {
        [SerializeField] private GameObject UISlotPrefab;
        [SerializeField] private Transform SlotHolder;
        [SerializeField] private List<StoreItemSO> storeItemSos;

        [SerializeField] private Button NextBtn;
        [SerializeField] private Button PreviousBtn;

        private UISlot[] _uiSlots = new UISlot[5];

        public int pointer;
        
        public int SetPointer
        {
            get => pointer;
            set
            {
                NextBtn.gameObject.SetActive(true);
                PreviousBtn.gameObject.SetActive(true);

                if (value == 0)
                {
                    pointer = 0;
                }
                if (value >= 0 && value < storeItemSos.Count)
                {
                    pointer = value;
                }

                if (pointer <= 0)
                {
                    PreviousBtn.gameObject.SetActive(false);
                }
                
                if (pointer + _uiSlots.Length >= storeItemSos.Count)
                {
                    NextBtn.gameObject.SetActive(false);
                }
                
            }
        }
        
        private void Awake()
        {
            NextBtn.onClick.AddListener(Next);
            PreviousBtn.onClick.AddListener(Previous);
        }

        private void Start()
        {
            for (int i = 0; i < _uiSlots.Length; i++)
            {
                var temp = Instantiate(UISlotPrefab, SlotHolder);
                _uiSlots[i] = temp.GetComponent<UISlot>();
                _uiSlots[i].gameObject.SetActive(false);
            }
        }

        public void GenerateInventory(List<StoreItemSO> storeItemSos)
        {
            this.storeItemSos = new List<StoreItemSO>();

            if (storeItemSos.Count > 0)
            {
                this.storeItemSos = storeItemSos;
            }
            
            SetPointer = 0;
            Load();
        }

        [ContextMenu("Next")]
        public void Next()
        {
            SetPointer += _uiSlots.Length;
            Load();
        }
        
        [ContextMenu("Previous")]
        public void Previous()
        {
            SetPointer -= _uiSlots.Length;
            Load();
        }

        private void Load()
        {
            for (int i = 0; i < _uiSlots.Length; i++)
            {
                if (i + pointer < storeItemSos.Count)
                {
                    _uiSlots[i].gameObject.SetActive(true);
                    _uiSlots[i].Init(storeItemSos[i + pointer]);
                    continue;
                }
                    
                _uiSlots[i].gameObject.SetActive(false);
            }
        }
    }
}