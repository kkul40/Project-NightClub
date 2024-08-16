using System;
using BuildingSystem;
using Data;
using UnityEngine;

namespace PropBehaviours
{
    public class Wall : MonoBehaviour, IInteractable, IChangableMaterial
    {
        public bool IsInteractable { get; } = true;
        public eInteraction Interaction { get; } = eInteraction.None;

        public void OnFocus()
        {
        }

        public void OnOutFocus()
        {
        }

        public void OnClick()
        {
            // Debug.Log(DiscoData.Instance.mapData.WallDatas.Find(x => x.assignedWall == this).assignedMaterialID);
            // Debug.Log(DiscoData.Instance.SavingSystem.CurrentSavedData.WallDatas.Find(x => x.assignedWall == this).assignedMaterialID);

            // Debug.Log(DiscoData.Instance.mapData.WallDatas.Count);
            // Debug.Log(DiscoData.Instance.SavingSystem.CurrentSavedData.WallDatas.Count);
        }

        public virtual MeshRenderer _meshRenderer => transform.GetComponentInChildren<MeshRenderer>();

        public eMaterialLayer MaterialLayer { get; } = eMaterialLayer.WallMaterial;

        public virtual Material CurrentMaterial => _meshRenderer.materials[1];

        public virtual void UpdateMaterial(Material material)
        {
            var materials = _meshRenderer.materials;
            materials[1] = material;
            _meshRenderer.materials = materials;
        }
    }
}