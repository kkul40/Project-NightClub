using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIStoreInventoryView : MonoBehaviour
    {
        [SerializeField] private GameObject UISlotPrefab;
        [SerializeField] private GameObject UICargoSlotPrefab;
        [SerializeField] private GameObject UIExtenderSlotPrefab;
        [SerializeField] private Transform SlotHolder;

        [SerializeField] private Transform ButtonHolder;

        private IUISlot[] _uiSlots;

        private StoreDataCarrier _storeDataCarrier = new();

        private const int listSize = 50;

        private void Awake()
        {
            _uiSlots = new IUISlot[listSize];
        }

        public void GenerateInventory(StoreDataCarrier storeDataCarrier)
        {
            _storeDataCarrier = storeDataCarrier;

            InstantiateSlots();
            Load();
        }

        private void InstantiateSlots()
        {
            _uiSlots = new IUISlot[_storeDataCarrier.StoreItemSos.Count];

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
            for (var i = 0; i < _uiSlots.Length; i++)
            {
                _uiSlots[i].mGameobject.SetActive(true);
                _storeDataCarrier.ChosedStoreItemSo = _storeDataCarrier.StoreItemSos[i];
                _uiSlots[i].Init(_storeDataCarrier);
            }
        }
    }
}