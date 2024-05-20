using System;
using Data;
using ScriptableObjects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BuildingSystemFolder
{
    public class FloorPropPlacer : IBuilder
    {
        private BuildingSystem _buildingSystem => BuildingSystem.Instance;
        
        private Vector3 placingOffset = new Vector3(0f,-0.5f,0f);

        private FloorPropSo _floorPropSo;
        private GameObject tempPrefab;
        private MeshRenderer tempMeshRenderer;

        private bool isPlacable = false;
        private Vector3 nextPlacableGridPos = Vector3.zero;

        private IRotater _rotater;

        public FloorPropPlacer(IRotater rotater)
        {
            _rotater = rotater;
        }

        public void Setup(PlacablePropSo placablePropSo)
        {
            if (placablePropSo is FloorPropSo placable)
            {
                _floorPropSo = placable;
                tempPrefab = Object.Instantiate(placable.Prefab, Vector3.zero, _rotater.RotationData.rotation);
                tempMeshRenderer = tempPrefab.GetComponent<MeshRenderer>() == null ? tempPrefab.GetComponentInChildren<MeshRenderer>() : tempPrefab.GetComponent<MeshRenderer>();
                tempPrefab.transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
                BuildingSystem.Instance.GetTileIndicator.SetTileIndicator(PlacingType.Direction);
                _buildingSystem.RotateDirectionIndicator(_rotater.RotationData.rotation);
            }
        }

        public void BuildUpdate()
        {
            _rotater.TryRotating(tempPrefab);
            TryPlacing();
        }

        public void Exit()
        {
            Object.Destroy(tempPrefab);
            BuildingSystem.Instance.StopBuilding();
        }

        public void TryPlacing()
        {
            Vector3Int cellPos = _buildingSystem.GetMouseCellPosition();
            
            isPlacable = ValidatePosition(cellPos);
            
            nextPlacableGridPos = _buildingSystem.GetCellCenterWorld(cellPos) + placingOffset;
            SetMaterialsColor(isPlacable);
            
            tempPrefab.transform.position = Vector3.Lerp(tempPrefab.transform.position, nextPlacableGridPos, Time.deltaTime * _buildingSystem.GetObjectPlacingSpeed());
            
            if (InputSystem.Instance.LeftClickOnWorld)
            {
                if (isPlacable)
                {
                    tempPrefab.transform.position = nextPlacableGridPos;
                    Place(cellPos);
                }
            }

            if (InputSystem.Instance.Esc)
            {
                Exit();
            }
        }
        
        protected void Place(Vector3Int CellPosition)
        {
            var newObject = Object.Instantiate(_floorPropSo.Prefab, tempPrefab.transform.position, tempPrefab.transform.rotation);
            newObject.transform.SetParent(_buildingSystem.GetSceneTransformContainer().PropHolderTransform);

            if (newObject.TryGetComponent(out Prop prop))
            {
                prop.Initialize(_floorPropSo, CellPosition, _rotater.RotationData.direction);
            }
            
            GameData.Instance.PlacementHandler.AddPlacementData(CellPosition, new PlacementData(_floorPropSo, newObject, _floorPropSo.ObjectSize, _rotater.RotationData.direction));
        }
        
        private void SetMaterialsColor(bool isCellPosValid)
        {
            Material placementMaterial = isCellPosValid ? _buildingSystem.blueMaterial : _buildingSystem.redMaterial;
            tempMeshRenderer.material = placementMaterial;
        }
        
        public bool ValidatePosition(Vector3Int cellPos)
        {
            LayerMask hitLayer = InputSystem.Instance.GetLastHit().transform.gameObject.layer;
            if (GameData.Instance.PlacementHandler.ContainsKey(cellPos, _floorPropSo.ObjectSize, _rotater.RotationData.direction) || hitLayer.value != 7)
            {
                return false;
            }
            return true;
        }
    }

    public interface IRotater
    {
        public RotationData RotationData { get; set; }
        public void TryRotating(GameObject gameObject);

        public void SaveData(RotationData rotationData)
        {
            RotationData = rotationData;
        }
    }
    
    public class InputRotationHandler360 : IRotater
    {
        public RotationData RotationData { get; set; }

        public InputRotationHandler360()
        {
            RotationData = new RotationData();
        }

        public void TryRotating(GameObject gameObject)
        {
            if (InputSystem.Instance.E)
            {
                Quaternion tempQ = gameObject.transform.rotation;
                RotationData rData = DirectionHelper.RotateClockWise(tempQ, RotationData.direction);
                
                gameObject.transform.rotation = rData.rotation;
                ((IRotater)this).SaveData(rData);
            }
            else if (InputSystem.Instance.Q)
            {
                Quaternion tempQ = gameObject.transform.rotation;
                RotationData rData = DirectionHelper.RotateCounterClockWise(tempQ, RotationData.direction);
                
                gameObject.transform.rotation = rData.rotation;
                ((IRotater)this).SaveData(rData);
            }
        }
    }

    public class InputRotationHandlerLeftAndDown : IRotater
    {
        public RotationData RotationData { get; set; }

        public InputRotationHandlerLeftAndDown()
        {
            RotationData = new RotationData();
            RotationData = DirectionHelper.RotateToDirection(Direction.Down);
        }
        
        public void TryRotating(GameObject gameObject)
        {
            if (InputSystem.Instance.E)
            {
                RotationData rData = DirectionHelper.RotateToDirection(Direction.Down);
                gameObject.transform.rotation = rData.rotation;
                
                ((IRotater)this).SaveData(rData);
            }
            else if (InputSystem.Instance.Q)
            {
                RotationData rData = DirectionHelper.RotateToDirection(Direction.Left);
                gameObject.transform.rotation = rData.rotation;
                
                ((IRotater)this).SaveData(rData);
            }
        }
    }
}