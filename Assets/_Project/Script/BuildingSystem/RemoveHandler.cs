using System.Collections.Generic;
using BuildingSystem.Builders;
using Data;
using PropBehaviours;
using UnityEngine;

namespace BuildingSystem
{
    public class RemoveHandler : IBuildingMethod
    {
        private int FloorLayerID = 7;
        private int WalllayerID = 8;

        public bool PressAndHold { get; } = false;
        public bool isFinished { get; private set; }
        public Vector3 Offset { get; } = Vector3.zero;

        private IPropUnit _propUnit;
        private IPropUnit _lastPropUnit;

        private Vector3Int _lastCellPos = -Vector3Int.one;

        private Dictionary<Transform, MaterialColorChanger.MaterialData> _materialDatas = new();

        public void OnStart(BuildingNeedsData buildingNeedsData)
        {
            _propUnit = null;
        }

        public bool OnValidate(BuildingNeedsData buildingNeedsData)
        {
            Transform transform = DiscoData.Instance.placementDataHandler.GetPlacementObjectByCellPos(buildingNeedsData.CellPosition, ePlacementLayer.WallProp);
            if (transform == null)
                transform = DiscoData.Instance.placementDataHandler.GetPlacementObjectByCellPos(buildingNeedsData.CellPosition, ePlacementLayer.FloorProp);
            if (transform == null)
                transform = DiscoData.Instance.placementDataHandler.GetPlacementObjectByCellPos(buildingNeedsData.CellPosition, ePlacementLayer.BaseSurface);

            if (transform == null)
            {
                _propUnit = null;
                return false;
            }

            if (transform.TryGetComponent(out IPropUnit prop))
                if (buildingNeedsData.DiscoData.placementDataHandler.ContainsKey(prop.CellPosition,
                        prop.PlacementLayer))
                {
                    _propUnit = prop;
                    return true;
                }

            _propUnit = null;
            return false;
        }

        public void OnUpdate(BuildingNeedsData buildingNeedsData)
        {
            if (_lastCellPos == buildingNeedsData.CellPosition) return;
            if (!buildingNeedsData.IsCellPosInBounds())
            {
                _lastCellPos = buildingNeedsData.CellPosition;
                ResetMaterials(buildingNeedsData.MaterialColorChanger);
                return;
            }

            _lastCellPos = buildingNeedsData.CellPosition;

            OnValidate(buildingNeedsData);

            if (_propUnit == null)
            {
                ResetMaterials(buildingNeedsData.MaterialColorChanger);
                return;
            }

            if (_propUnit != _lastPropUnit)
            {
                ResetMaterials(buildingNeedsData.MaterialColorChanger);
                _lastPropUnit = _propUnit;
                buildingNeedsData.MaterialColorChanger.SetCustomMaterial(_propUnit.transform,
                    MaterialColorChanger.eMaterialColor.RemovingMaterial, ref _materialDatas);
            }
        }

        private void ResetMaterials(MaterialColorChanger materialColorChanger)
        {
            if (_lastPropUnit == null) return;

            materialColorChanger.SetMaterialToDefault(ref _materialDatas);
            _lastPropUnit = null;
        }

        public void OnPlace(BuildingNeedsData buildingNeedsData)
        {
            buildingNeedsData.DiscoData.placementDataHandler.RemovePlacement(_propUnit.CellPosition,
                _propUnit.PlacementLayer);
            ResetMaterials(buildingNeedsData.MaterialColorChanger);
            _lastCellPos = -Vector3Int.one;
        }

        public void OnStop(BuildingNeedsData buildingNeedsData)
        {
            ResetMaterials(buildingNeedsData.MaterialColorChanger);
            isFinished = true;
        }
    }
}