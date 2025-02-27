using System;
using UnityEngine;

namespace GameEvents
{
    public static class KEvent_Map
    {
        public static event Action<Vector2Int> OnMapSizeChanged;

        public static event Action OnMapSizeUpdate;

        public static void TriggerMapSizeChanged(Vector2Int mapSize)
        {
            OnMapSizeUpdate?.Invoke();
            OnMapSizeChanged?.Invoke(mapSize);
        }
    }
}