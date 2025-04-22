using Data;
using Disco_ScriptableObject;
using DiscoSystem;
using ExtensionMethods;
using UnityEngine;

namespace System.Building_System.Controller.Tools
{
    public class IFloorMaterialPlacerTool : ITool
    {
        private MaterialItemSo _materialItemSo;
    
        private IChangableMaterial _mouseOnChangableMaterial;
        private IChangableMaterial _currentChangableMaterial;
        private MaterialItemSo _storedMaterial;
        private FloorData _mouseOnFloorData;
        
        
        public bool isFinished { get; private set; }


        public void OnStart(ToolHelper TH)
        {
            _materialItemSo = TH.SelectedStoreItem as MaterialItemSo;

            TH.TileIndicator.SetTileIndicator(ePlacingType.Place, Vector2.one);
        }

        public bool OnValidate(ToolHelper TH)
        {
            if (_mouseOnChangableMaterial == null) return false;
            if (_materialItemSo.Material == _storedMaterial.Material) return false;
            return true;
        }

        public void OnUpdate(ToolHelper TH)
        {
            
            Debug.Log("Floor Material placer");
            _mouseOnChangableMaterial = GetFloorMaterial(TH);
        
            if (_mouseOnChangableMaterial == null)
            {
                ResetPreviousMaterial();
                return;
            }

            if (_mouseOnChangableMaterial != _currentChangableMaterial)
            {
                ResetPreviousMaterial();
                _currentChangableMaterial = _mouseOnChangableMaterial;

                _storedMaterial = TH.DiscoData.FindAItemByID(_currentChangableMaterial.assignedMaterialID) as MaterialItemSo;
                _mouseOnChangableMaterial.UpdateMaterial(_materialItemSo);
                
                TH.LastPosition = _mouseOnFloorData.CellPosition.CellCenterPosition(eGridType.PlacementGrid);
                TH.TileIndicator.SetPositionAndRotation(TH.LastPosition, Quaternion.identity);
            }
        }

        public void OnPlace(ToolHelper TH)
        {
            _currentChangableMaterial = null;
            TH.FXCreatorSystem.CreateFX(FXType.Floor, TH.LastPosition, Vector2.one, Quaternion.identity);
        }

        public void OnStop(ToolHelper TH)
        {
            ResetPreviousMaterial();
            TH.TileIndicator.CloseTileIndicator();
        }

        public bool CheckPlaceInput(ToolHelper TH)
        {
            return TH.InputSystem.LeftHoldClickOnWorld;
        }

        private IChangableMaterial GetFloorMaterial(ToolHelper TH)
        {
            if (!TH.MouseInBoundryCheck()) return null;

            Vector3 mousePos = TH.InputSystem.GetMousePositionOnLayer(ToolHelper.GroundLayerID);
            mousePos.y = 0;
            _mouseOnFloorData = TH.DiscoData.MapData.GetFloorGridData((int)mousePos.x, (int)mousePos.z);
            return _mouseOnFloorData.assignedFloorTile;
        }
    
        private void ResetPreviousMaterial()
        {
            if (_currentChangableMaterial == null) return;
            _currentChangableMaterial.UpdateMaterial(_storedMaterial);
            _currentChangableMaterial = null;
        }
    }
}