using System.Collections.Generic;
using System.Linq;
using BuildingSystem.SO;
using Data;
using UnityEngine;

namespace BuildingSystem.Builders
{
    public class PlacementBuilder : IBuildingMethod
    {
        public bool PressAndHold { get; }
        public bool isFinished { get; private set; }
        public Vector3 Offset { get; private set; }

        private GameObject _tempObject;
        private List<MeshRenderer> _tempMeshRenderer;
        private PlacementItemSO _storeItemSo;

        private Dictionary<Transform, MaterialColorChanger.MaterialData> _materialDatas = new();

        public void OnStart(BuildingNeedsData buildingNeedsData)
        {
            _storeItemSo = buildingNeedsData.StoreItemSo as PlacementItemSO;
            _tempObject =
                Object.Instantiate(_storeItemSo.Prefab, Vector3.zero, buildingNeedsData.RotationData.rotation);
            _tempMeshRenderer = buildingNeedsData.MaterialColorChanger.ReturnMeshRendererList(_tempObject);
            SetOffset();
            switch (_storeItemSo.PlacementLayer)
            {
                case ePlacementLayer.Surface:
                    buildingNeedsData.MaterialColorChanger.SetCustomMaterial(
                        SceneGameObjectHandler.Instance.GetPropHolderTransform,
                        MaterialColorChanger.eMaterialColor.TransparentMaterial, ref _materialDatas);
                    return;
                case ePlacementLayer.Floor:
                case ePlacementLayer.Wall:
                    // _materialColorChanger.SetMaterialTransparency(sceneGameObjectHandler.SurfaceHolderTransform);
                    return;
            }
        }

        private void SetOffset()
        {
            switch (_storeItemSo.PlacementLayer)
            {
                case ePlacementLayer.Wall:
                    Offset = ConstantVariables.WallObjectOffset;
                    break;
                case ePlacementLayer.Surface:
                case ePlacementLayer.Floor:
                    Offset = ConstantVariables.PropObjectOffset;
                    break;
            }
        }

        public bool OnValidate(BuildingNeedsData buildingNeedsData)
        {
            Transform transform = null;
            switch (_storeItemSo.PlacementLayer)
            {
                case ePlacementLayer.Floor:
                case ePlacementLayer.Surface:
                    transform = buildingNeedsData.InputSystem.GetHitTransformWithLayer(ConstantVariables.FloorLayerID);
                    break;
                case ePlacementLayer.Wall:
                    transform = buildingNeedsData.InputSystem.GetHitTransformWithLayer(ConstantVariables.WalllayerID);
                    break;
            }

            if (transform == null) return false;

            if (buildingNeedsData.DiscoData.placementDataHandler.ContainsKey(buildingNeedsData.CellPosition,
                    _storeItemSo.Size, buildingNeedsData.RotationData,
                    _storeItemSo.PlacementLayer)) return false;

            return true;
        }

        public void OnUpdate(BuildingNeedsData buildingNeedsData)
        {
            _tempObject.transform.position = Vector3.Lerp(_tempObject.transform.position,
                buildingNeedsData.CellCenterPosition + new Vector3(0.02f, 0.02f, 0.02f),
                Time.deltaTime * buildingNeedsData.MoveSpeed);
            _tempObject.transform.rotation = buildingNeedsData.RotationData.rotation;

            buildingNeedsData.MaterialColorChanger.SetMaterialsColorByValidity(_tempMeshRenderer,
                OnValidate(buildingNeedsData));
        }

        public void OnPlace(BuildingNeedsData buildingNeedsData)
        {
            var createdObject = Object.Instantiate(_storeItemSo.Prefab, buildingNeedsData.CellCenterPosition,
                buildingNeedsData.RotationData.rotation);
            switch (_storeItemSo.PlacementLayer)
            {
                case ePlacementLayer.Surface:
                    createdObject.transform.SetParent(SceneGameObjectHandler.Instance.GetSurfaceHolderTransform);
                    break;
                case ePlacementLayer.Floor:
                case ePlacementLayer.Wall:
                    createdObject.transform.SetParent(SceneGameObjectHandler.Instance.GetPropHolderTransform);
                    break;
            }

            buildingNeedsData.DiscoData.placementDataHandler.AddPlacementData(buildingNeedsData.CellPosition,
                new PlacementData(_storeItemSo, buildingNeedsData.CellPosition, createdObject, _storeItemSo.Size,
                    buildingNeedsData.RotationData), _storeItemSo.PlacementLayer);
        }

        public void OnStop(BuildingNeedsData buildingNeedsData)
        {
            Object.Destroy(_tempObject);
            isFinished = true;
            buildingNeedsData.MaterialColorChanger.SetMaterialToDefault(ref _materialDatas);
        }
    }
}