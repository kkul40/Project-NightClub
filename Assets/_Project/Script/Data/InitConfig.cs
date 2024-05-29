using System;
using BuildingSystem.SO;
using UnityEngine;

namespace Data
{
    public class InitConfig : Singleton<InitConfig>
    {
        [SerializeField] private MaterialItemSo DefaultTileMaterial;
        [SerializeField] private MaterialItemSo DefaultWallMaterial;
    
        public MaterialItemSo GetDefaultTileMaterial
        {
            get { return DefaultTileMaterial; }
        }

        public MaterialItemSo GetDefaultWallMaterial
        {
            get { return DefaultWallMaterial; }
        }
    }
}