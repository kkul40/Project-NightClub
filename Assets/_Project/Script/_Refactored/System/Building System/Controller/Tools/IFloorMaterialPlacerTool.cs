using Disco_Building;
using Disco_ScriptableObject;
using DiscoSystem;
using UnityEngine;

namespace System.Building_System.Controller.Tools
{
    public class IFloorMaterialPlacerTool : ITool
    {
        private MaterialItemSo _materialItemSo;
    
        private IChangableMaterial _mouseOnChangableMaterial;
        private IChangableMaterial _currentChangableMaterial;
        private MaterialItemSo _storedMaterial;
    
    
        public bool isFinished { get; private set; }
    
        public void OnStart(ToolHelper TH)
        {
            _materialItemSo = TH.SelectedStoreItem as MaterialItemSo;
        }

        public bool OnValidate(ToolHelper TH)
        {
            if (_materialItemSo.Material == _storedMaterial.Material) return false;
            return true;
        }

        public void OnUpdate(ToolHelper TH)
        {
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
            }

            if (TH.InputSystem.LeftHoldClickOnWorld)
            {
                if (OnValidate(TH))
                {
                    OnPlace(TH);
                    SFXPlayer.Instance.PlaySoundEffect(SFXPlayer.Instance.Succes);
                }
                else
                {
                    SFXPlayer.Instance.PlaySoundEffect(SFXPlayer.Instance.Error, true);
                }
            }
        }

        public void OnPlace(ToolHelper TH)
        {
            _currentChangableMaterial = null;
        }

        public void OnStop(ToolHelper TH)
        {
            ResetPreviousMaterial();
        }

        private IChangableMaterial GetFloorMaterial(ToolHelper TH)
        {
            if (!TH.MouseInBoundryCheck()) return null;

            Vector3 mousePos = TH.InputSystem.GetMousePositionOnLayer(ToolHelper.GroundLayerID);
            mousePos.y = 0;
        
            return TH.DiscoData.MapData.GetFloorGridData((int)mousePos.x, (int)mousePos.z).assignedFloorTile;
        }
    
        private void ResetPreviousMaterial()
        {
            if (_currentChangableMaterial == null) return;
            _currentChangableMaterial.UpdateMaterial(_storedMaterial);
            _currentChangableMaterial = null;
        }
    }
}