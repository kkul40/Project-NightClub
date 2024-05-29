using UnityEngine;

namespace PropBehaviours
{
    public interface IMaterial
    {
        public MeshRenderer MeshRenderer { get; }
        public void ChangeMaterial(Material material);
        public void ResetMaterial(Material[] materials);

        public Material[] GetCurrentMaterial()
        {
            return MeshRenderer.materials;
        }
    }
}