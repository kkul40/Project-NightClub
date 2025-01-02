using System;
using System.Collections.Generic;
using CharacterCustomization.UI;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
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
        
        [Searchable]
        public ItemGroup MaleItems;
        [Searchable]
        public ItemGroup FemaleItems;
    }
}