using System;
using UnityEngine;

namespace _Project.Script.NewSystem
{
    public class Wall : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;

        public Material currentMaterial;
        
        private void Start()
        {
            GameData.Instance.WallMap.Add(this);
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        public void ChangeWallpaper(Material newWallPaper)
        {
            Debug.Log("material has changed");
            var materials = _meshRenderer.materials;
            materials[1] = newWallPaper;
            _meshRenderer.materials = materials;
        }

        public Material[] GetCurrentMaterial() => _meshRenderer.materials;
    }
}