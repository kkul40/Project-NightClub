using UnityEngine;

namespace BuildingSystem
{
    public class MaterialColorChanger : MonoBehaviour
    {
        [SerializeField] private Material blueMaterial;
        [SerializeField] private Material redMaterial;
        [SerializeField] private Material yellowMaterial;

        public void SetMaterialsColor(MeshRenderer meshRenderer, bool canPlace)
        {
            meshRenderer.material = canPlace ? blueMaterial : redMaterial;
        }
    }
}