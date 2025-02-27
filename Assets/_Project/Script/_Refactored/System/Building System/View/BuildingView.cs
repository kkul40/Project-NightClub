using System.Collections.Generic;
using Disco_ScriptableObject;
using Framework.Mvcs.View;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace System.Building_System.View
{
    public class BuildingView : BaseView
    {
        [SerializeField] private Transform _contentParent;
        public Action<StoreItemSO> OnSlotItemClicked;

        public GameObject SlotPrefab;
    
        public void InstantiateItems(List<StoreItemSO> storeItemSos)
        {
            foreach (var storeItem in storeItemSos)
            {
                var slot = Instantiate(SlotPrefab);
                slot.transform.SetParent(_contentParent);
                UI_StoreItemSlot slotClass = slot.GetComponent<UI_StoreItemSlot>();
                slotClass.Initialize(storeItem);
            
                Button button = slot.GetComponent<Button>();
                button.onClick.AddListener(() => ItemSlotClicked(button, storeItem));
            }
        }

        public void ItemSlotClicked(Button button, StoreItemSO storeItemSo)
        {
            OnSlotItemClicked?.Invoke(storeItemSo);
            EventSystem.current.SetSelectedGameObject(button.gameObject);
            button.Select();
        }

        public void ToggleView(bool toggle)
        {
            gameObject.SetActive(toggle);
        }
    }
}