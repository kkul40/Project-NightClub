using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class Table : Prop
{
    public GameObject CubePrefab;

    [SerializeField] private List<MarkedArea> _markedAreas;
        
    public void Initialize(PlacablePropSo placablePropSo, Vector3 propPos, Direction direction)
    {
        base.Initialize(placablePropSo, propPos, direction);
    }
}


[Serializable]
public class MarkedArea
{
    public Vector3Int Pos;
}