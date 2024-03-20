using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New PlacablePropSo")]
public class PlacablePropSo : ItemSo
{
    [field : SerializeField]
    public GameObject Prefab { get; private set; }
    public Vector2Int ObjectSize = Vector2Int.one;
    public PlacementType placementType;

    public GameObject GetPrefab() => Prefab;
}

public enum PlacementType
{
    FloorProp,
    WallProp,
    WallPaper,
    Tile,
}

