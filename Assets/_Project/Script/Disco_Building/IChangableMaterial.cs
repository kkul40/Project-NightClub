using BuildingSystem.SO;
using Data;
using UnityEngine;

namespace BuildingSystem
{
    public interface IChangableMaterial
    {
        int assignedMaterialID { get; }
        MeshRenderer _meshRenderer { get; }
        eMaterialLayer MaterialLayer { get; }
        Material CurrentMaterial { get; }
        void UpdateMaterial(MaterialItemSo materialItemSo);
    }
}