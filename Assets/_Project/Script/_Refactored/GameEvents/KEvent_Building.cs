using System;
using PropBehaviours;

namespace GameEvents
{
    public static class KEvent_Building
    {
        public static event Action<bool> OnBuildingToggled; 
        public static event Action OnPlaced;
        public static event Action<IPropUnit> OnPropPlaced;
        public static event Action<IPropUnit> OnPropRemoved;
        public static event Action<IPropUnit> OnPropRelocated;
        
        public static event Action<int> OnPlacementRemove; 
        public static event Action<int> OnPlacementRelocate;
        public static event Action<WallDoor> OnWallDoorRelocate; 

        public static void TriggerPlaced()
        {
            OnPlaced?.Invoke();
        }

        public static void TriggerPropPlaced(IPropUnit propUnit)
        {
            OnPropPlaced?.Invoke(propUnit);
        }

        public static void TriggerPropRemoved(IPropUnit propUnit)
        {
            OnPropRemoved?.Invoke(propUnit);
        }

        public static void TriggerPropRelocated(IPropUnit propUnit)
        {
            OnPropRelocated?.Invoke(propUnit);
        }

        public static void TriggerBuildingToggle(bool toggle)
        {
            OnBuildingToggled?.Invoke(toggle);
        }

        public static void TriggerPlacementRelocate(int instanceID)
        {
            OnPlacementRelocate?.Invoke(instanceID);
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