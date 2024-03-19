using UnityEngine;

public class PropSo : ItemSo
{
    public GameObject Prefab;
    public Vector2Int ObjectSize = Vector2Int.one;
}


[CreateAssetMenu(fileName = "New FloorProp")]
public class FloorPropSo : PropSo
{
    public PropType PropType;
}

public enum PropType
{
    Tile,
    Wall,
    Chair,
    Table,
    Decoration,
}

