using ScriptableObjects;
using UnityEngine;

namespace BuildingSystemFolder
{
    public class WallPropPlacer : MonoBehaviour, IBuild
    {
        [SerializeField] private BuildingSystem _buildingSystem;
        [SerializeField] private Transform propHolder;
        [SerializeField] private LayerMask placableLayer;
        
        private Vector3 placingOffset = new Vector3(0f,0,0f);
        private Quaternion lastRotation = Quaternion.identity;

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
            }
        }

        public void BuildUpdate()
        {
            TryPlacing();
            TryRotating();
        }

        public void Exit()
        {
            Destroy(tempPrefab);
            BuildingSystem.Instance.ResetPlacerAndRemover();
        }
      
        public void TryPlacing()
        {
            Vector3Int cellPos = _buildingSystem.GetMouseCellPosition();
            Vector3 mousePos = InputSystem.Instance.GetMouseMapPosition();
            Vector3Int offset = Vector3Int.up * cellPos.y; 

            var nextPlacableGridPos = _buildingSystem.GetGrid().GetCellCenterWorld(GetClosestWall(mousePos) + offset);
            Vector3Int snappedCellPos = _buildingSystem.GetGrid().WorldToCell(nextPlacableGridPos);
            
            tempPrefab.transform.position = Vector3.Lerp(tempPrefab.transform.position, nextPlacableGridPos, Time.deltaTime * _buildingSystem.GetObjectPlacingSpeed());
            
            bool isPlacable = !GameData.Instance.ValidateKey(snappedCellPos, _placablePropSo.ObjectSize);
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

            return _buildingSystem.GetGrid().WorldToCell(closestWall);
        }
        
        private void Place(Vector3Int CellPosition)
        {
            var newObject = Instantiate(_placablePropSo.Prefab, tempPrefab.transform.position, tempPrefab.transform.rotation);
            newObject.transform.SetParent(propHolder);
            
            if (newObject.TryGetComponent(out Prop prop))
            {
                prop.Initialize(_placablePropSo, CellPosition, Direction.Up);
            }
            
            GameData.Instance.AddPlacementData(CellPosition, new PlacementData(_placablePropSo, newObject));
        }
        
        private void SetMaterialsColor(bool isCellPosValid)
        {
            Material placementMaterial = isCellPosValid ? _buildingSystem.blueMaterial : _buildingSystem.redMaterial;
            tempMeshRenderer.material = placementMaterial;
        }
    }
}