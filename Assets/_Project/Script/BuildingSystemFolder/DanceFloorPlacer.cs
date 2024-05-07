using ScriptableObjects;
using UnityEngine;

namespace BuildingSystemFolder
{
    public class DanceFloorPlacer : IBuilder
    {
        private BuildingSystem _buildingSystem => BuildingSystem.Instance;

        private DancableTileSo _dancableTile;
        private GameObject tempPrefab;
        private MeshRenderer tempMeshRenderer;

        private bool isPlacable = false;
        private Vector3Int lastCellPos = -Vector3Int.one;
        private Vector3 nextPlacableGridPos = Vector3.zero;
        private Vector3 offset = new Vector3(0, -0.5f, 0);
        
        public void Setup(PlacablePropSo placablePropSo)
        {
            if (placablePropSo is DancableTileSo placable)
            {
                _dancableTile = placable;
                tempPrefab = Object.Instantiate(placable.Prefab, Vector3.zero, Quaternion.identity);
                tempMeshRenderer = tempPrefab.GetComponent<MeshRenderer>() == null ? tempPrefab.GetComponentInChildren<MeshRenderer>() : tempPrefab.GetComponent<MeshRenderer>();
                tempPrefab.transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
                BuildingSystem.Instance.GetTileIndicator.SetTileIndicator(PlacingType.Place);
            }
        }

        public void BuildUpdate()
        {
            Vector3Int cellPos = _buildingSystem.GetMouseCellPosition();
            
            isPlacable = ValidatePosition(cellPos);
            
            if (cellPos != lastCellPos)
            {
                nextPlacableGridPos = _buildingSystem.GetCellCenterWorld(cellPos) + offset;
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

        public void Exit()
        {
            Object.Destroy(tempPrefab);
            BuildingSystem.Instance.StopBuilding();
        }
        
        protected void Place(Vector3Int CellPosition)
        {
            var newObject = Object.Instantiate(_dancableTile.Prefab, tempPrefab.transform.position, tempPrefab.transform.rotation);
            newObject.transform.SetParent(_buildingSystem.GetSceneTransformContainer().PropHolderTransform);

            if (newObject.TryGetComponent(out Prop prop))
            {
                prop.Initialize(_dancableTile, CellPosition, Direction.Up);
            }
            
            GameData.Instance.PlacementHandler.AddPlacementData(CellPosition, new PlacementData(_dancableTile, newObject, Vector2Int.one, Direction.Up));
            
            lastCellPos = -Vector3Int.one;
        }
        
        private void SetMaterialsColor(bool isCellPosValid)
        {
            Material placementMaterial = isCellPosValid ? _buildingSystem.blueMaterial : _buildingSystem.redMaterial;
            tempMeshRenderer.material = placementMaterial;
        }
        
        public bool ValidatePosition(Vector3Int cellPos)
        {
            if (GameData.Instance.PlacementHandler.ContainsKey(cellPos))
            {
                return false;
            }
            return true;
        }
    }
}