using ScriptableObjects;
using UnityEngine;

namespace BuildingSystemFolder
{
    [DisallowMultipleComponent]
    public class BuildingSystem : Singleton<BuildingSystem>
    {
        [SerializeField] private InputSystem inputSystem;
        [SerializeField] private Grid grid;
        
        [SerializeField] private float objectMoveSpeedMultiplier = 10;
    
        [Header("Placing Materials")]
        [SerializeField] public Material redMaterial;
        [SerializeField] public Material blueMaterial;
        [SerializeField] public Material yellowMaterial;
    
        [SerializeField] private PlacingType placingType;
        [SerializeField] private TileIndicator tileIndicator;

        private IBuilder _currentBuilder = null;

        private void Awake()
        {
            placingType = PlacingType.None;
        }

        private void Update()
        {
            if(_currentBuilder != null) 
                _currentBuilder.BuildUpdate();
        }

        public void ResetPlacerAndRemover()
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

        public Vector3Int GetMouseCellPosition()
        {
            var mousePos = inputSystem.GetMouseMapPosition();
            Vector3Int cellPos = grid.WorldToCell(mousePos);
            tileIndicator.SetPosition(grid.CellToWorld(cellPos));
            
            return cellPos;
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

        public void PlayFX(Transform fx_Prefab ,Vector3 pos, Quaternion rotation)
        {
            var fx = Instantiate(fx_Prefab, pos, rotation);
            Destroy(fx.gameObject, 1.5f);
        }
    
        public PlacingType GetPlacingType() => placingType;
        public Grid GetGrid() => grid;
        public float GetObjectPlacingSpeed() => objectMoveSpeedMultiplier;
    }

    public enum PlacingType
    {
        None,
        Place,
        Direction,
        Remove,
    }
}