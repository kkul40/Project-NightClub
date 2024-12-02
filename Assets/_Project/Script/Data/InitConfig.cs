using System;
using Disco_ScriptableObject;
using NPCBehaviour;
using ScriptableObjects;
using UnityEngine;

namespace Data
{
    public class InitConfig : Singleton<InitConfig>
    {
        [Header("UI Style")] 
        [SerializeField] private UITextStyle DefaultUITextStyle;
        [SerializeField] private UIColorStyle DefaultUIColorStyle;
        
        [Header("Default Material")] 
        [SerializeField] private MaterialItemSo DefaultTileMaterial;
        [SerializeField] private MaterialItemSo DefaultWallMaterial;

        [Header("Default NPC Customization")] 
        [SerializeField] private NPCCustomizationDataSO DefaultBoyNpcCustomization;
        [SerializeField] private NPCCustomizationDataSO DefaultGirlNpcCustomization;

        [Header("Default NPC Animation")] 
        [SerializeField] private NpcAnimationSo DefaultBoyNpcAnimation;
        [SerializeField] private NpcAnimationSo DefaultGirlNpcAnimation;
        [SerializeField] private BartenderAnimationSo DefaultBartenderAnimation;

        // GETTERS
        public UITextStyle GetDefaultUITextStyle => DefaultUITextStyle;
        public UIColorStyle GetDefaultUIColorStyle => DefaultUIColorStyle;
        public MaterialItemSo GetDefaultTileMaterial => DefaultTileMaterial;
        public MaterialItemSo GetDefaultWallMaterial => DefaultWallMaterial;
        public NPCCustomizationDataSO GetDefaultBoyNpcCustomization => DefaultBoyNpcCustomization;
        public NPCCustomizationDataSO GetDefaultGirlNpcCustomization => DefaultGirlNpcCustomization;
        public NpcAnimationSo GetDefaultGirlNpcAnimation => DefaultGirlNpcAnimation;
        public NpcAnimationSo GetDefaultBoyNpcAnimation => DefaultBoyNpcAnimation;
        public BartenderAnimationSo GetDefaultBartenderAnimation => DefaultBartenderAnimation;
    }
}