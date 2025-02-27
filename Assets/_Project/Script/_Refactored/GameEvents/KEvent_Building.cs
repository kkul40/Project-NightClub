using System;
using Disco_ScriptableObject;
using PropBehaviours;

namespace GameEvents
{
    public static class KEvent_Building
    {
        public static event Action<bool> OnBuildingToggled; 
        public static event Action OnPlaced;
        public static event Action<IPropUnit> OnPropPlaced;
        public static event Action<IPropUnit> OnPropRemoved; 
        public static event Action<int> OnPlacementRemove; 
        public static event Action<int, StoreItemSO> OnPlacementRelocate;
        public static event Action<WallDoor> OnWallDoorRelocate; 

        public static void TriggerPlaced()
        {
            OnPlaced?.Invoke();
        }

        public static void TriggerPlaced(IPropUnit propUnit)
        {
            OnPropPlaced?.Invoke(propUnit);
        }

        public static void TriggerRemoved(IPropUnit propUnit)
        {
            OnPropRemoved?.Invoke(propUnit);
        }

        public static void TriggerBuildingToggle(bool toggle)
        {
            OnBuildingToggled?.Invoke(toggle);
        }

        public static void TriggerPlacementRelocate(int instanceID, StoreItemSO storeItemSo)
        {
            OnPlacementRelocate?.Invoke(instanceID, storeItemSo);
        }

        public static void TriggerOnWallDoorRelocate(WallDoor wallDoor)
        {
            OnWallDoorRelocate?.Invoke(wallDoor);
        }

        public static void TriggerPlacementRemove(int instanceID)
        {
            OnPlacementRemove?.Invoke(instanceID);
        }
    }
}