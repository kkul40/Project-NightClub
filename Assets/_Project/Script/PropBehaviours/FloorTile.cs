using System;
using BuildingSystem;
using Data;
using UnityEngine;

namespace PropBehaviours
{
    public class FloorTile : MonoBehaviour, IInteractable, IChangableMaterial
    {
        #region IChangableMaterial

        public MeshRenderer _meshRenderer => GetComponentInChildren<MeshRenderer>();

        public eMaterialLayer MaterialLayer { get; } = eMaterialLayer.FloorMaterial;

        public Material CurrentMaterial => _meshRenderer.material;

        public void UpdateMaterial(Material material)
        {
            _meshRenderer.material = material;
        }

        #endregion

        #region IInteractable

        public eInteraction Interaction { get; } = eInteraction.None;

        public void OnFocus()
        {
        }

        public void OnOutFocus()
        {
        }

        public void OnClick()
        {
            var a = GridHandler.Instance.GetMouseCellPosition(InputSystem.Instance.GetMouseMapPosition());
            var test = DiscoData.Instance.MapData.FloorGridDatas[a.x, a.z];
            // Debug.Log(test.CellPosition);
            // Debug.Log(test.assignedMaterialID);

            // Debug.Log(DiscoData.Instance.SavingSystem.CurrentSavedData.FloorGridDatas[a.x, a.z].assignedMaterialID);
            Debug.Log(DiscoData.Instance.MapData.GetTileNodeByCellPos(a).IsWalkable);
        }

        #endregion
    }
}