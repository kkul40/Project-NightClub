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
        public Vector3 Offset { get; }

        private MaterialItemSo _materialItemSo;
        private IChangableMaterial _changableMaterial;
        private IChangableMaterial _lastChangableMaterial;
        private Material _previousMaterial;

        private Dictionary<Transform, MaterialColorChanger.MaterialData> _materialDatas = new();

        public void OnStart(BuildingNeedsData buildingNeedsData)
        {
            _materialDatas = new Dictionary<Transform, MaterialColorChanger.MaterialData>();
            _materialItemSo = buildingNeedsData.StoreItemSo as MaterialItemSo;

            switch (_materialItemSo.MaterialLayer)
            {
                case eMaterialLayer.WallMaterial:
                    buildingNeedsData.MaterialColorChanger.SetCustomMaterial(
                        SceneGameObjectHandler.Instance.GetPropHolderTransform,
                        MaterialColorChanger.eMaterialColor.TransparentMaterial, ref _materialDatas);
                    return;
                case eMaterialLayer.FloorMaterial:
                    buildingNeedsData.MaterialColorChanger.SetCustomMaterial(
                        SceneGameObjectHandler.Instance.GetSurfaceHolderTransform,
                        MaterialColorChanger.eMaterialColor.TransparentMaterial, ref _materialDatas);
                    buildingNeedsData.MaterialColorChanger.SetCustomMaterial(
                        SceneGameObjectHandler.Instance.GetPropHolderTransform,
                        MaterialColorChanger.eMaterialColor.TransparentMaterial, ref _materialDatas);
                    return;
            }
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
                    DiscoData.Instance.MapData
                        .FloorGridDatas[buildingNeedsData.CellPosition.x, buildingNeedsData.CellPosition.z]
                        .AssignNewID(_materialItemSo.ID);
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