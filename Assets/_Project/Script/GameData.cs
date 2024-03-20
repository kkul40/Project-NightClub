using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameData : MonoBehaviour
{
    public static GameData Instance;

    public Dictionary<Vector3Int, PlacementData> placementDatas { get; private set; } = new();
    [Header("Data")]
    public List<Prop> placedProps = new ();
    public List<Vector3> FloorMap = new ();
    public List<Vector3> WallMap = new();

    private void Awake()
    {
        Instance = this;
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
    
    public void AddPlacementData(Vector3Int cellPos, PlacementData placementData)
    {
        placementDatas.Add(cellPos, placementData);
    }
    
    private bool ValidatePosition(Vector3Int cellPos)
    {
        return ValidatePosition(cellPos, Vector2Int.one);
    }

    public List<Vector3> GetWallMapPosList() => WallMap;
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
