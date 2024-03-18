using UnityEngine;

[CreateAssetMenu(fileName = "New Prop")]
public class PropSo : ItemSo
{
    public PropType PropType;
    public GameObject Prefab;
    public Vector2Int ObjectSize = Vector2Int.one;
    public PlacableType placableType;

    public LayerMask placableLayer;

    private void OnValidate()
    {
        placableLayer = 1 << (int)placableType;
    }
    
    public int GetPlacableLayer() => placableLayer;
}

public enum PropType
{
    Tile,
    Wall,
    Chair,
    Table,
    Decoration,
}