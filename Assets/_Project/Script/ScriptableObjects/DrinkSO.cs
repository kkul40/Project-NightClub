using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    public class DrinkSO : ScriptableObject
    {
        public int ID;
        public Sprite Icon;
        public string Name;
        public int DrinkAmount;
        public float PrepareTime;
        public int Price;
        public GameObject Prefab;
        
        private void Awake()
        {
            ID = name.GetHashCode() + Name.GetHashCode();
        }
    }
}