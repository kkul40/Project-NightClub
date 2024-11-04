using System;
using System.Collections.Generic;
using System.Linq;
using BuildingSystem.SO;
using Data;
using PropBehaviours;
using Testing;
using Unity.Mathematics;
using UnityEngine;

namespace BuildingSystem.Builders
{
    public class MaterialBuilder : IBuildingMethod
    {
        public bool PressAndHold { get; } = true;
        public bool isFinished { get; }

        private MaterialItemSo _materialItemSo;
        private IChangableMaterial _mouseOnChangableMaterial;
        private IChangableMaterial _currentChangableMaterial;
        private Material _storedMaterial;

        private bool Placed = false;
        private quaternion _wallRotation;

        private Dictionary<Transform, MaterialColorChanger.MaterialData> _materialDatas = new();

        public void OnStart(BuildingNeedsData buildingNeedsData)
        {
            _materialDatas = new Dictionary<Transform, MaterialColorChanger.MaterialData>();
            _materialItemSo = buildingNeedsData.StoreItemSo as MaterialItemSo;

            TranspartizeOtherLayers(buildingNeedsData);
        }

        public bool OnValidate(BuildingNeedsData buildingNeedsData)
        {
            if (Placed) return false;
            return buildingNeedsData.IsCellPosInBounds();
        }

        public void OnUpdate(BuildingNeedsData buildingNeedsData)
        {
            if (!buildingNeedsData.InputSystem.HasMouseMoveToNewCell) return;
            else
                Placed = false;

            switch (_materialItemSo.MaterialLayer)
            {
                case eMaterialLayer.FloorMaterial:
                    _mouseOnChangableMaterial = FindFloorMaterial(buildingNeedsData);
                    break;
                case eMaterialLayer.WallMaterial:
                    _mouseOnChangableMaterial = GetClosestWallMaterial(buildingNeedsData);
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

                _storedMaterial = _mouseOnChangableMaterial.CurrentMaterial;
                _mouseOnChangableMaterial.UpdateMaterial(_materialItemSo.Material);
            }
        }

        public void OnPlace(BuildingNeedsData buildingNeedsData)
        {
            switch (_materialItemSo.MaterialLayer)
            {
                case eMaterialLayer.FloorMaterial:
                    var gridData = DiscoData.Instance.MapData.GetFloorGridData(buildingNeedsData.CellPosition.x, buildingNeedsData.CellPosition.z);
                    if (gridData != null)
                    {
                        gridData.AssignNewID(_materialItemSo.ID);
                        buildingNeedsData.FXCreator.CreateFX(FXType.Floor, gridData.CellPosition.CellCenterPosition(eGridType.PlacementGrid), Vector2.one, buildingNeedsData.RotationData.rotation);
                    }
                    break;
                case eMaterialLayer.WallMaterial:
                    var wallData = DiscoData.Instance.MapData.WallDatas
                        .FirstOrDefault(x => x.assignedWall as IChangableMaterial == _currentChangableMaterial);

                    if (wallData != null)
                    {
                        wallData.AssignNewID(_materialItemSo.ID);
                        buildingNeedsData.FXCreator.CreateFX(FXType.Wall, wallData.assignedWall.transform.position, Vector2.one, _wallRotation);
                    }

                    break;
            }

            Placed = true;
            _currentChangableMaterial = null;
        }

        public void OnStop(BuildingNeedsData buildingNeedsData)
        {
            ResetPreviousMaterial();
            buildingNeedsData.MaterialColorChanger.SetMaterialToDefault(ref _materialDatas);
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
            var transforms = SceneGameObjectHandler.Instance.GetExcludeTransformsByLayer(_materialItemSo.MaterialLayer);

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