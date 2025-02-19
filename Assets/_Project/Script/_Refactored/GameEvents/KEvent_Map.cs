using System;
using UnityEngine;

namespace DefaultNamespace._Refactored.Event
{
    public static class KEvent_Map
    {
        public static event Action<Vector2Int> OnMapSizeChanged;

        public static void TriggerMapSizeChanged(Vector2Int mapSize)
        {
            OnMapSizeChanged?.Invoke(mapSize);
        }
    }
}