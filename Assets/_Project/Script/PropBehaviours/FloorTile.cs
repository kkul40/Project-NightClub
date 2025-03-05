using System;
using System.Building_System;
using Data;
using Disco_Building;
using Disco_ScriptableObject;
using UnityEngine;

namespace PropBehaviours
{
    public class FloorTile : MonoBehaviour, IInteractable, IChangableMaterial
    {
        #region IChangableMaterial

        public int assignedMaterialID { get; private set; }
        public MeshRenderer _meshRenderer => GetComponentInChildren<MeshRenderer>();

        public eMaterialLayer MaterialLayer { get; } = eMaterialLayer.FloorMaterial;

        public Material CurrentMaterial => _meshRenderer.material;
        public void UpdateMaterial(MaterialItemSo materialItemSo)
        {
            _meshRenderer.material = materialItemSo.Material;
            assignedMaterialID = materialItemSo.ID;
        }

        #endregion

        #region IInteractable

        public GameObject mGameobject { get; private set; }
        public bool IsInteractable { get; } = true;
        public bool IsAnimatable { get; } = true;
        public eInteraction Interaction { get; } = eInteraction.None;

        private void Start()
        {
            mGameobject = gameObject;
        }

        public void OnFocus()
        {
        }

        public void OnOutFocus()
        {
        }

        public void OnClick()
        {
            // var a = GridHandler.Instance.GetMouseCellPosition(InputSystem.Instance.GetMouseMapPosition(), eGridType.PathFinderGrid);
            // var test = DiscoData.Instance.MapData.FloorGridDatas[a.x, a.z];
            // Debug.Log(test.CellPosition);
            // Debug.Log(test.assignedMaterialID);

            // Debug.Log(DiscoData.Instance.SavingSystem.CurrentSavedData.FloorGridDatas[a.x, a.z].assignedMaterialID);
            // Debug.Log(GridHandler.Instance.GetMouseCellPosition(InputSystem.Instance.GetMouseMapPosition(), eGridType.PlacementGrid));
            // Debug.Log(DiscoData.Instance.MapData.GetTileNodeByCellPos(a));
        }

        #endregion
    }
}