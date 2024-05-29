using System;
using _1BuildingSystemNew;
using _1BuildingSystemNew.SO;
using UnityEngine;

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