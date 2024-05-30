using System;
using BuildingSystem.SO;
using New_NPC;
using NPC;
using ScriptableObjects;
using UnityEngine;

namespace Data
{
    public class InitConfig : Singleton<InitConfig>
    {
        [Header("Default Material")]
        [SerializeField] private MaterialItemSo DefaultTileMaterial;
        [SerializeField] private MaterialItemSo DefaultWallMaterial;
        [Header("Default NPC Customization")]
        [SerializeField] private NPCCustomizationDataSO DefaultBoyNpcCustomization;
        [SerializeField] private NPCCustomizationDataSO DefaultGirlNpcCustomization;
        [Header("Default NPC Animation")]
        [SerializeField] private NpcAnimationSo DefaultBoyNpcAnimation;
        [SerializeField] private NpcAnimationSo DefaultGirlNpcAnimation;

        public NpcAnimationSo GetDefaultGirlNpcAnimation
        {
            get => DefaultGirlNpcAnimation;
        }
        public NpcAnimationSo GetDefaultBoyNpcAnimation
        {
            get => DefaultBoyNpcAnimation;
        }
        public MaterialItemSo GetDefaultTileMaterial
        {
            get { return DefaultTileMaterial; }
        }
        public MaterialItemSo GetDefaultWallMaterial
        {
            get { return DefaultWallMaterial; }
        }
        public NPCCustomizationDataSO GetDefaultBoyNpcCustomization
        {
            get => DefaultBoyNpcCustomization;
        }
        public NPCCustomizationDataSO GetDefaultGirlNpcCustomization
        {
            get => DefaultGirlNpcCustomization;
        }
    }
}