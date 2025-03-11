using System.Collections.Generic;
using Disco_ScriptableObject;
using Framework.Mvcs.View;
using GameEvents;
using UI.GamePages.GameButtons;
using UnityEngine;
using UnityEngine.UI;

namespace System.Building_System.View
{
    public class BuildingView : BaseView
    {
        [SerializeField] private Transform _contentParent;
        [SerializeField] private Transform _buttonsParent;
        
        public Action<StoreItemSO> OnSlotItemClicked;
        public Action<StoreItemSO, int> OnStorageItemClicked;

        private List<UI_StoreItemSlot> _slots;

        private List<UI_CargoItemSlot> _cargoSlots;

        public GameObject SlotPrefab;
    
        public void GenerateItems(List<StoreItemSO> storeItemSos)
        {
            _slots = new List<UI_StoreItemSlot>();
            _cargoSlots = new List<UI_CargoItemSlot>();
            
            foreach (var storeItem in storeItemSos)
            {
                var slot = Instantiate(SlotPrefab, _contentParent,false);
                UI_StoreItemSlot slotClass = slot.GetComponent<UI_StoreItemSlot>();
                slotClass.Initialize(storeItem);

                _slots.Add(slotClass);
            
                Button button = slot.GetComponent<Button>();

                if (storeItem is ExtendItemSo extendItemSo)
                    button.onClick.AddListener((() => ExtensionSlotClicked(button, extendItemSo)));
                else
                    button.onClick.AddListener(() => ItemSlotClicked(button, storeItem));
            }

            for (int i = 0; i < _buttonsParent.childCount; i++)
            {
                StoreButtonBase slotClass = _buttonsParent.GetChild(i).GetComponent<StoreButtonBase>();

                if (slotClass != null)
                {
                    if (_buttonsParent.GetChild(i).TryGetComponent(out Button button))
                        button.onClick.AddListener( () => SelectCategory(slotClass.ItemType));
                }
            }
            
            
            
            SelectCategory(StoreItemTypes.Bar);
        }

        public void ItemSlotClicked(Button button, StoreItemSO storeItemSo)
        {
            OnSlotItemClicked?.Invoke(storeItemSo);
        }

        public void ExtensionSlotClicked(Button button, ExtendItemSo extendItemSo)
        {
            KEvent_Map.TriggerExtendMapSize(extendItemSo.ExtendX, extendItemSo.ExtendY);
        }

        public void StorageSlotClicked(Button button, StoreItemSO storeItemSo, int amount)
        {
            OnStorageItemClicked?.Invoke(storeItemSo, amount);
        }

        public void ToggleView(bool toggle)
        {
            
            gameObject.SetActive(toggle);
        }

        public void SelectCategory(StoreItemTypes storeItemType)
        {
            foreach (var slot in _slots)
            {
                if (slot.StoreItemSo.ItemType == storeItemType)
                    slot.gameObject.SetActive(true);
                else
                    slot.gameObject.SetActive(false);
            }
        }
    }
}