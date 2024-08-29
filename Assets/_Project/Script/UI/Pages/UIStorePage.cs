using UnityEngine;

namespace UI
{
    public class UIStorePage : UIPageBase
    {
        [SerializeField] private GameObject _storeButtonHolder;
        [SerializeField] private UIStoreInventoryView uiStoreInventoryView;

        [SerializeField] private UIButton _lastButtonClicked;

        private void OnEnable()
        {
            _lastButtonClicked.OnClick();
        }

        public void GenerateInventory(UIButton uiButton, StoreDataCarrier storeDataCarrier)
        {
            _lastButtonClicked = uiButton;
            uiStoreInventoryView.GenerateInventory(storeDataCarrier);
        }
    }
}