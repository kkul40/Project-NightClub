using Data;
using UnityEngine;

namespace BuildingSystem
{
    public interface IChangableMaterial
    {
        MeshRenderer _meshRenderer { get; }
        eMaterialLayer MaterialLayer { get; }
        Material CurrentMaterial { get; }
        void UpdateMaterial(Material material);
    }
}