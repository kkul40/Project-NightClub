using DiscoSystem;
using UnityEngine;

namespace CharacterCustomization
{
    public class CharacterCustomizationManager : Singleton<CharacterCustomizationManager>
    {
        [SerializeField] private CharacterCustomizationDataSo _characterCustomization;
    }
}