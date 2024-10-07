using System.Collections.Generic;
using System.Linq;
using BuildingSystem.SO;
using Data;
using PropBehaviours;
using UnityEngine;

namespace BuildingSystem.Builders
{
    public class MaterialBuilder : IBuildingMethod
    {
        public bool PressAndHold { get; } = true;
        public bool isFinished { get; }

        private MaterialItemSo _materialItemSo;
        private IChangableMaterial _changableMaterial;
        private IChangableMaterial _lastChangableMaterial;
        private Material _previousMaterial;

        private Dictionary<Transform, MaterialColorChanger.MaterialData> _materialDatas = new();

        public void OnStart(BuildingNeedsData buildingNeedsData)
        {
            _materialDatas = new Dictionary<Transform, MaterialColorChanger.MaterialData>();
            _materialItemSo = buildingNeedsData.StoreItemSo as MaterialItemSo;

            TranspartizeOtherLayers(buildingNeedsData);
        }

        public bool OnValidate(BuildingNeedsData buildingNeedsData)
        {
            return buildingNeedsData.IsCellPosInBounds();
        }

        public void OnUpdate(BuildingNeedsData buildingNeedsData)
        {
            switch (_materialItemSo.MaterialLayer)
            {
                case eMaterialLayer.FloorMaterial:
                    _changableMaterial = FindMaterial(buildingNeedsData);
                    break;
                case eMaterialLayer.WallMaterial:
                    _changableMaterial = GetClosestWallMaterial(buildingNeedsData);
                    break;
            }

            if (_changableMaterial == null)
            {
                ResetPreviousMaterial();
                return;
            }

            if (_changableMaterial != _lastChangableMaterial)
            {
                ResetPreviousMaterial();
                _lastChangableMaterial = _changableMaterial;

                _previousMaterial = _changableMaterial.CurrentMaterial;
                _changableMaterial.UpdateMaterial(_materialItemSo.Material);
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
                    }
                    break;
                case eMaterialLayer.WallMaterial:
                    DiscoData.Instance.MapData.WallDatas
                        .FirstOrDefault(x => x.assignedWall as IChangableMaterial == _lastChangableMaterial)
                        .AssignNewID(_materialItemSo.ID);
                    break;
            }

            _lastChangableMaterial = null;
        }

        public void OnStop(BuildingNeedsData buildingNeedsData)
        {
            ResetPreviousMaterial();
            buildingNeedsData.MaterialColorChanger.SetMaterialToDefault(ref _materialDatas);
        }

        private IChangableMaterial FindMaterial(BuildingNeedsData buildingNeedsData)
        {
            Transform hitTransform = null;
            switch (_materialItemSo.MaterialLayer)
            {
                case eMaterialLayer.FloorMaterial:
                    hitTransform = buildingNeedsData.InputSystem.GetHitTransformWithLayer(7);
                    break;
                case eMaterialLayer.WallMaterial:
                    hitTransform = buildingNeedsData.InputSystem.GetHitTransformWithLayer(8);
                    break;
            }

            if (hitTransform != null && hitTransform.TryGetComponent(out IChangableMaterial changableMaterial))
                return changableMaterial;

            return null;
        }

        private void ResetPreviousMaterial()
        {
            if (_lastChangableMaterial == null) return;
            _lastChangableMaterial.UpdateMaterial(_previousMaterial);
            _lastChangableMaterial = null;
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

                var dis = Vector3.Distance(buildingNeedsData.InputSystem.GetMouseMapPosition(),
                    wall.assignedWall.transform.position);
                if (dis < lastDis)
                {
                    closestChangableMaterial = wall.assignedWall as IChangableMaterial;
                    lastDis = dis;
                }
            }

            return closestChangableMaterial;
        }
    }
}