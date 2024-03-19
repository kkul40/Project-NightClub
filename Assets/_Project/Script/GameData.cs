using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameData : MonoBehaviour
{
    public static GameData Instance;

    [Header("Data")] 
    public Dictionary<Vector3Int, PlacementData> placedObjects = new();
    public List<Prop> placedProps = new ();
    public List<Vector3> FloorMap = new ();

    private void Awake()
    {
        Instance = this;
    }
}

public class PlacementData
{
    public PropSo ItemSo;
    public GameObject Prefab;

    public PlacementData(PropSo itemSo, GameObject prefab)
    {
        ItemSo = itemSo;
        Prefab = prefab;
    }
}
