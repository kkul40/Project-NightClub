using UnityEngine;

public class FloorTile : TileObject
{
    protected override void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }
}