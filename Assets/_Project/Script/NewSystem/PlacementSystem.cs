using System;
using System.Collections.Generic;
using _Project.Script.NewSystem;
using UnityEngine;
using UnityEngine.Serialization;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator, cellIndicator;
    [FormerlySerializedAs("_inputSystem")] [SerializeField] private InputSystem2 inputSystem2;
    [SerializeField] private Grid grid;

    [SerializeField] private DataBaseSo database;
    private int selectedObjectIndex = -1;

    [SerializeField] private GameObject gridVisualization;

    private GridData floorData, furnitureData;
    private Renderer previewRendererr;

    private List<GameObject> placedObjects = new();

    private void Start()
    {
        StopPlacement();
        floorData = new GridData();
        furnitureData = new();
        previewRendererr = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartPlacement(int ID)
    {
        selectedObjectIndex = database.ObjectDatas.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"Invalid object {ID}");
            return;
        }

        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        inputSystem2.OnClicked += PlaceStructure;
        inputSystem2.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        // // if (_inputSystem.IsPointerOverUi())
        // {
        //     return;
        // }
        
        Vector3 mousePos = inputSystem2.GetMouseMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePos);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (!placementValidity) return;
        
        GameObject newObject = Instantiate(database.ObjectDatas[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        placedObjects.Add(newObject);
        
        GridData selectedData = database.ObjectDatas[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        selectedData.AddObjectAt(gridPosition,
            database.ObjectDatas[selectedObjectIndex].Size, 
            database.ObjectDatas[selectedObjectIndex].ID,
            placedObjects.Count -1
            );
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int i)
    {
        GridData selectedData = database.ObjectDatas[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.ObjectDatas[selectedObjectIndex].Size);

    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputSystem2.OnClicked -= PlaceStructure;
        inputSystem2.OnExit -= StopPlacement;
    }

    private void Update()
    {
        if (selectedObjectIndex < 0) return;
        
        Vector3 mousePos = inputSystem2.GetMouseMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePos);
        
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        previewRendererr.material.color = placementValidity ? Color.white : Color.red;
        
        mouseIndicator.transform.position = mousePos;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
