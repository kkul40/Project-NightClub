using System;
using System.Linq;
using Data;
using ScriptableObjects;
using UnityEngine;

namespace BuildingSystemFolder
{
    public class WallMaterialPlacer : IBuilder
    {
        private WallMaterialSo _tempWallMaterialSo;
        private IMaterial _tempMaterialObject;
        private IMaterial _lastMaterialObject;
        
        private Material[] savedMaterials;
        
        public void Setup(PlacablePropSo placablePropSo)
        {
            if (placablePropSo is WallMaterialSo wallPaperSo)
            {
                _tempWallMaterialSo = wallPaperSo;
                BuildingSystem.Instance.GetTileIndicator.SetTileIndicator(PlacingType.Place);
            }
        }

        public void BuildUpdate()
        {
            BuildingSystem.Instance.GetMouseCellPosition();
            var mousePos = InputSystem.Instance.GetMouseMapPosition();
            _tempMaterialObject = GetClosestWallMaterial(mousePos);
            
            if (_tempMaterialObject != _lastMaterialObject) // Optimization
            {
                // Reset Previous Material
                if (_lastMaterialObject != null)
                {
                    _lastMaterialObject.ResetMaterial(savedMaterials);
                    _lastMaterialObject = null;
                }
            
                // Preview Selected Material On Object
                if (_tempMaterialObject != null)
                {
                    _lastMaterialObject = _tempMaterialObject;
                    savedMaterials = _tempMaterialObject.GetCurrentMaterial();
                    _tempMaterialObject.ChangeMaterial(_tempWallMaterialSo.Material);
                }
            }
            
            if (InputSystem.Instance.LeftHoldClickOnWorld)
            {
                if (_tempMaterialObject != null)
                {
                    _lastMaterialObject = null;
                }
            }

            if (InputSystem.Instance.Esc)
            {
                Exit();
            }
        }

        public void Exit()
        {
            if (_lastMaterialObject != null)
            {
                _lastMaterialObject.ResetMaterial(savedMaterials);
                _lastMaterialObject = null;
            }
            
            BuildingSystem.Instance.StopBuilding();
        }
        
        private IMaterial GetClosestWallMaterial(Vector3 cellPos)
        {
            float lastDis = 9999;
            IMaterial closestWallMaterial = null;
            foreach(var wall in GameData.Instance.GetWallMapPosList())
            {
                var dis = Vector3.Distance(cellPos, wall.transform.position);
                if (dis < lastDis)
                {
                    closestWallMaterial = wall as IMaterial;
                    lastDis = dis;
                }
            }

            return closestWallMaterial;
        }
    }
}