using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DiscoSystem.Building_System
{
    public class MaterialColorChanger : MonoBehaviour
    {
        public enum eMaterialColor
        {
            TransparentMaterial,
            RemovingMaterial
        }
        
        [SerializeField] private Material greenMaterial;
        [SerializeField] private Material redMaterial;
        [SerializeField] private Material yellowMaterial;
        [SerializeField] private Material whiteMaterial;
        public void SetMaterialsColorByValidity(List<MeshRenderer> meshRenderers, bool canPlace)
        {
            // TODO There is A Memory Leak On This Fucntion. Fix It Later.
            var tempMaterial = canPlace ? greenMaterial : redMaterial;
            SetMaterial(meshRenderers, tempMaterial);
        }

        public void SetMaterialColor(List<MeshRenderer> meshRenderers, Material material)
        {
            SetMaterial(meshRenderers, material);
        }

        public void SetCustomMaterial(Transform transform, eMaterialColor eMaterialColor,
            ref Dictionary<Transform, MaterialData> materialDatas)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                materialDatas.Add(transform.GetChild(i),
                    new MaterialData(ReturnMeshRendererList(transform.GetChild(i).gameObject)));

                var listMesh = ReturnMeshRendererList(transform.GetChild(i).gameObject);
                switch (eMaterialColor)
                {
                    case eMaterialColor.TransparentMaterial:
                        SetMaterial(listMesh, whiteMaterial);
                        break;
                    case eMaterialColor.RemovingMaterial:
                        SetMaterial(listMesh, yellowMaterial);
                        break;
                }
            }
        }

        /// <summary>
        /// To use this function you need to Call SetCustomMaterial to Initiliaze first
        /// </summary>
        /// <param name="materialDatas"></param>
        public void SetMaterialToDefault(ref Dictionary<Transform, MaterialData> materialDatas)
        {
            if (materialDatas.Count == 0) return;
            foreach (var key in materialDatas.Keys)
            {
                var listMesh = materialDatas[key].MeshRenderer;
                for (var i = 0; i < listMesh.Count; i++)
                {
                    if(listMesh[i] == null) continue;
                    listMesh[i].materials = materialDatas[key].Materials[i].ToArray();
                }
            }

            materialDatas = new Dictionary<Transform, MaterialData>();
        }

        private void SetMaterial(List<MeshRenderer> meshRenderers, Material tempMaterial)
        {
            foreach (var mesh in meshRenderers) SetMaterial(mesh, tempMaterial);
        }

        private void SetMaterial(MeshRenderer meshRenderer, Material material)
        {
            var tempMaterial = new Material[meshRenderer.materials.Length];
            for (var i = 0; i < meshRenderer.materials.Length; i++) tempMaterial[i] = material;

            meshRenderer.materials = tempMaterial;
        }

        public List<MeshRenderer> ReturnMeshRendererList(GameObject gameObject)
        {
            return gameObject.GetComponentsInChildren<MeshRenderer>().ToList();
        }

        public struct MaterialData
        {
            public List<MeshRenderer> MeshRenderer;
            public List<List<Material>> Materials;

            public MaterialData(List<MeshRenderer> meshRenderer)
            {
                MeshRenderer = meshRenderer;
                Materials = new List<List<Material>>();
                for (var i = 0; i < meshRenderer.Count; i++) Materials.Add(meshRenderer[i].materials.ToList());
            }
        }
    }
}