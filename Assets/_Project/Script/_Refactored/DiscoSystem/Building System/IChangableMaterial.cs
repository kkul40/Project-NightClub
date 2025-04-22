using Data;
using Disco_ScriptableObject;
using UnityEngine;

namespace DiscoSystem.Building_System
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