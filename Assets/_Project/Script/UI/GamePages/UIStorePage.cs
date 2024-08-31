using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class UIStorePage : UIPageBase
    {
        [SerializeField] private GameObject _storeButtonHolder;
        [SerializeField] private UIStoreInventoryView uiStoreInventoryView;

        [FormerlySerializedAs("_lastButtonClicked")] [SerializeField] private UIButtonBase lastButtonBaseClicked;

        public static Action<bool> OnStoreToggle;

        private void OnEnable()
        {
            lastButtonBaseClicked.OnClick();
        }

        public void GenerateInventory(UIButtonBase uiButtonBase, StoreDataCarrier storeDataCarrier)
        {
            lastButtonBaseClicked = uiButtonBase;
            uiStoreInventoryView.GenerateInventory(storeDataCarrier);
        }

        public override void Toggle(bool? toggle = null)
        {
            base.Toggle(toggle);
            OnStoreToggle?.Invoke(toggle ?? isActiveAndEnabled);
        }
    }
}