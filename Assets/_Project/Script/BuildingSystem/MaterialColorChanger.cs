using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BuildingSystem
{
    public class MaterialColorChanger : MonoBehaviour
    {
        [SerializeField] private Material blueMaterial;
        [SerializeField] private Material redMaterial;
        [SerializeField] private Material yellowMaterial;
        [SerializeField] private Material whiteMaterial;

        private Dictionary<Transform, MaterialData>
            materialData = new Dictionary<Transform, MaterialData>();

        public void SetMaterialsColorByValidity(List<MeshRenderer> meshRenderers, bool canPlace)
        {
            Material tempMaterial = canPlace ? blueMaterial : redMaterial;
            SetMaterial(meshRenderers, tempMaterial);
        }

        public void SetMaterialTransparency(Transform transform)
        {
            // GetChilds Of Transform
            for (int i = 0; i < transform.childCount; i++)
            {
                materialData.Add(transform.GetChild(i), new MaterialData(ReturnMeshRendererList(transform.GetChild(i).gameObject)));
                
                var listMesh = ReturnMeshRendererList(transform.GetChild(i).gameObject);

                SetMaterial(listMesh, whiteMaterial);
            }
        }

        private static void SetMaterial(List<MeshRenderer> meshRenderers, Material tempMaterial)
        {
            foreach (var mesh in meshRenderers)
            {
                Material[] material = new Material[mesh.materials.Length];
                for (int i = 0; i < mesh.materials.Length; i++)
                {
                    material[i] = tempMaterial;
                }
                mesh.materials = material;
            }
        }

        public void SetTransparencyToDefault()
        {
            if (materialData.Count <= 0) return;

            foreach (var key in materialData.Keys)
            {
                var listMesh = ReturnMeshRendererList(key.gameObject);

                for (int i = 0; i < listMesh.Count; i++)
                {
                    listMesh[i].materials = materialData[key].Materials[i].ToArray();
                }
            }

            materialData = new Dictionary<Transform, MaterialData>();
        }
        
        public List<MeshRenderer> ReturnMeshRendererList(GameObject gameObject) => gameObject.GetComponentsInChildren<MeshRenderer>().ToList();
    }

    public struct MaterialData
    {
        public List<MeshRenderer> MeshRenderer;
        public List<List<Material>> Materials;

        public MaterialData(List<MeshRenderer> meshRenderer)
        {
            MeshRenderer = meshRenderer;
            Materials = new List<List<Material>>();
            for (int i = 0; i < meshRenderer.Count; i++)
            {
                Materials.Add(meshRenderer[i].materials.ToList());
            }
        }
    }
}