using System;
using UnityEditor;
using UnityEngine;

namespace BuildingSystem.SO
{
    public class StoreItemSO : ScriptableObject
    {
        public int ID;
        public string Name;
        public Sprite Icon;
        public int Price;

        private void Awake()
        {
            ID = Guid.NewGuid().GetHashCode();
        }
    }
}