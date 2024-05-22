using ScriptableObjects;
using UnityEngine;

namespace System
{
    [DisallowMultipleComponent]
    public class BuildingManager : Singleton<BuildingManager>
    {
        [SerializeField] private InputSystem inputSystem;
        [SerializeField] private GridHandler gridHandler;
        [SerializeField] private SceneTransformContainer sceneTransformContainer;
        [SerializeField] private float objectMoveSpeedMultiplier = 10f;

        [Header("Placing Materials")] [SerializeField]
        public Material redMaterial;

        [SerializeField] public Material blueMaterial;
        private IPlaceableItemData _currentPlaceableItemData;
        private Vector3 cellPos;

        private Vector3Int cellPosInt;
        private bool isPlacable;
        private Vector3 nextPlacableGridPos;
        private RotationData rotationData;
        private MeshRenderer tempMeshRenderer;

        private GameObject tempPrefab;

        public bool isPlacing => _currentPlaceableItemData != null;

        public SceneTransformContainer GetSceneTransformContainer => sceneTransformContainer;

        private void Update()
        {
            if (_currentPlaceableItemData != null)
            {
                _currentPlaceableItemData.PlacementMethod.LogicUpdate(cellPosInt, _currentPlaceableItemData,
                    rotationData);
                UpdatePlacement();
            }
        }

        public void StartPlacement(IPlaceableItemData placeableItemData)
        {
            StopPlacement();
            if (placeableItemData.Prefab == null) return;

            _currentPlaceableItemData = placeableItemData;
            tempPrefab = Instantiate(placeableItemData.Prefab, Vector3.zero, Quaternion.identity);
            tempMeshRenderer = tempPrefab.GetComponentInChildren<MeshRenderer>();
            _currentPlaceableItemData.ColorChanger.ChangeColor(tempPrefab, placeableItemData.InitialColor);
            rotationData = new RotationData();
        }

        public void StopPlacement()
        {
            if (tempPrefab != null) Destroy(tempPrefab);

            _currentPlaceableItemData = null;
            cellPosInt = Vector3Int.zero;
            cellPos = Vector3.zero;
            rotationData = new RotationData();
        }

        private void UpdatePlacement()
        {
            rotationData = _currentPlaceableItemData.RotationMethod.GetRotation(rotationData);
            cellPosInt = gridHandler.GetMouseCellPosition(inputSystem.GetMouseMapPosition());
            cellPos = gridHandler.GetCellCenterWorld(cellPosInt) + _currentPlaceableItemData.PlacementMethod.offset;

            isPlacable =
                _currentPlaceableItemData.PlacementMethod.CanPlace(cellPosInt, _currentPlaceableItemData, rotationData);
            tempPrefab.transform.position = Vector3.Lerp(tempPrefab.transform.position,
                cellPos + new Vector3(0.02f, 0, 0.02f),
                Time.deltaTime * objectMoveSpeedMultiplier);
            tempPrefab.transform.rotation = rotationData.rotation;

            SetMaterialsColor(isPlacable);

            if (InputSystem.Instance.LeftClickOnWorld && isPlacable) PlaceObject();

            if (InputSystem.Instance.Esc) StopPlacement();
        }

        private void PlaceObject()
        {
            var ob = _currentPlaceableItemData.PlacementMethod.Place(cellPosInt, cellPos, _currentPlaceableItemData,
                rotationData);
            if (ob.TryGetComponent(out Prop prop))
                prop.Initialize(_currentPlaceableItemData, cellPosInt, rotationData.direction);
            // Finalize placement logic
            // tempPrefab = null;
            // currentPlaceableItem = null;
        }

        private void SetMaterialsColor(bool canPlace)
        {
            tempMeshRenderer.material = canPlace ? blueMaterial : redMaterial;
        }
    }

    public enum PlacingType
    {
        None,
        Place,
        Direction,
        Remove
    }
}