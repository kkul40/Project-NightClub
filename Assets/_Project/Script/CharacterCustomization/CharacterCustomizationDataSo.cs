using UnityEngine;

namespace CharacterCustomization
{
    [CreateAssetMenu(fileName = "New Character Customization Data", menuName = "Data/Character Customization")]

    public class CharacterCustomizationDataSo : ScriptableObject
    {
        [System.Serializable]
        public class ItemGroup
        {
            // public GameObject m_BaseBody;
            // public CustomizationItemAsset[] m_Heads;
            // public CustomizationItemAsset[] m_Hairstyles;
            // public CustomizationItemAsset[] m_HeadAccessories;
            // public CustomizationItemAsset[] m_Tops;
            // public CustomizationItemAsset[] m_Bottoms;
            // public CustomizationItemAsset[] m_Shoes;
            // public CustomizationItemAsset[] m_Outfits;
        }

        [SerializeField] private float m_FakeLoadTime = 1f;

        [Space]
        [SerializeField] private ItemGroup m_MaleItems;
        [SerializeField] private ItemGroup m_FemaleItems;
    }
}