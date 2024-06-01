using System;
using System.Collections.Generic;
using BuildingSystem.SO;
using UnityEngine;

namespace UI
{
    public class UIStoreManager : Singleton<UIStoreManager>
    {
        [SerializeField] private UIStoreInventory _uiStoreInventory;

        public void GenerateInventory(List<StoreItemSO> storeItemSos)
        {
            _uiStoreInventory.GenerateInventory(storeItemSos);
        }
    }
}