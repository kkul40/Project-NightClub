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

        private IBuild currentBuild = null;

        private void Awake()
        {
            placingType = PlacingType.None;
        }

        private void Update()
        {
            if(currentBuild != null) 
                currentBuild.BuildUpdate();
        }

        public void ResetPlacerAndRemover()
        {
            tileIndicator.CloseTileIndicator();
            currentBuild = null;
        }
    
        public void StartPlacement<T>(T itemSo) where T : ItemSo
        {
            StopBuild();

            if (itemSo is PlacablePropSo placablePropSo)
            {
                switch (placablePropSo.placementType)
                {
                    case PlacementType.FloorProp:
                        currentBuild = transform.GetComponent<Placer>();
                        currentBuild.Setup(placablePropSo);
                        tileIndicator.SetTileIndicator(PlacingType.Direction);
                        break;
                    case PlacementType.WallProp:
                        currentBuild = transform.GetComponent<WallPropPlacer>();
                        currentBuild.Setup(placablePropSo);
                        tileIndicator.SetTileIndicator(PlacingType.Place);
                        break;
                    default:
                        Debug.LogWarning(placablePropSo.name + " is Missing Something");
                        return;
                        break;
                }
            }
            else if (itemSo is WallPaperSo wallPaperSo)
            {
                currentBuild = GetComponent<WallPaperPlacer>();
                currentBuild.Setup(wallPaperSo);
                tileIndicator.SetTileIndicator(PlacingType.Place);
            }
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
            if (currentBuild != null)
                currentBuild.Exit();
        
            tileIndicator.transform.rotation = Quaternion.identity;
        }
    
        // IRemover Section
        public void StartRemoving()
        {
            StopBuild();

            currentBuild = transform.GetComponent<Remover>();
            currentBuild.Setup(new PlacablePropSo());
        
            tileIndicator.SetTileIndicator(PlacingType.Remove);
        }
    
        private void RemovePlacedObject(Vector3Int cellPos ,GameObject placedObject)
        {
            GameData.Instance.RemovePlacementData(cellPos);
            Destroy(placedObject);
        }
    
        private GameObject GetPlacedObjectFromTile(Vector3Int cellPos)
        {
            if (GameData.Instance.placementDatas.TryGetValue(cellPos, out var placedObject))
            {
                return placedObject.Prefab;
            }
            return null;
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