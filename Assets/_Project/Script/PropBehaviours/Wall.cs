using BuildingSystem;
using Data;
using UnityEngine;

namespace PropBehaviours
{
    public class Wall : MonoBehaviour, IInteractable, IChangableMaterial
    {
        protected void Start()
        {
            GameData.Instance.WallMap.Add(this);
            LoadMaterial();
        }

        public eInteraction Interaction { get; } = eInteraction.None;

        public void OnFocus()
        {
        }

        public void OnOutFocus()
        {
        }

        public void OnClick()
        {
        }

        public eMaterialLayer MaterialLayer { get; } = eMaterialLayer.Wall;
        public Material CurrentMaterial { get; set; }
        public void LoadMaterial()
        {
            CurrentMaterial = InitConfig.Instance.GetDefaultWallMaterial.Material;
            UpdateMaterial();
        }

        public virtual void UpdateMaterial()
        {
            MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
            var materials = meshRenderer.materials;
            materials[1] = CurrentMaterial;
            meshRenderer.materials = materials;
        }
    }
}