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

        private Dictionary<Transform, MaterialColorChanger.MaterialData> _materialDatas = new();

        public void OnStart(BuildingNeedsData buildingNeedsData)
        {
            _propUnit = null;
        }

        public bool OnValidate(BuildingNeedsData buildingNeedsData)
        {
            Transform transform = null;
            transform = buildingNeedsData.InputSystem.GetHitTransform();

            if (transform == null) return false;
            else Debug.Log(transform.name);

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
            buildingNeedsData.DiscoData.placementDataHandler.RemovePlacementData(_propUnit.CellPosition,
                _propUnit.PlacementLayer);
            ResetMaterials(buildingNeedsData.MaterialColorChanger);
        }

        public void OnStop(BuildingNeedsData buildingNeedsData)
        {
            ResetMaterials(buildingNeedsData.MaterialColorChanger);
            isFinished = true;
        }
    }
}