using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Disco_ScriptableObject;
using DiscoSystem;
using ExtensionMethods;
using Unity.Mathematics;
using UnityEngine;

namespace Disco_Building.Builders
{
    public class MaterialBuilder : IBuildingMethod
    {
        public bool PressAndHold { get; } = true;
        public bool isFinished { get; }

        private MaterialItemSo _materialItemSo;
        private IChangableMaterial _mouseOnChangableMaterial;
        private IChangableMaterial _currentChangableMaterial;
        private MaterialItemSo _storedMaterial;

        private bool updateGuard = false;
        private bool Placed = false;
        private quaternion _wallRotation;

        private Dictionary<Transform, MaterialColorChanger.MaterialData> _materialDatas = new();

        public void OnStart(BuildingNeedsData BD)
        {
            _materialDatas = new Dictionary<Transform, MaterialColorChanger.MaterialData>();
            _materialItemSo = BD.StoreItemSo as MaterialItemSo;

            TranspartizeOtherLayers(BD);
        }

        public bool OnValidate(BuildingNeedsData BD)
        {
            if (Placed) return false;

            if (!BD.IsCellPosInBounds()) return false;
            
            if (_currentChangableMaterial == null) return false;
            
            if (_currentChangableMaterial.assignedMaterialID == _storedMaterial.ID) return false;

            return BD.IsCellPosInBounds();
        }

        public void OnUpdate(BuildingNeedsData BD)
        {
            if (!BD.IsCellPosInBounds() && updateGuard) return;

            Placed = false;

            switch (_materialItemSo.MaterialLayer)
            {
                case eMaterialLayer.FloorMaterial:
                    _mouseOnChangableMaterial = FindFloorMaterial(BD);
                    break;
                case eMaterialLayer.WallMaterial:
                    _mouseOnChangableMaterial = GetClosestWallMaterial(BD);
                    break;
            }

            if (_mouseOnChangableMaterial == null)
            {
                ResetPreviousMaterial();
                return;
            }

            if (_mouseOnChangableMaterial != _currentChangableMaterial)
            {
                ResetPreviousMaterial();
                _currentChangableMaterial = _mouseOnChangableMaterial;

                _storedMaterial = BD.DiscoData.FindAItemByID(_currentChangableMaterial.assignedMaterialID) as MaterialItemSo;
                _mouseOnChangableMaterial.UpdateMaterial(_materialItemSo);
            }

            updateGuard = true;
        }

        public void OnPlace(BuildingNeedsData BD)
        {
            switch (_materialItemSo.MaterialLayer)
            {
                case eMaterialLayer.FloorMaterial:
                    var gridData = DiscoData.Instance.MapData.GetFloorGridData(BD.CellPosition.x, BD.CellPosition.z);
                    if (gridData != null)
                    {
                        gridData.AssignNewID(_materialItemSo);
                        BD.FXCreator.CreateFX(FXType.Floor, gridData.CellPosition.CellCenterPosition(eGridType.PlacementGrid), Vector2.one, BD.RotationData.rotation);
                        gridData.assignedFloorTile.gameObject.AnimatedPlacement(ePlacementAnimationType.BouncyScaleUp);
                    }
                    break;
                case eMaterialLayer.WallMaterial:
                    var wallData = DiscoData.Instance.MapData.WallDatas
                        .FirstOrDefault(x => x.assignedWall as IChangableMaterial == _currentChangableMaterial);

                    if (wallData != null)
                    {
                        wallData.AssignNewID(_materialItemSo);
                        BD.FXCreator.CreateFX(FXType.Wall, wallData.assignedWall.transform.position, Vector2.one, _wallRotation);
                        wallData.assignedWall.gameObject.AnimatedPlacement(ePlacementAnimationType.Shaky);
                    }
                    

                    break;
            }

            Placed = true;
            _currentChangableMaterial = null;
        }

        public void OnStop(BuildingNeedsData BD)
        {
            ResetPreviousMaterial();
            BD.MaterialColorChanger.SetMaterialToDefault(ref _materialDatas);
        }

        private IChangableMaterial FindFloorMaterial(BuildingNeedsData buildingNeedsData)
        {
            Transform hitTransform = null;
            hitTransform = buildingNeedsData.InputSystem.GetHitTransformWithLayer(7);

            if (hitTransform != null && hitTransform.TryGetComponent(out IChangableMaterial changableMaterial))
                return changableMaterial;

            return null;
        }

        private void ResetPreviousMaterial()
        {
            if (_currentChangableMaterial == null) return;
            _currentChangableMaterial.UpdateMaterial(_storedMaterial);
            _currentChangableMaterial = null;
        }

        /// <summary>
        /// Turn Layers To Transparent but Selected one
        /// </summary>
        /// <param name="buildingNeedsData"></param>
        private void TranspartizeOtherLayers(BuildingNeedsData buildingNeedsData)
        {
            var transforms = SceneGameObjectHandler.Instance.GetExcludedTransformsByLayer(_materialItemSo.MaterialLayer);

            foreach (var transform in transforms)
                buildingNeedsData.MaterialColorChanger.SetCustomMaterial(transform,
                    MaterialColorChanger.eMaterialColor.TransparentMaterial, ref _materialDatas);
        }

        private IChangableMaterial GetClosestWallMaterial(BuildingNeedsData buildingNeedsData)
        {
            float lastDis = 9999;
            IChangableMaterial closestChangableMaterial = null;
            foreach (var wall in buildingNeedsData.DiscoData.MapData.WallDatas)
            {
                if (wall.assignedWall == null) continue;

                var dis = Vector3.Distance(buildingNeedsData.InputSystem.MousePosition,
                    wall.assignedWall.transform.position);
                if (dis < lastDis)
                {
                    closestChangableMaterial = wall.assignedWall as IChangableMaterial;
                    _wallRotation = wall.assignedWall.transform.rotation;
                    lastDis = dis;
                }
            }

            return closestChangableMaterial;
        }
    }
}