using System.Collections.Generic;
using _Project.Script.NewSystem;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem instance;
    [SerializeField] private InputSystem _inputSystem;
    [SerializeField] private Grid _grid;
    [SerializeField] private Transform propHolder;
    [SerializeField] private PlacingType placingType;
    [SerializeField] private TileIndicator tileIndicator;
    [SerializeField] private Material redPlacement;
    [SerializeField] private Material bluePlacement;
    private Vector3 placingOffset = new Vector3(0f,-0.5f,0f);
    

    [Header("Placement Variables")] 
    [SerializeField] private PropSo temp_propSo;
    [SerializeField] private GameObject temp_object;
    [SerializeField] private MeshRenderer temp_meshRenderer;
    [SerializeField] private Vector3Int lastCell;
    [SerializeField] private Quaternion lastRotation;

    private void Awake()
    {
        instance = this;
        placingType = PlacingType.None;
    }

    private void Update()
    {
        if (placingType == PlacingType.None)
        {
            tileIndicator.CloseTileIndicator();
            return;
        }

        var mousePos = _inputSystem.GetMouseMapPosition();
        Vector3Int cellPos = _grid.WorldToCell(mousePos);
        tileIndicator.SetPosition(_grid.CellToWorld(cellPos));

        bool leftClickPressed = Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject();
        bool escapedPressed = Input.GetKeyDown(KeyCode.Escape);
        bool ePressed = Input.GetKeyDown(KeyCode.E);
        
        switch (placingType)
        {
            case PlacingType.Place:
                temp_object.transform.position = _grid.GetCellCenterWorld(cellPos) + placingOffset;

                // Her Yeni Cell Secildiginde calisacak
                if (lastCell != cellPos)
                {
                    lastCell = cellPos;
                }
                //TODO Inputlari Input Syteme Tasi
                if (ValidatePosition(cellPos, temp_propSo.ObjectSize) && leftClickPressed)
                {
                    Place(cellPos);
                    break;
                }

                switch (temp_propSo.placableType)
                {
                    case PlacableType.Wall:
                        temp_object.transform.rotation = _inputSystem.GetLastHit().transform.rotation;
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
        temp_object.transform.Rotate(Vector3.up, 90f);
        lastRotation = temp_object.transform.rotation;
    }

    private void RemovePlacedObject(Vector3Int cellPos ,GameObject placedObject)
    {
        GameData.Instance.placedObjects.Remove(cellPos);
        Destroy(placedObject);
    }

    public void StartPlacement(PropSo propSo)
    {
        StopPlacement();

        temp_propSo = propSo;
        var newObject = Instantiate(propSo.Prefab, Vector3.zero, propSo.placableType == PlacableType.Floor ? lastRotation : Quaternion.identity);
        temp_object = newObject;
        temp_meshRenderer = newObject.GetComponent<MeshRenderer>();
        newObject.name = "Temp_Object";
        tileIndicator.SetTileIndicator(placingType);
        
        placingType = PlacingType.Place;
    }

    public void StartRemoving()
    {
        placingType = PlacingType.Remove;
        tileIndicator.SetTileIndicator(PlacingType.Remove);
    }

    private void Place(Vector3Int cellPos)
    {
        var newObject = Instantiate(temp_propSo.Prefab, temp_object.transform.position, temp_object.transform.rotation);
        newObject.transform.SetParent(propHolder);
        GameData.Instance.placedObjects.Add(cellPos, new PlacementData(temp_propSo, newObject));

        Prop newProp = newObject.AddComponent<Prop>();
        newProp.Initialize(temp_propSo,temp_object.transform.position);
        
        StopPlacement();
    }

    private void StopPlacement()
    {
        placingType = PlacingType.None;
        temp_propSo = null;
        Destroy(temp_object);
        tileIndicator.CloseTileIndicator();
        lastCell = -Vector3Int.one;
    }

    private bool ValidatePosition(Vector3Int cellPos, Vector2Int objectSize)
    {
        //TODO Object size check
        var hitObject = _inputSystem.GetLastHit().transform.gameObject;
        int layerMask = temp_propSo.placableLayer.value;

        if (GameData.Instance.placedObjects.ContainsKey(cellPos) || (layerMask & (1 << hitObject.layer)) == 0)
        {
            SetMaterialsColor(false);
            return false;
        }
        SetMaterialsColor(true);
        return true;
    }
    
    private GameObject GetPlacedObjectFromTile(Vector3Int cellPos)
    {
        if (GameData.Instance.placedObjects.TryGetValue(cellPos, out var placedObject))
        {
            return placedObject.Prefab;
        }
        return null;
    }

    private void SetMaterialsColor(bool isCellPosValid)
    {
        Material placementMaterial = isCellPosValid ? bluePlacement : redPlacement;
        temp_meshRenderer.material = placementMaterial;
    }
}

public enum PlacingType
{
    None,
    Place,
    Remove,
}