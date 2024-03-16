using System.Collections.Generic;
using System.Linq;
using _Project.Script.NewSystem;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem instance;
    [SerializeField] private InputSystem _inputSystem;
    [SerializeField] private Grid _grid;
    [SerializeField] private TileIndicator TileIndicator;
    
    [Header("Data")]
    private Dictionary<Vector3Int, PlacementData> PlacedObjects = new();

    [Header("Placement Variables")] 
    [SerializeField] private PlacingType placingType;
    [SerializeField] private PropSo placingPropSo;
    [SerializeField] private GameObject placingObject;
    [SerializeField] private Vector3Int lastCell;
    [SerializeField] private Quaternion lastRotation;
    [SerializeField] private List<Material> placingMaterials;
    private Vector3 placingOffset = new Vector3(0f,-0.5f,0f);

    private void Awake()
    {
        instance = this;
        placingType = PlacingType.None;
    }

    private void Update()
    {
        if (placingType == PlacingType.None)
        {
            TileIndicator.CloseTileIndicator();
            return;
        }

        var mousePos = _inputSystem.GetMouseMapPosition();
        Vector3Int cellPos = _grid.WorldToCell(mousePos);
        Debug.Log(cellPos);
        TileIndicator.SetPosition(_grid.CellToWorld(cellPos));

        bool leftClickPressed = Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject();
        bool escapedPressed = Input.GetKeyDown(KeyCode.Escape);
        bool ePressed = Input.GetKeyDown(KeyCode.E);
        
        switch (placingType)
        {
            case PlacingType.Place:
                placingObject.transform.position = _grid.GetCellCenterWorld(cellPos) + placingOffset;

                // Her Yeni Cell Secildiginde calisacak
                if (lastCell != cellPos)
                {
                    lastCell = cellPos;
                    SetMaterialsColor(ValidatePosition(cellPos, placingPropSo.ObjectSize));
                }
        
                //TODO Inputlari Input Syteme Tasi
                if (leftClickPressed && ValidatePosition(cellPos, placingPropSo.ObjectSize))
                {
                    Place(cellPos);
                }

                switch (placingPropSo.placableType)
                {
                    case PlacableType.Wall:
                        placingObject.transform.rotation = _inputSystem.GetLastHit().transform.rotation;
                        break;
                    case PlacableType.Floor:
                        if (ePressed)
                        {
                            RotatePlacingObject();
                        }
                        break;
                }
                
                if (escapedPressed)
                {
                    StopPlacement();
                }
                break;
            case PlacingType.Remove:
                var placedObject = GetPlacedObjectFromTile(cellPos);
                if (placedObject != null)
                {
                    if (leftClickPressed)
                    {
                        RemovePlacedObject(cellPos, placedObject);
                    }
                }
                if (escapedPressed)
                {
                    placingType = PlacingType.None;
                }
                break;
        }
    }

    private void RotatePlacingObject()
    {
        placingObject.transform.Rotate(Vector3.up, 90f);
        lastRotation = placingObject.transform.rotation;
    }

    private void RemovePlacedObject(Vector3Int cellPos ,GameObject placedObject)
    {
        PlacedObjects.Remove(cellPos);
        Destroy(placedObject);
    }

    public void StartPlacement(PropSo propSo)
    {
        StopPlacement();

        placingType = PlacingType.Place;
        placingPropSo = propSo;
        var newObject = Instantiate(propSo.Prefab, Vector3.zero, propSo.placableType == PlacableType.Floor ? lastRotation : Quaternion.identity);
        placingObject = newObject;
        placingMaterials = newObject.GetComponent<MeshRenderer>().materials.ToList();
        
        TileIndicator.SetTileIndicator(placingType);
    }

    public void StartRemoving()
    {
        placingType = PlacingType.Remove;
        TileIndicator.SetTileIndicator(PlacingType.Remove);
    }

    private void Place(Vector3Int cellPos)
    {
        var newObject = Instantiate(placingPropSo.Prefab, placingObject.transform.position, placingObject.transform.rotation);
        PlacedObjects.Add(cellPos, new PlacementData(placingPropSo, newObject));
        StopPlacement();
    }

    private void StopPlacement()
    {
        placingType = PlacingType.None;
        placingPropSo = null;
        Destroy(placingObject);
        TileIndicator.CloseTileIndicator();
        lastCell = -Vector3Int.one;
    }

    private bool ValidatePosition(Vector3Int cellPos, Vector2Int objectSize)
    {
        if (PlacedObjects.ContainsKey(cellPos)) return false;

        return true;
    }
    
    private GameObject GetPlacedObjectFromTile(Vector3Int cellPos)
    {
        if (PlacedObjects.TryGetValue(cellPos, out var placedObject))
        {
            return placedObject.Prefab;
        }

        return null;
    }

    private void SetMaterialsColor(bool isCellPosValid)
    {
        Color color = new Color();
        
        if(isCellPosValid) color = Color.blue;
        else color = Color.red;
        
        foreach (var material in placingMaterials)
        {
            material.color = color;
        }
    }
}

public class PlacementData
{
    public PropSo ItemSo;
    public GameObject Prefab;

    public PlacementData(PropSo itemSo, GameObject prefab)
    {
        ItemSo = itemSo;
        Prefab = prefab;
    }
}

public enum PlacingType
{
    None,
    Place,
    Remove,
}