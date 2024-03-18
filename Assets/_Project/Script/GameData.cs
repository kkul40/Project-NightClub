using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameData : MonoBehaviour
{
    public static GameData Instance;

    [SerializeField] private Vector3Int initialMapSize;
    [SerializeField] private Transform floorTileHolder;
    [SerializeField] private GameObject floorTilePrefab;

    [Header("Data")] 
    public Dictionary<Vector3Int, PlacementData> placedObjects = new();
    public List<Vector3> FloorMap;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetUpMap();
    }

    private void SetUpMap()
    {
        int xMax = initialMapSize.x;
        int zMax = initialMapSize.z;
        
        Vector3 offset = floorTileHolder.transform.position; // 0.5f, 0, 0.5f
        for (int i = 0; i < xMax; i++)
        {
            for (int j = 0; j < zMax; j++)
            {
                Vector3 pos = new Vector3Int(i, 0, j) + offset;
                var newObject = Instantiate(floorTilePrefab, pos, Quaternion.identity);
                newObject.transform.SetParent(floorTileHolder);
                FloorMap.Add(pos);
            }
        }
    }

    private void LoadMap()
    {
        //Load Map
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
