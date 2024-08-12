using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    public class Drink : ScriptableObject
    {
        public string Name;
        public int DrinkAmount;
        public float PrepareTime;
        public int Price;
        public GameObject Prefab;
    }
}