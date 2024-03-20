using System;
using _Project.Script.NewSystem;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance;
    
    [SerializeField] private InputSystem inputSystem;
    [SerializeField] private Grid grid;
    
    
    [SerializeField] private PlacingType placingType;
    [SerializeField] private TileIndicator tileIndicator;

    private IBuild currentBuild = null;

    private void Awake()
    {
        Instance = this;
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
    
    public void StartPlacement(PlacablePropSo placablePropSo)
    {
        StopBuild();
        
        switch (placablePropSo.placementType)
        {
            case PlacementType.FloorProp:
                currentBuild = transform.GetComponent<Placer>();
                currentBuild.Setup(placablePropSo);
                break;
            case PlacementType.WallProp:
                currentBuild = transform.GetComponent<WallPlacer>();
                currentBuild.Setup(placablePropSo);
                break;
            default:
                Debug.LogWarning(placablePropSo.name + " is Missing Something");
                return;
                break;
        }
        
        tileIndicator.SetTileIndicator(PlacingType.Place);
    }

    public Vector3Int GetMouseCellPosition()
    {
        var mousePos = inputSystem.GetMouseMapPosition();
        Vector3Int cellPos = grid.WorldToCell(mousePos);
        
        tileIndicator.SetPosition(grid.CellToWorld(cellPos));

        return cellPos;
    }

    private void StopBuild()
    {
        if (currentBuild != null)
            currentBuild.Exit();
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
    
    
    public PlacingType GetPlacingType() => placingType;
}

public enum PlacingType
{
    None,
    Place,
    Remove,
}