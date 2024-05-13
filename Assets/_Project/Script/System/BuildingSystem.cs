using BuildingSystemFolder;
using ScriptableObjects;
using UnityEngine;

namespace System
{
    [DisallowMultipleComponent]
    public class BuildingSystem : Singleton<BuildingSystem>
    {
        [SerializeField] private InputSystem inputSystem;
        [SerializeField] private GridHandler gridHandler;
        [SerializeField] private SceneTransformContainer _sceneTransformContainer;
        
        [SerializeField] private float objectMoveSpeedMultiplier = 10;
    
        [Header("Placing Materials")]
        [SerializeField] public Material redMaterial;
        [SerializeField] public Material blueMaterial;
        [SerializeField] public Material yellowMaterial;
    
        [SerializeField] private TileIndicator tileIndicator;

        private IBuilder _currentBuilder = null;

        private void Update()
        {
            _currentBuilder?.BuildUpdate();
        }

        public void StopBuilding()
        {
            tileIndicator.CloseTileIndicator();
            _currentBuilder = null;
        }
    
        public void StartPlacement(PlacablePropSo placablePropSo)
        {
            StopBuild();
            _currentBuilder = placablePropSo.GetBuilder();
            _currentBuilder.Setup(placablePropSo);
        }

        public void RotateDirectionIndicator(Quaternion quaternion)
        {
            tileIndicator.RoateDirectionIndicator(quaternion);
        }

        private void StopBuild()
        {
            if (_currentBuilder != null)
                _currentBuilder.Exit();
        
            tileIndicator.transform.rotation = Quaternion.identity;
        }
    
        // IRemover Section
        public void StartRemoving()
        {
            StopBuild();
            _currentBuilder = new Remover();
            tileIndicator.SetTileIndicator(PlacingType.Remove);
        }
    
        public PlacingType GetPlacingType => tileIndicator.placingType;
        public float GetObjectPlacingSpeed() => objectMoveSpeedMultiplier;

        public Vector3Int GetMouseCellPosition()
        {
            var cellPos = gridHandler.GetMouseCellPosition(inputSystem);
            tileIndicator.SetPosition(gridHandler.CellToWorldPosition(cellPos));
            return cellPos;
        }
        
        public Vector3 GetCellCenterWorld(Vector3Int cellPos) => gridHandler.GetCellCenterWorld(cellPos);
        public Vector3Int GetWorldToCell(Vector3 worldPos) => gridHandler.GetWorldToCell(worldPos);
        public SceneTransformContainer GetSceneTransformContainer() => _sceneTransformContainer;
        public TileIndicator GetTileIndicator => tileIndicator;
    }

    public enum PlacingType
    {
        None,
        Place,
        Direction,
        Remove,
    }
}