using System.Collections.Generic;
using Data;
using Disco_ScriptableObject;
using UnityEngine;

namespace DiscoSystem.Building_System.Controller.Tools
{
    public class IWallMaterialPlacementTool : ITool
    {
        private MaterialItemSo _materialItemSo;

        private WallData _closestWallData;
        private IChangableMaterial _mouseOnChangableMaterial;
        private IChangableMaterial _currentChangableMaterial;
        private MaterialItemSo _storedMaterial;
    
        private Dictionary<Transform, MaterialColorChanger.MaterialData> _materialDatas = new();

        public bool isFinished { get; private set; }

        public void OnStart(ToolHelper TH)
        {
            _materialDatas = new Dictionary<Transform, MaterialColorChanger.MaterialData>();
            _materialItemSo = TH.SelectedStoreItem as MaterialItemSo;
        }

        public bool OnValidate(ToolHelper TH)
        {
            if (_currentChangableMaterial == null) return false;
            if (_materialItemSo.Material == _storedMaterial.Material) return false;
            return true;
        }

        public void OnUpdate(ToolHelper TH)
        {
            _mouseOnChangableMaterial = GetClosestWallMaterial(TH);
        
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
        }

        public void OnPlace(ToolHelper TH)
        {
            TH.PlacementTracker.AddTrack(new WallMaterialUndo(_storedMaterial.ID, _currentChangableMaterial.assignedMaterialID, _closestWallData.CellPosition));
            
            _closestWallData.AssignNewID(_materialItemSo);
            _currentChangableMaterial = null;

            Vector3 position = _closestWallData.assignedWall.transform.position;
            Quaternion rotation = _closestWallData.assignedWall.transform.rotation;
            TH.FXCreatorSystem.CreateFX(FXType.Wall, position , Vector2.one, rotation);
        }

        public void OnStop(ToolHelper TH)
        {
            ResetPreviousMaterial();
            TH.MaterialColorChanger.SetMaterialToDefault(ref _materialDatas);
        }

        public bool CheckPlaceInput(ToolHelper TH)
        {
            return TH.InputSystem.LeftHoldClickOnWorld;
        }

        private void ResetPreviousMaterial()
        {
            if (_currentChangableMaterial == null) return;
            _currentChangableMaterial.UpdateMaterial(_storedMaterial);
            _currentChangableMaterial = null;
        }
    
        private IChangableMaterial GetClosestWallMaterial(ToolHelper TH)
        {
            float lastDis = float.MaxValue;
            IChangableMaterial closestChangableMaterial = null;
            foreach (var wall in TH.DiscoData.MapData.WallDatas)
            {
                if (wall.assignedWall == null) continue;

                var dis = Vector3.Distance(TH.InputSystem.MousePosition, wall.assignedWall.transform.position);
                if (dis < lastDis)
                {
                    _closestWallData = wall;
                    closestChangableMaterial = wall.assignedWall;
                    lastDis = dis;
                }
            }

            return closestChangableMaterial;
        }
    }
}