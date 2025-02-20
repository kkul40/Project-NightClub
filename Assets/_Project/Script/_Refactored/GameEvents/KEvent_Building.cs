using System;

namespace DefaultNamespace._Refactored.Event
{
    public static class KEvent_Building
    {
        public static event Action<bool> OnBuildingToggled; 
        public static event Action OnPlacementPlaced;

        public static void TriggerPlacementPlaced()
        {
            OnPlacementPlaced?.Invoke();
        }

        public static void TriggerBuildingToggle(bool toggle)
        {
            OnBuildingToggled?.Invoke(toggle);
        }
    }
}