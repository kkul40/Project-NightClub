using UnityEngine;

namespace _1BuildingSystemNew
{
    public interface IChangableMaterial
    {
        eMaterialLayer MaterialLayer { get; }
        Material CurrentMaterial { get; set; }
        void LoadMaterial();
        void UpdateMaterial();
    
    }
}