using CharacterCustomization.Scriptables;
using Disco_ScriptableObject;
using DiscoSystem;
using DiscoSystem.Character.Bartender;
using DiscoSystem.Character.NPC;
using ScriptableObjects;
using UnityEngine;

namespace Data
{
    public class InitConfig : Singleton<InitConfig>
    {
        public NewAnimationSO test;

        [Header("UI Style")] 
        [SerializeField] private UITextStyle DefaultUITextStyle;
        [SerializeField] private UIColorStyle DefaultUIColorStyle;
        
        [Header("Default Material")] 
        [SerializeField] private MaterialItemSo DefaultTileMaterial;
        [SerializeField] private MaterialItemSo DefaultWallMaterial;

        [Header("Default NPC Customization")] 
        [SerializeField] private CustomizationItemsSo DefaultNPCCustomization;
        [SerializeField] private CustomizationItemsSo DefaultBartenderCustomization;

        [Header("Default NPC Animation")] 
        [SerializeField] private NewAnimationSO DefaultBoyNpcAnimation;
        [SerializeField] private NewAnimationSO DefaultGirlNpcAnimation;
        [SerializeField] private BartenderAnimationSo DefaultBartenderAnimation;

        // GETTERS
        public UITextStyle GetDefaultUITextStyle => DefaultUITextStyle;
        public UIColorStyle GetDefaultUIColorStyle => DefaultUIColorStyle;
        public MaterialItemSo GetDefaultTileMaterial => DefaultTileMaterial;
        public MaterialItemSo GetDefaultWallMaterial => DefaultWallMaterial;
        public CustomizationItemsSo GetDefaultNPCCustomization => DefaultNPCCustomization;
        public CustomizationItemsSo GetefaultBartenderCustomization => DefaultBartenderCustomization;
        public NewAnimationSO GetDefaultGirlNpcAnimation => DefaultGirlNpcAnimation;
        public NewAnimationSO GetDefaultBoyNpcAnimation => DefaultBoyNpcAnimation;
        public BartenderAnimationSo GetDefaultBartenderAnimation => DefaultBartenderAnimation;
    }
}