using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace _Project.Script.NewSystem
{
    public class Placer : MonoBehaviour , IPlacer
    {
        [SerializeField] private InputSystem inputSystem;
        [SerializeField] private Grid grid;
        [SerializeField] private Transform propHolder;
        [SerializeField] private LayerMask placableLayer;
        [SerializeField] private float objectMoveSpeedMultiplier = 10;
        
        [Header("Placing Materials")]
        [SerializeField] private Material redPlacement;
        [SerializeField] private Material bluePlacement;
        
        private Vector3 placingOffset = new Vector3(0f,-0.5f,0f);
        private Quaternion lastRotation = Quaternion.identity;

        private PropSo _propSo;
        private GameObject tempPrefab;
        private MeshRenderer tempMeshRenderer;
        

        public virtual void StartPlacing(PropSo propSo)
        {
            _propSo = propSo;
            tempPrefab = Instantiate(propSo.Prefab, Vector3.zero, lastRotation);
            tempMeshRenderer = tempPrefab.GetComponent<MeshRenderer>();
        }

        public virtual void TryPlacing()
        {
            Vector3Int cellPos = BuildingSystem.Instance.GetMouseCellPosition(inputSystem, grid);
            
            var nextPlacableGridPos = grid.GetCellCenterWorld(cellPos) + placingOffset;
            tempPrefab.transform.position = Vector3.Lerp(tempPrefab.transform.position, nextPlacableGridPos, Time.deltaTime * objectMoveSpeedMultiplier);

            
            bool isValidated = ValidatePosition(cellPos, _propSo.ObjectSize);
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (isValidated)
                {
                    tempPrefab.transform.position = nextPlacableGridPos;
                    Place(cellPos);
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopPlacing();
            }
        }
        
        public virtual void TryRotating()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                tempPrefab.transform.Rotate(Vector3.up * 90);
                lastRotation = tempPrefab.transform.rotation;
            }
        }

        protected virtual void Place(Vector3Int CellPosition)
        {
            var newObject = Instantiate(_propSo.Prefab, tempPrefab.transform.position, tempPrefab.transform.rotation);
            newObject.transform.SetParent(propHolder);
            GameData.Instance.placedObjects.Add(CellPosition, new PlacementData(_propSo, newObject));

            if (newObject.TryGetComponent(out Prop prop))
            {
                prop.Initialize(_propSo, tempPrefab.transform.position);
            }
            else
            {
                // TODO Buna daha sonra bak ama silme
                // Prop newProp = newObject.AddComponent<Prop>();
                // newProp.Initialize(temp_propSo,temp_object.transform.position);
            }
            StopPlacing();
        }
        
        public virtual void StopPlacing()
        {
            Destroy(tempPrefab);
            BuildingSystem.Instance.ResetPlacerAndRemover();
        }
        
        protected virtual bool ValidatePosition(Vector3Int cellPos, Vector2Int objectSize)
        {
            //TODO Object size check

            LayerMask hitLayer = inputSystem.GetLastHit().transform.gameObject.layer;
            if (GameData.Instance.placedObjects.ContainsKey(cellPos) || (placableLayer.value & (1 << hitLayer)) == 0)
            {
                SetMaterialsColor(false);
                return false;
            }
            SetMaterialsColor(true);
            return true;
        }
        
        protected virtual void SetMaterialsColor(bool isCellPosValid)
        {
            Material placementMaterial = isCellPosValid ? bluePlacement : redPlacement;
            tempMeshRenderer.material = placementMaterial;
        }
    }
}