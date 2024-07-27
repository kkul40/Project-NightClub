using System.Collections.Generic;
using BuildingSystem.SO;
using Data;
using UnityEngine;

namespace UI
{
    public interface IUISlot
    {
        public GameObject mGameobject { get; }
        public StoreItemSO StoreItemSo { get; }
        public void Init(StoreDataCarrier storeDataCarrier);
        public void OnClick();
    }

    public class StoreDataCarrier
    {
        // TODO Bu Store Data Mantigi Degistir!!!
        public List<StoreItemSO> StoreItemSos = new();
        public StoreItemSO ChosedStoreItemSo = null;
        public Dictionary<StoreItemSO, int> inventory = new();

        public DiscoData.eUISlot EUISlot;
    }
}