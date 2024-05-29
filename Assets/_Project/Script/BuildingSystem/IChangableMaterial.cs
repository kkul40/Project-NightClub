using UnityEngine;

namespace BuildingSystem
{
    public interface IChangableMaterial
    {
        eMaterialLayer MaterialLayer { get; }
        Material CurrentMaterial { get; set; }
        void LoadMaterial();
        void UpdateMaterial();
    
    }
}