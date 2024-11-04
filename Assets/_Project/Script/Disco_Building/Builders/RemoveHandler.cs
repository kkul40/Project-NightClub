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

        public void OnStart(BuildingNeedsData BD)
        {
            _propUnit = null;
        }

        public bool OnValidate(BuildingNeedsData BD)
        {
            var transform =
                DiscoData.Instance.placementDataHandler.GetPlacementObjectByCellPos(BD.CellPosition,
                    ePlacementLayer.WallProp);
            if (transform == null)
                transform = DiscoData.Instance.placementDataHandler.GetPlacementObjectByCellPos(
                    BD.CellPosition, ePlacementLayer.FloorProp);
            if (transform == null)
                transform = DiscoData.Instance.placementDataHandler.GetPlacementObjectByCellPos(
                    BD.CellPosition, ePlacementLayer.BaseSurface);

            if (transform == null)
            {
                _propUnit = null;
                return false;
            }

            if (transform.TryGetComponent(out IPropUnit prop))
                if (BD.DiscoData.placementDataHandler.ContainsKey(prop.CellPosition,
                        prop.PlacementLayer))
                {
                    _propUnit = prop;
                    return true;
                }

            _propUnit = null;
            return false;
        }

        public void OnUpdate(BuildingNeedsData BD)
        {
            if (_lastCellPos == BD.CellPosition) return;
            if (!BD.IsCellPosInBounds())
            {
                _lastCellPos = BD.CellPosition;
                ResetMaterials(BD.MaterialColorChanger);
                return;
            }

            _lastCellPos = BD.CellPosition;

            OnValidate(BD);

            if (_propUnit == null)
            {
                ResetMaterials(BD.MaterialColorChanger);
                return;
            }

            if (_propUnit != _lastPropUnit)
            {
                ResetMaterials(BD.MaterialColorChanger);
                _lastPropUnit = _propUnit;
                BD.MaterialColorChanger.SetCustomMaterial(_propUnit.transform,
                    MaterialColorChanger.eMaterialColor.RemovingMaterial, ref _materialDatas);
            }
        }

        private void ResetMaterials(MaterialColorChanger materialColorChanger)
        {
            if (_lastPropUnit == null) return;

            materialColorChanger.SetMaterialToDefault(ref _materialDatas);
            _lastPropUnit = null;
        }

        public void OnPlace(BuildingNeedsData BD)
        {
            BD.DiscoData.placementDataHandler.RemovePlacement(_propUnit.CellPosition, _propUnit.PlacementLayer);
            // buildingNeedsData.MaterialColorChanger.SetCustomMaterial(_propUnit.transform, MaterialColorChanger.eMaterialColor.RemovingMaterial);
            
            _materialDatas = new Dictionary<Transform, MaterialColorChanger.MaterialData>();
            _lastCellPos = -Vector3Int.one;
        }

        public void OnStop(BuildingNeedsData BD)
        {
            ResetMaterials(BD.MaterialColorChanger);
            isFinished = true;
        }
    }
}