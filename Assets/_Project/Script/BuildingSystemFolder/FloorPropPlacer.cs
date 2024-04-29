using ScriptableObjects;
using UnityEngine;

namespace BuildingSystemFolder
{
    public class FloorPropPlacer : IBuilder
    {
        private BuildingSystem _buildingSystem => BuildingSystem.Instance;
        
        private Vector3 placingOffset = new Vector3(0f,-0.5f,0f);
        private Quaternion lastRotation = Quaternion.identity;
        private Direction lastDirection = Direction.Down;

        private FloorPropSo _floorPropSo;
        private GameObject tempPrefab;
        private MeshRenderer tempMeshRenderer;

        private Vector3Int lastCellPos = -Vector3Int.one;
        private bool isPlacable = false;
        private Vector3 nextPlacableGridPos = Vector3.zero;

        public void Setup(PlacablePropSo placablePropSo)
        {
            if (placablePropSo is FloorPropSo placable)
            {
                _floorPropSo = placable;
                tempPrefab = Object.Instantiate(placable.Prefab, Vector3.zero, lastRotation);
                tempMeshRenderer = tempPrefab.GetComponent<MeshRenderer>();
                tempPrefab.transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
            }
        }

        public void BuildUpdate()
        {
            TryRotating();
            TryPlacing();
        }

        public void Exit()
        {
            Object.Destroy(tempPrefab);
            BuildingSystem.Instance.ResetPlacerAndRemover();
        }

        public void TryPlacing()
        {
            Vector3Int cellPos = _buildingSystem.GetMouseCellPosition();

            if (cellPos != lastCellPos)
            {
                nextPlacableGridPos = _buildingSystem.GetCellCenterWorld(cellPos) + placingOffset;
                isPlacable = ValidatePosition(cellPos, _floorPropSo.ObjectSize);
                SetMaterialsColor(isPlacable);
                lastCellPos = cellPos;
            }
            
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
        
        public void TryRotating()
        {
            if (InputSystem.Instance.E)
            {
                Quaternion tempQ = tempPrefab.transform.rotation;
                tempPrefab.transform.rotation = DirectionHelper.RotateClockWise(tempQ, ref lastDirection);
                lastRotation = tempPrefab.transform.rotation;
                _buildingSystem.RotateDirectionIndicator(lastRotation);
            }
            else if (InputSystem.Instance.Q)
            {
                Quaternion tempQ = tempPrefab.transform.rotation;
                tempPrefab.transform.rotation = DirectionHelper.RotateCounterClockWise(tempQ,ref lastDirection);
                lastRotation = tempPrefab.transform.rotation;
                _buildingSystem.RotateDirectionIndicator(lastRotation);
            }
        }

        protected void Place(Vector3Int CellPosition)
        {
            var newObject = Object.Instantiate(_floorPropSo.Prefab, tempPrefab.transform.position, tempPrefab.transform.rotation);
            newObject.transform.SetParent(_buildingSystem.GetSceneTransformContainer().PropHolderTransform);

            if (newObject.TryGetComponent(out Prop prop))
            {
                prop.Initialize(_floorPropSo, CellPosition, lastDirection);
            }
            
            GameData.Instance.AddPlacementData(CellPosition, new PlacementData(_floorPropSo, newObject));
            
            lastCellPos = -Vector3Int.one;
        }
        
        private void SetMaterialsColor(bool isCellPosValid)
        {
            Material placementMaterial = isCellPosValid ? _buildingSystem.blueMaterial : _buildingSystem.redMaterial;
            tempMeshRenderer.material = placementMaterial;
        }
        
        public bool ValidatePosition(Vector3Int cellPos, Vector2Int objectSize)
        {
            LayerMask hitLayer = InputSystem.Instance.GetLastHit().transform.gameObject.layer;
            if (GameData.Instance.ValidateKey(cellPos, objectSize) || hitLayer.value != 7)
            {
                return false;
            }
            return true;
        }
    }
}