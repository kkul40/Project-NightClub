using System;
using System.Character.NPC;
using UnityEngine;

namespace GameEvents
{
    public static class KEvent_Map
    {
        public static event Action<Vector2Int> OnMapSizeChanged;

        public static event Action OnMapSizeUpdate;

        public static event Action<int, int> OnExtendMapSize;

        public static void TriggerMapSizeChanged(Vector2Int mapSize)
        {
            OnMapSizeUpdate?.Invoke();
            OnMapSizeChanged?.Invoke(mapSize);
        }

        public static void TriggerExtendMapSize(int x, int y)
        {
            OnExtendMapSize?.Invoke(x, y);
        }

        public static void TriggerExtenMapSize(Vector2Int vector2)
        {
            TriggerExtendMapSize(vector2.x, vector2.y);
        }
    }
}