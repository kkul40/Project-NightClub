using ScriptableObjects;
using UnityEngine;

namespace BuildingSystemFolder
{
    public class Placer : MonoBehaviour , IBuild
    {
        [SerializeField] private BuildingSystem _buildingSystem;
        [SerializeField] private Transform propHolder;
        [SerializeField] private LayerMask placableLayer;
        [SerializeField] private Transform fx_Floor;
        
        private Vector3 placingOffset = new Vector3(0f,-0.5f,0f);
        private Quaternion lastRotation = Quaternion.identity;
        private Direction lastDirection = Direction.Down;

        private PlacablePropSo _placablePropSo;
        private GameObject tempPrefab;
        private MeshRenderer tempMeshRenderer;
        
        public void Setup<T>(T itemSo) where T : ItemSo
        {
            if (itemSo is PlacablePropSo placablePropSo)
            {
                _placablePropSo = placablePropSo;
                tempPrefab = Instantiate(placablePropSo.Prefab, Vector3.zero, lastRotation);
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
            Destroy(tempPrefab);
            BuildingSystem.Instance.ResetPlacerAndRemover();
        }

        public void TryPlacing()
        {
            Vector3Int cellPos = _buildingSystem.GetMouseCellPosition();
            
            var nextPlacableGridPos = _buildingSystem.GetGrid().GetCellCenterWorld(cellPos) + placingOffset;
            tempPrefab.transform.position = Vector3.Lerp(tempPrefab.transform.position, nextPlacableGridPos, Time.deltaTime * _buildingSystem.GetObjectPlacingSpeed());

            bool isValidated = ValidatePosition(cellPos, _placablePropSo.ObjectSize, placableLayer);
            SetMaterialsColor(isValidated);
            
            if (InputSystem.Instance.LeftClickOnWorld)
            {
                if (isValidated)
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
            var newObject = Instantiate(_placablePropSo.Prefab, tempPrefab.transform.position, tempPrefab.transform.rotation);
            newObject.transform.SetParent(propHolder);
            GameData.Instance.AddPlacementData(CellPosition, new PlacementData(_placablePropSo, newObject));
            
            _buildingSystem.PlayFX(fx_Floor, tempPrefab.transform.position, tempPrefab.transform.rotation);

            if (newObject.TryGetComponent(out Prop prop))
            {
                var cellPos = _buildingSystem.GetVectorIntFromVector(tempPrefab.transform.position);
                prop.Initialize(_placablePropSo, cellPos, lastDirection);
            }
        }
        
        private void SetMaterialsColor(bool isCellPosValid)
        {
            Material placementMaterial = isCellPosValid ? _buildingSystem.blueMaterial : _buildingSystem.redMaterial;
            tempMeshRenderer.material = placementMaterial;
        }
        
        public bool ValidatePosition(Vector3Int cellPos, Vector2Int objectSize, LayerMask placableLayer)
        {
            LayerMask hitLayer = InputSystem.Instance.GetLastHit().transform.gameObject.layer;
            if (!GameData.Instance.ValidatePosition(cellPos, objectSize) || (placableLayer.value & (1 << hitLayer)) == 0)
            {
                return false;
            }
            return true;
        }
    }
}