using System;
using UI.GamePages.GameButtons;
using UnityEngine;

namespace UI.GamePages
{
    public class UIStorePage : UIPageBase
    {
        public override PageType PageType { get; protected set; } = PageType.FullPage;

        [SerializeField] private GameObject _storeButtonHolder;
        [SerializeField] private UIStoreInventoryView uiStoreInventoryView;

        [SerializeField] private UIButtonBase lastButtonBaseClicked;

        public static Action<bool> OnStoreToggle;

        public void GenerateInventory(UIButtonBase uiButtonBase, StoreDataCarrier storeDataCarrier)
        {
            lastButtonBaseClicked = uiButtonBase;
            uiStoreInventoryView.GenerateInventory(storeDataCarrier);
        }


        protected override void OnShow()
        {
            lastButtonBaseClicked.OnClick();
            OnStoreToggle?.Invoke(true);
        }

        protected override void OnHide()
        {
            OnStoreToggle?.Invoke(false);
        }
    }
}