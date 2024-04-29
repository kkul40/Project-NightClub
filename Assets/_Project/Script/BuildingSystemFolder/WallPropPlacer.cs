using ScriptableObjects;
using UnityEngine;

namespace BuildingSystemFolder
{
    public class WallPropPlacer : IBuilder
    {
        private BuildingSystem _buildingSystem => BuildingSystem.Instance;
        private LayerMask placableLayer;
        
        private Vector3 placingOffset = new Vector3(0f,0,0f);
        private Quaternion lastRotation = Quaternion.identity;

        private WallPropSo _wallPropSo;
        private GameObject tempPrefab;
        private MeshRenderer tempMeshRenderer;

        public void Setup(PlacablePropSo placablePropSo)
        {
            if (placablePropSo is WallPropSo placable)
            {
                _wallPropSo = placable;
                tempPrefab = Object.Instantiate(placable.Prefab, Vector3.zero, lastRotation);
                tempMeshRenderer = tempPrefab.GetComponent<MeshRenderer>();
            }
        }

        public void BuildUpdate()
        {
            TryPlacing();
            TryRotating();
        }

        public void Exit()
        {
            Object.Destroy(tempPrefab);
            BuildingSystem.Instance.ResetPlacerAndRemover();
        }
      
        public void TryPlacing()
        {
            Vector3Int cellPos = _buildingSystem.GetMouseCellPosition();
            Vector3 mousePos = InputSystem.Instance.GetMouseMapPosition();
            Vector3Int offset = Vector3Int.up * cellPos.y; 

            var nextPlacableGridPos = _buildingSystem.GetCellCenterWorld(GetClosestWall(mousePos) + offset);
            Vector3Int snappedCellPos = _buildingSystem.GetWorldToCell(nextPlacableGridPos);
            
            tempPrefab.transform.position = Vector3.Lerp(tempPrefab.transform.position, nextPlacableGridPos, Time.deltaTime * _buildingSystem.GetObjectPlacingSpeed());
            
            bool isPlacable = !GameData.Instance.ValidateKey(snappedCellPos, _wallPropSo.ObjectSize);
            SetMaterialsColor(isPlacable);
            if (InputSystem.Instance.LeftClickOnWorld)
            {
                if (isPlacable)
                {
                    tempPrefab.transform.position = nextPlacableGridPos;
                    Place(snappedCellPos);
                }
            }

            if (InputSystem.Instance.Esc)
            {
                Exit();
            }
        }

        public void TryRotating()
        {
            tempPrefab.transform.rotation = lastRotation;
        }

        private Vector3Int GetClosestWall(Vector3 cellPos)
        {
            float lastDis = 9999;
            Vector3 closestWall = Vector3.zero;
            foreach(var pos in GameData.Instance.GetWallMapPosList())
            {
                if (pos is WallDoor) continue;
                
                var dis = Vector3.Distance(cellPos, pos.transform.position);
                if (dis < lastDis)
                {
                    closestWall = pos.transform.position;
                    lastRotation = pos.transform.localRotation;
                    lastDis = dis;
                }
            }

            return _buildingSystem.GetWorldToCell(closestWall);
        }
        
        private void Place(Vector3Int CellPosition)
        {
            var newObject = Object.Instantiate(_wallPropSo.Prefab, tempPrefab.transform.position, tempPrefab.transform.rotation);
            newObject.transform.SetParent(_buildingSystem.GetSceneTransformContainer().PropHolderTransform);
            
            if (newObject.TryGetComponent(out Prop prop))
            {
                prop.Initialize(_wallPropSo, CellPosition, Direction.Up);
            }
            
            GameData.Instance.AddPlacementData(CellPosition, new PlacementData(_wallPropSo, newObject));
        }
        
        private void SetMaterialsColor(bool isCellPosValid)
        {
            Material placementMaterial = isCellPosValid ? _buildingSystem.blueMaterial : _buildingSystem.redMaterial;
            tempMeshRenderer.material = placementMaterial;
        }
    }
}