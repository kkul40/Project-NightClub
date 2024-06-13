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
        public List<StoreItemSO> StoreItemSos = new List<StoreItemSO>();
        public StoreItemSO ChosedStoreItemSo = null;
        public Dictionary<StoreItemSO, int> inventory = new Dictionary<StoreItemSO, int>();
        
        public DiscoData.eUISlot EUISlot;
    }
}