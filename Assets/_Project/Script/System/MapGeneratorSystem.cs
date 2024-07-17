using System.Linq;
using BuildingSystem;
using Data;
using PropBehaviours;
using Unity.Mathematics;
using UnityEngine;

namespace System
{
    public class MapGeneratorSystem : Singleton<MapGeneratorSystem>, ISaveLoad
    {
        //TODO Transform holder kullanmak yerine SceneContainerden ulas bunlara
        [SerializeField] private GameObject floorTilePrefab;
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject wallDoorPrefab;

        // TODO Gereksizse kaldir
        private Vector2Int MapSize
        {
            get => DiscoData.Instance.mapData.CurrentMapSize;
            set => DiscoData.Instance.mapData.CurrentMapSize = value;
        }

        public static event Action<Vector2Int> OnMapSizeChanged;

        private void Awake()
        {
            SetUpMap();
        }

        private void SetUpMap()
        {
            var xMax = MapSize.x;
            var yMax = MapSize.y;

            for (var i = 0; i < xMax; i++)
            for (var j = 0; j < yMax; j++)
                InstantiateFloorTile(i, j);

            for (var i = 1; i <= xMax; i++)
            {
                if (i == DiscoData.Instance.mapData.WallDoorIndex)
                {
                    var newWallDoorObject =
                        Instantiate(wallDoorPrefab, new Vector3(i - 0.5f, 0, 0), Quaternion.identity);

                    LoadAndAssignWallMaterial(new Vector3Int(DiscoData.Instance.mapData.WallDoorIndex, 0, 0),
                        newWallDoorObject);

                    newWallDoorObject.transform.SetParent(SceneGameObjectHandler.Instance.GetWallHolder);

                    // var wall = InstantiateXWall(i);
                    // wall.SetActive(false);

                    continue;
                }

                InstantiateXWall(i);
            }

            for (var i = 1; i <= yMax; i++) InstantiateYWall(i);

            MapSize = new Vector2Int(xMax, yMax);
            OnMapSizeChanged?.Invoke(MapSize);
        }


        [ContextMenu("Expend X")]
        public void ExpendX()
        {
            InstantiateXWall(MapSize.x + 1);

            for (var i = 0; i < MapSize.y; i++) InstantiateFloorTile(MapSize.x, i);
            MapSize += Vector2Int.right;

            OnMapSizeChanged?.Invoke(MapSize);
        }

        [ContextMenu("Expend Y")]
        public void ExpendY()
        {
            InstantiateYWall(MapSize.y + 1);

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
            var pos2 = new Vector3(0, 0, y - 0.5f);
            var newWallObject = Instantiate(wallPrefab, pos2, Quaternion.Euler(0, 90, 0));
            newWallObject.transform.SetParent(SceneGameObjectHandler.Instance.GetWallHolder);

            LoadAndAssignWallMaterial(new Vector3Int(0, 0, y), newWallObject);

            return newWallObject;
        }

        private GameObject InstantiateXWall(int x)
        {
            var pos2 = new Vector3(x - 0.5f, 0, 0);
            var newWallObject = Instantiate(wallPrefab, pos2, Quaternion.identity);
            newWallObject.transform.SetParent(SceneGameObjectHandler.Instance.GetWallHolder);

            LoadAndAssignWallMaterial(new Vector3Int(x, 0, 0), newWallObject);

            return newWallObject;
        }

        private void InstantiateFloorTile(int x, int y)
        {
            var offset = new Vector3(0.5f, 0, 0.5f);
            var pos = new Vector3Int(x, 0, y) + offset;
            var newObject = Instantiate(floorTilePrefab, pos, Quaternion.identity);
            newObject.transform.SetParent(SceneGameObjectHandler.Instance.GetFloorTileHolder);
            DiscoData.Instance.mapData.PathFinderNodes[x, y] = new PathFinderNode(true, pos, x, y);

            LoadAndAssignFloorTileMaterial(new Vector3Int(x, 0, y), newObject);
        }

        private void LoadAndAssignFloorTileMaterial(Vector3Int cellPosition, GameObject newObject)
        {
            var data = DiscoData.Instance.mapData.FloorGridDatas[cellPosition.x, cellPosition.z];
            if (data == null)
            {
                Debug.Log("Data Was NULL");
                return;
            }

            data.AssignReferance(newObject.GetComponent<FloorTile>(), cellPosition);
            data.AssignNewID(eFloorGridAssignmentType.Material, data.assignedMaterialID);
        }

        private void LoadAndAssignWallMaterial(Vector3Int cellPosition, GameObject newWallObject)
        {
            var data = DiscoData.Instance.mapData.WallDatas.Find(x => x.CellPosition == cellPosition);
            if (data == null)
            {
                Debug.Log("Data Was NULL");
                DiscoData.Instance.mapData.WallDatas.Add(new WallAssignmentData(cellPosition, -1));
                data = DiscoData.Instance.mapData.WallDatas[DiscoData.Instance.mapData.WallDatas.Count - 1];
            }

            data.AssignReferance(newWallObject.GetComponent<Wall>());
            data.AssignNewID(data.assignedMaterialID);
        }

        public void Load(GameData gameData)
        {
            throw new NotImplementedException();

            //Load Map
        }

        public void Save(ref GameData gameData)
        {
            throw new NotImplementedException();
        }
    }
}