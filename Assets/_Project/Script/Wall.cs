using System;
using UnityEngine;

namespace _Project.Script.NewSystem
{
    public class Wall : MonoBehaviour
    {
        protected MeshRenderer _meshRenderer;

        protected virtual void Start()
        {
            GameData.Instance.WallMap.Add(this);
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        public virtual void ChangeWallpaper(Material newWallPaper)
        {
            var materials = _meshRenderer.materials;
            materials[1] = newWallPaper;
            _meshRenderer.materials = materials;
        }

        public virtual void ResetWallPaper(Material[] defaultMaterials)
        {
            var materials = _meshRenderer.materials;
            materials = defaultMaterials;
            _meshRenderer.materials = materials;
        }

        public Material[] GetCurrentMaterial() => _meshRenderer.materials;
    }
}

