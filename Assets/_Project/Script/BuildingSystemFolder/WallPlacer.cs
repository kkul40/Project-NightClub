using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Script.NewSystem
{
    public class WallPlacer : MonoBehaviour, IBuild
    {
        [SerializeField] private BuildingSystem _buildingSystem;
        [SerializeField] private Grid grid;
        [SerializeField] private Transform propHolder;
        [SerializeField] private LayerMask placableLayer;
        [SerializeField] private float objectMoveSpeedMultiplier = 10;
        
        [Header("Wall Placing Materials")]
        [SerializeField] public Material redPlacement;
        [SerializeField] public Material bluePlacement;
        
        private Vector3 placingOffset = new Vector3(0f,0,0f);
        private Quaternion lastRotation = Quaternion.identity;

        private PlacablePropSo _placablePropSo;
        private GameObject tempPrefab;
        private MeshRenderer tempMeshRenderer;

        public void Setup(PlacablePropSo placablePropSo)
        {
            _placablePropSo = placablePropSo;
            tempPrefab = Instantiate(placablePropSo.Prefab, Vector3.zero, lastRotation);
            tempMeshRenderer = tempPrefab.GetComponent<MeshRenderer>();
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
            Vector3Int offset = Vector3Int.up * new Vector3Int(0, cellPos.z, 0);
            
            var nextPlacableGridPos = grid.GetCellCenterWorld(GetClosestWall(cellPos)) + offset;
            Vector3Int snappedCellPos = grid.WorldToCell(nextPlacableGridPos);
            
            tempPrefab.transform.position = Vector3.Lerp(tempPrefab.transform.position, nextPlacableGridPos, Time.deltaTime * objectMoveSpeedMultiplier);
            
            bool isValidated = GameData.Instance.ValidatePosition(snappedCellPos, _placablePropSo.ObjectSize);
            SetMaterialsColor(isValidated);
            
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (isValidated)
                {
                    tempPrefab.transform.position = nextPlacableGridPos;
                    Place(snappedCellPos);
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Exit();
            }
        }

        public void TryRotating()
        {
            tempPrefab.transform.rotation = lastRotation;
        }

        private Vector3Int GetClosestWall(Vector3Int cellPos)
        {
            var newCellPos = new Vector3Int(cellPos.x, 0, cellPos.y);
            
            float lastDis = 9999;
            Vector3 closestWall = Vector3.zero;
            foreach(var pos in GameData.Instance.GetWallMapPosList())
            {
                var dis = Vector3.Distance(newCellPos, pos.transform.position);
                if (dis < lastDis)
                {
                    closestWall = pos.transform.position;
                    lastRotation = pos.transform.localRotation;
                    lastDis = dis;
                }
            }

            return grid.WorldToCell(closestWall);
        }
        
        private void Place(Vector3Int CellPosition)
        {
            var newObject = Instantiate(_placablePropSo.Prefab, tempPrefab.transform.position, tempPrefab.transform.rotation);
            newObject.transform.SetParent(propHolder);
            GameData.Instance.AddPlacementData(CellPosition,new PlacementData(_placablePropSo, newObject));

            if (newObject.TryGetComponent(out Prop prop))
            {
                prop.Initialize(_placablePropSo, tempPrefab.transform.position);
            }
        }
        
        private void SetMaterialsColor(bool isCellPosValid)
        {
            Material placementMaterial = isCellPosValid ? bluePlacement : redPlacement;
            tempMeshRenderer.material = placementMaterial;
        }
    }
}