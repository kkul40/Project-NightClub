using System;
using System.Collections.Generic;
using BuildingSystem.SO;
using Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class UIStoreManager : Singleton<UIStoreManager>
    {
        [SerializeField] private UIStoreInventoryView uiStoreInventoryView;

        public UIStoreInventoryView GetUiStoreInventoryView => uiStoreInventoryView;
    }
}