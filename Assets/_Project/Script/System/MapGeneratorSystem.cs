using Data;
using UnityEngine;

namespace System
{
    public class MapGeneratorSystem : Singleton<MapGeneratorSystem>
    {
        [SerializeField] private Vector2Int initialMapSize;
        [SerializeField] private Vector2Int mapSize;
        [SerializeField] private Transform floorTileHolder;
        [SerializeField] private GameObject floorTilePrefab;
        [SerializeField] private Transform wallHolder;
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject wallDoorPrefab;

        public Vector2Int MapSize => mapSize;

        private void Start()
        {
            SetUpMap();
        }

        private void SetUpMap()
        {
            var xMax = initialMapSize.x;
            var yMax = initialMapSize.y;

            for (var i = 0; i < xMax; i++)
            for (var j = 0; j < yMax; j++)
                InstantiateFloorTile(i, j);

            for (var i = 0; i < xMax; i++) InstantiateYWall(i);

            for (var i = 0; i < yMax; i++)
            {
                var wallDoorIndex = 4;
                if (i == wallDoorIndex)
                {
                    var newWallDoorObject = Instantiate(wallDoorPrefab, new Vector3(i + 0.5f, 0, 0), Quaternion.identity);
                    newWallDoorObject.transform.SetParent(wallHolder);

                    var wall = InstantiateXWall(i);
                    wall.SetActive(false);
                    continue;
                }

                InstantiateXWall(i);
            }

            mapSize = new Vector2Int(xMax, yMax);
        }


        private void LoadMap()
        {
            //Load Map
        }

        [ContextMenu("Expend X")]
        public void ExpendX()
        {
            InstantiateXWall(mapSize.x);

            for (var i = 0; i < mapSize.y; i++) InstantiateFloorTile(mapSize.x, i);
            mapSize.x += 1;
        }

        [ContextMenu("Expend Y")]
        public void ExpendY()
        {
            InstantiateYWall(mapSize.y);

            for (var i = 0; i < mapSize.x; i++) InstantiateFloorTile(i, mapSize.y);
            mapSize.y += 1;
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
            GameData.Instance.FloorMap.Add(pos);
        }
    }
}