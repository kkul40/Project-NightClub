using System;
using UnityEngine;

namespace CharacterCustomization.Scriptables
{
    [CreateAssetMenu(fileName = "New Character Customization Data", menuName = "Data/Character Customization")]

    public class CharacterCustomizationDataSo : ScriptableObject
    {
        [Serializable]
        public class ItemGroup
        {
            public GameObject m_BaseBody;
            public CustomizationItem[] m_Heads;
            public CustomizationItem[] m_Hairstyles;
            public CustomizationItem[] m_HeadAccessories;
            public CustomizationItem[] m_Tops;
            public CustomizationItem[] m_Bottoms;
            public CustomizationItem[] m_Shoes;
            public CustomizationItem[] m_Outfits;
        }

        [SerializeField] private float m_FakeLoadTime = 1f;

        [Space]
        [SerializeField] private ItemGroup m_MaleItems;
        [SerializeField] private ItemGroup m_FemaleItems;
    }
}