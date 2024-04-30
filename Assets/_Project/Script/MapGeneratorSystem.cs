using UnityEngine;

public class MapGeneratorSystem : MonoBehaviour
{
    [SerializeField] private Vector3Int initialMapSize;
    [SerializeField] private Transform floorTileHolder;
    [SerializeField] private GameObject floorTilePrefab;
    [SerializeField] private Transform wallHolder;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject wallDoorPrefab;

    private void Start()
    {
        SetUpMap();
    }

    private void SetUpMap()
    {
        int xMax = initialMapSize.x;
        int zMax = initialMapSize.z;
        
        Vector3 offset = new Vector3(0.5f, 0, 0.5f);
        for (int i = 0; i < xMax; i++)
        {
            for (int j = 0; j < zMax; j++)
            {
                // Instantiate Floor Tile
                Vector3 pos = new Vector3Int(i, 0, j) + offset;
                var newObject = Instantiate(floorTilePrefab, pos, Quaternion.identity);
                newObject.transform.SetParent(floorTileHolder);
                GameData.Instance.FloorMap.Add(pos);
                
                // Instantiate Wall
                if (j == 0)
                {
                    Vector3 pos2 = new Vector3(i + 0.5f, 0, 0);
                    var wallDoorIndex = 4;
                    if (i == wallDoorIndex)
                    {
                        var newWallDoorObject = Instantiate(wallDoorPrefab, pos2, Quaternion.identity);
                        newWallDoorObject.transform.SetParent(wallHolder);
                    }
                    else
                    {
                        var newWallObject = Instantiate(wallPrefab, pos2, Quaternion.identity);
                        newWallObject.transform.SetParent(wallHolder);
                    }
                }
                if (i == 0)
                {
                    Vector3 pos2 = new Vector3(0, 0, j + 0.5f);
                    var newWallObject = Instantiate(wallPrefab, pos2, Quaternion.Euler(0,90,0));
                    newWallObject.transform.SetParent(wallHolder);
                }
                
            }
        }
    }

    private void LoadMap()
    {
        //Load Map
    }
    public Vector3Int MapSize => initialMapSize;
}