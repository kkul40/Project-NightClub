using System;
using System.Collections.Generic;
using CharacterCustomization.UI;
using Sirenix.Serialization;
using UnityEngine;

namespace CharacterCustomization.Scriptables
{
    [CreateAssetMenu(fileName = "New Customization Items", menuName = "Data/Customization Items", order = 1)]
    public class CustomizationItemsSo : ScriptableObject
    {
        [Serializable]
        public class ItemGroup
        {
            public GameObject ArmaturePrefab;
            public List<CustomizationItem> Head;
            public List<CustomizationItem> Hair;
            public List<CustomizationItem> Accessoriees;
            public List<CustomizationItem> Top;
            public List<CustomizationItem> Bottom;
            public List<CustomizationItem> Shoes;
        }
        
        public ItemGroup MaleItems;
        public ItemGroup FemaleItems;
    }
}