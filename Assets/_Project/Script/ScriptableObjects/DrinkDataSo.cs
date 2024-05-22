using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Drink/New DrinkData")]
    public class DrinkDataSo : ScriptableObject
    {
        [field: SerializeField] public List<Drink> Drinks { get; private set; }
    }

    [Serializable]
    public class Drink
    {
        [field: SerializeField] public string Name { get; private set; }

        [field: SerializeField] public int DrinkAmount { get; private set; }

        [field: SerializeField] public float PrepareTime { get; private set; }

        [field: SerializeField] public int Price { get; private set; }

        [field: SerializeField] public GameObject Prefab { get; private set; }
    }
}