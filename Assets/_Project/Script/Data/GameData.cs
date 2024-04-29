using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class GameData : Singleton<GameData>
{
    private Dictionary<Vector3Int, PlacementData> placementDatas;

    [Header("Data")] 
    public List<Prop> PropList; 
    public List<Vector3> FloorMap = new ();
    public List<Wall> WallMap = new();

    private void Awake()
    {
        placementDatas = new Dictionary<Vector3Int, PlacementData>();
    }
    
    /// <summary>
    /// Returns True If Contains An Object
    /// </summary>
    /// <param name="cellPos"></param>
    /// <param name="objectSize"></param>
    /// <returns></returns>
    public bool ValidateKey(Vector3Int cellPos, Vector2Int objectSize)
    {
        //TODO Object size check
        if (placementDatas.ContainsKey(cellPos))
        {
            return true;
        }
        return false;
    }

    public void RemovePlacementData(Vector3Int cellPos)
    {
        if (ValidateKey(cellPos))
        {
            TryRemoveProp(placementDatas[cellPos].SceneObject);
            Destroy(placementDatas[cellPos].SceneObject);
            placementDatas.Remove(cellPos);
            UpdateProps();
        }
    }
    
    public void AddPlacementData(Vector3Int cellPos, PlacementData placementData)
    {
        placementDatas.Add(cellPos, placementData);
        Debug.Log("Added");
        TryAddProp(placementData.SceneObject);
        UpdateProps();
    }
    
    public bool ValidateKey(Vector3Int cellPos)
    {
        return ValidateKey(cellPos, Vector2Int.one);
    }

    private void TryAddProp(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Prop prop))
        {
            PropList.Add(prop);
        }
    }

    private void TryRemoveProp(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Prop prop))
        {
            PropList.Remove(prop);
        }
    }

    private void UpdateProps()
    {
        foreach (var prop in PropList)
        {
            if (prop.transform.TryGetComponent(out IPropUpdate propUpdate))
            {
                propUpdate.PropUpdate();
            }
        }
    }

    public GameObject GetPlacedObject(Vector3Int cellPos)
    {
        if (ValidateKey(cellPos))
        {
            return placementDatas[cellPos].SceneObject;
        }
        return null;
    }

    public List<Wall> GetWallMapPosList() => WallMap;

    public List<Prop> GetPropList() => PropList;

    public List<PlacementData> GetPlacementData()
    {
        return placementDatas.Values.ToList();
    }
}