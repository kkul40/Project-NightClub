using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Player Customization Data")]
    public class PlayerCustomizationDataSo : ScriptableObject
    {
        [Tooltip("0 = Male, 1 = Female")]
        public List<Mesh> playerGenders;
        public List<CustomizationItem> playerHairPrefabs;
        public List<CustomizationItem> playerBeardPrefabs;
        public List<CustomizationItem> playerAttachtmentPrefabs;
        public List<CustomizationItem> playerEaringPrefabs;
    }

    [Serializable]
    public class CustomizationItem
    {
        public GameObject Prefab;
        public Sprite Icon;
    }
}