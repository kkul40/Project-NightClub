using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

[DisallowMultipleComponent]
public class GameData : Singleton<GameData>
{
    public Dictionary<Vector3Int, PlacementData> placementDatas { get; private set; } = new();
    [Header("Data")]
    [SerializeField] private List<Prop> placedProps = new ();
    public List<Vector3> FloorMap = new ();
    public List<Vector3> Test = new ();
    public List<Wall> WallMap = new();

    private void Update()
    {
        Test.Clear();
        foreach (var t in placementDatas)
        {
            Test.Add(t.Key);
        }
    }

    public bool ValidatePosition(Vector3Int cellPos, Vector2Int objectSize)
    {
        //TODO Object size check
        if (placementDatas.ContainsKey(cellPos))
        {
            return false;
        }
        return true;
    }

    public void RemovePlacementData(Vector3Int cellPos)
    {
        if (ValidatePosition(cellPos))
        {
            placementDatas.Remove(cellPos);
        }
    }
    public void AddProp(Prop prop)
    {
        placedProps.Add(prop);
    }

    public void RemoveProp(Prop prop)
    {
        placedProps.Remove(prop);
    }
    
    public void AddPlacementData(Vector3Int cellPos, PlacementData placementData)
    {
        placementDatas.Add(cellPos, placementData);
    }
    
    private bool ValidatePosition(Vector3Int cellPos)
    {
        return ValidatePosition(cellPos, Vector2Int.one);
    }

    public List<Wall> GetWallMapPosList() => WallMap;
    public List<Prop> GetPropList() => placedProps;
}

public class PlacementData
{
    public PlacablePropSo ItemSo;
    public GameObject Prefab;

    public PlacementData(PlacablePropSo itemSo, GameObject prefab)
    {
        ItemSo = itemSo;
        Prefab = prefab;
    }
}