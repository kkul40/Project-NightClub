using Data;
using UnityEngine;

namespace System
{
    public class MapGeneratorSystem : Singleton<MapGeneratorSystem>
    {
        [SerializeField] private Transform floorTileHolder;
        [SerializeField] private GameObject floorTilePrefab;
        [SerializeField] private Transform wallHolder;
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject wallDoorPrefab;

        private Vector2Int MapSize
        {
            get { return DiscoData.Instance.mapData.CurrentMapSize; }
            set { DiscoData.Instance.mapData.CurrentMapSize = value; }
        }

        public static event Action<Vector2Int> OnMapSizeChanged;

        private void Awake()
        {
            SetUpMap(); 
        }

        private void SetUpMap()
        {
            var xMax = DiscoData.MapData.InitialMapSize.x;
            var yMax = DiscoData.MapData.InitialMapSize.y;

            for (var i = 0; i < xMax; i++)
            for (var j = 0; j < yMax; j++)
                InstantiateFloorTile(i, j);

            for (var i = 0; i < xMax; i++) InstantiateYWall(i);

            for (var i = 0; i < yMax; i++)
            {
                var wallDoorIndex = 6;
                if (i == wallDoorIndex)
                {
                    var newWallDoorObject = Instantiate(wallDoorPrefab, new Vector3(i + 0.5f, 0, 0), Quaternion.identity);
                    newWallDoorObject.transform.SetParent(wallHolder);

                    var wall = InstantiateXWall(i);
                    wall.SetActive(false);
                    DiscoData.Instance.mapData.DoorPosition = new Vector3Int(i, 0, -1);
                    continue;
                }

                InstantiateXWall(i);
            }

            MapSize = new Vector2Int(xMax, yMax);
            OnMapSizeChanged?.Invoke(MapSize);
        }

        private void LoadMap()
        {
            //Load Map
        }

        [ContextMenu("Expend X")]
        public void ExpendX()
        {
            InstantiateXWall(MapSize.x);

            for (var i = 0; i < MapSize.y; i++) InstantiateFloorTile(MapSize.x, i);
            MapSize += Vector2Int.right;
            
            OnMapSizeChanged?.Invoke(MapSize);
        }

        [ContextMenu("Expend Y")]
        public void ExpendY()
        {
            InstantiateYWall(MapSize.y);

            for (var i = 0; i < MapSize.x; i++) InstantiateFloorTile(i, MapSize.y);
            MapSize += Vector2Int.up;

            OnMapSizeChanged?.Invoke(MapSize);
        }

        [ContextMenu("Expend Both")]
        public void ExpendXY()
        {
            ExpendX();
            ExpendY();
        }

        private GameObject InstantiateYWall(int y)
        {
            var pos2 = new Vector3(0, 0, y + 0.5f);
            var newWallObject = Instantiate(wallPrefab, pos2, Quaternion.Euler(0, 90, 0));
            newWallObject.transform.SetParent(wallHolder);
            return newWallObject;
        }

        private GameObject InstantiateXWall(int x)
        {
            var pos2 = new Vector3(x + 0.5f, 0, 0);
            var newWallObject = Instantiate(wallPrefab, pos2, Quaternion.identity);
            newWallObject.transform.SetParent(wallHolder);
            return newWallObject;
        }

        private void InstantiateFloorTile(int x, int y)
        {
            var offset = new Vector3(0.5f, 0, 0.5f);
            var pos = new Vector3Int(x, 0, y) + offset;
            var newObject = Instantiate(floorTilePrefab, pos, Quaternion.identity);
            newObject.transform.SetParent(floorTileHolder);
            DiscoData.Instance.mapData.FloorMap.Add(pos);
            DiscoData.Instance.mapData.TileNodes[x, y] = new TileNode(true, pos, x, y);
        }
    }
}