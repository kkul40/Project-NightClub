using System;
using _Project.Script.NewSystem;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance;
    [SerializeField] private PlacingType placingType;
    [SerializeField] private TileIndicator tileIndicator;

    
    [SerializeField] private IPlacer currentPlacer;
    [SerializeField] private IRemover currentRemover;

    private void Awake()
    {
        Instance = this;
        placingType = PlacingType.None;
    }

    private void Update()
    {
        UpdatePlacer();
        UpdateRemover();
    }

    private void UpdateRemover()
    {
        if (currentRemover == null) return;
        currentRemover.TryRemoving();
    }

    private void UpdatePlacer()
    {
        if (currentPlacer == null) return;
        
        currentPlacer.TryRotating();
        currentPlacer.TryPlacing();
    }

    public void ResetPlacerAndRemover()
    {
        tileIndicator.CloseTileIndicator();
        currentPlacer = null;
        currentRemover = null;
    }
    
    public void StartPlacement(PropSo propSo)
    {
        if (currentPlacer != null)
            currentPlacer.StopPlacing();
        
        currentPlacer = transform.GetComponent<Placer>();
        currentPlacer.StartPlacing(propSo);
        
        tileIndicator.SetTileIndicator(PlacingType.Place);
    }

    public Vector3Int GetMouseCellPosition(InputSystem inputSystem, Grid grid)
    {
        var mousePos = inputSystem.GetMouseMapPosition();
        Vector3Int cellPos = grid.WorldToCell(mousePos);
        tileIndicator.SetPosition(grid.CellToWorld(cellPos));

        return cellPos;
    }
    
    
    // IRemover Section
    public void StartRemoving()
    {
        if (currentRemover != null)
            currentRemover.StopRemoving();

        currentRemover = transform.GetComponent<Remover>();
        currentRemover.StartRemoving();
        
        tileIndicator.SetTileIndicator(PlacingType.Remove);
    }
    
    private void RemovePlacedObject(Vector3Int cellPos ,GameObject placedObject)
    {
        GameData.Instance.placedObjects.Remove(cellPos);
        Destroy(placedObject);
    }
    
    private GameObject GetPlacedObjectFromTile(Vector3Int cellPos)
    {
        if (GameData.Instance.placedObjects.TryGetValue(cellPos, out var placedObject))
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