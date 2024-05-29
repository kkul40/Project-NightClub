using BuildingSystem.SO;
using Data;
using UnityEngine;

namespace BuildingSystem.Builders
{
    public class PlacementBuilder : IBuildingMethod
    {
        private int FloorLayerID = 7;
        private int WalllayerID = 8;
        public bool isFinished { get; private set; }
        public Vector3 Offset { get; } = new Vector3(0f, -0.5f, 0f);

        private GameObject _tempObject;
        private MeshRenderer _tempMeshRenderer;
        private PlacementItemSO _storeItemSo;
        
        public void OnStart(BuildingNeedsData buildingNeedsData)
        {
            _storeItemSo = buildingNeedsData.StoreItemSo as PlacementItemSO;
            _tempObject = Object.Instantiate(_storeItemSo.Prefab, Vector3.zero, buildingNeedsData.RotationData.rotation);
            _tempMeshRenderer = _tempObject.GetComponentInChildren<MeshRenderer>();
        }

        public bool OnValidate(BuildingNeedsData buildingNeedsData)
        {
            Transform transform = null;
            switch (_storeItemSo.PlacementLayer)
            {
                case ePlacementLayer.Floor:
                case ePlacementLayer.Surface:
                    transform = buildingNeedsData.InputSystem.GetHitTransformWithLayer(FloorLayerID);
                    break;
                case ePlacementLayer.Wall:
                    transform = buildingNeedsData.InputSystem.GetHitTransformWithLayer(WalllayerID);
                    break;
            }
            
            if (transform == null) return false;
            
            if (buildingNeedsData.GameData.placementDataHandler.ContainsKey(buildingNeedsData.CellPosition, _storeItemSo.Size, buildingNeedsData.RotationData,
                    _storeItemSo.PlacementLayer)) return false;
            
            return true;
        }
        
        public void OnUpdate(BuildingNeedsData buildingNeedsData)
        {
            _tempObject.transform.position = Vector3.Lerp(_tempObject.transform.position, buildingNeedsData.CellCenterPosition + new Vector3(0.02f, 0.02f, 0.02f), Time.deltaTime * buildingNeedsData.MoveSpeed);
            _tempObject.transform.rotation = buildingNeedsData.RotationData.rotation;
            
            buildingNeedsData.MaterialColorChanger.SetMaterialsColor(_tempMeshRenderer, OnValidate(buildingNeedsData));
        }

        public void OnPlace(BuildingNeedsData buildingNeedsData)
        {
            var createdObject = Object.Instantiate(_storeItemSo.Prefab, buildingNeedsData.CellCenterPosition, buildingNeedsData.RotationData.rotation);
            createdObject.transform.SetParent(buildingNeedsData.SceneTransformContainer.PropHolderTransform);
            buildingNeedsData.GameData.placementDataHandler.AddPlacementData(buildingNeedsData.CellPosition, new PlacementData(_storeItemSo, createdObject, _storeItemSo.Size, buildingNeedsData.RotationData), _storeItemSo.PlacementLayer);
        }

        public void OnFinish(BuildingNeedsData buildingNeedsData)
        {
            Object.Destroy(_tempObject);
            isFinished = true;
        }
    }
}