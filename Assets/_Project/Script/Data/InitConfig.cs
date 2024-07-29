﻿using System;
using BuildingSystem.SO;
using New_NPC;
using ScriptableObjects;
using UnityEngine;

namespace Data
{
    public class InitConfig : Singleton<InitConfig>
    {
        [Header("Default Material")] [SerializeField]
        private MaterialItemSo DefaultTileMaterial;

        [SerializeField] private MaterialItemSo DefaultWallMaterial;

        [Header("Default NPC Customization")] [SerializeField]
        private NPCCustomizationDataSO DefaultBoyNpcCustomization;

        [SerializeField] private NPCCustomizationDataSO DefaultGirlNpcCustomization;

        [Header("Default NPC Animation")] [SerializeField]
        private NpcAnimationSo DefaultBoyNpcAnimation;

        [SerializeField] private NpcAnimationSo DefaultGirlNpcAnimation;
        [SerializeField] private BartenderAnimationSo DefaultBartenderAnimation;

        public MaterialItemSo GetDefaultTileMaterial => DefaultTileMaterial;
        public MaterialItemSo GetDefaultWallMaterial => DefaultWallMaterial;

        public NPCCustomizationDataSO GetDefaultBoyNpcCustomization => DefaultBoyNpcCustomization;
        public NPCCustomizationDataSO GetDefaultGirlNpcCustomization => DefaultGirlNpcCustomization;

        public NpcAnimationSo GetDefaultGirlNpcAnimation => DefaultGirlNpcAnimation;
        public NpcAnimationSo GetDefaultBoyNpcAnimation => DefaultBoyNpcAnimation;
        public BartenderAnimationSo GetDefaultBartenderAnimation => DefaultBartenderAnimation;
    }
}