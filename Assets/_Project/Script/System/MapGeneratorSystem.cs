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
        public MapData MapData { get; private set; }
        public PlacementDataHandler placementDataHandler { get; private set; }

        [SerializeField] private GameObject floorTilePrefab;
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject wallDoorPrefab;

        private void Awake()
        {
            MapData = new MapData();
            placementDataHandler = new PlacementDataHandler();
        }

        // TODO Gereksizse kaldir
        private Vector2Int MapSize
        {
            get => MapData.CurrentMapSize;
            set => MapData.SetCurrentMapSize(this, value);
        }

        public static event Action<Vector2Int> OnMapSizeChanged;

        public void LoadData(GameData gameData)
        {
            MapData = new MapData(gameData);
            SetUpMap();
            placementDataHandler.LoadGameProps(gameData);
        }

        public void SaveData(ref GameData gameData)
        {
            MapData.SaveData(ref gameData);
            placementDataHandler.SaveGameProps(ref gameData);
        }

        private void SetUpMap()
        {
            MapSize = MapData.CurrentMapSize;

            for (var i = 0; i < MapSize.x; i++)
            for (var j = 0; j < MapSize.y; j++)
                InstantiateFloorTile(i, j);

            for (var i = 1; i <= MapSize.x; i++)
            {
                if (i == MapData.WallDoorIndex)
                {
                    var newWallDoorObject =
                        Instantiate(wallDoorPrefab, new Vector3(i - 0.5f, 0, 0), Quaternion.identity);

                    LoadAndAssignWallMaterial(new Vector3Int(MapData.WallDoorIndex, 0, 0), newWallDoorObject);

                    newWallDoorObject.transform.SetParent(SceneGameObjectHandler.Instance.GetWallHolder);

                    continue;
                }

                InstantiateXWall(i);
            }

            for (var i = 1; i <= MapSize.y; i++) InstantiateYWall(i);

            OnMapSizeChanged?.Invoke(MapSize);
        }


        [ContextMenu("Expend X")]
        public void ExpendX()
        {
            if (MapSize.x + 1 > ConstantVariables.MaxMapSizeX) return;
            InstantiateXWall(MapSize.x + 1);

            for (var i = 0; i < MapSize.y; i++) InstantiateFloorTile(MapSize.x, i);
            MapSize += Vector2Int.right;

            OnMapSizeChanged?.Invoke(MapSize);
        }

        [ContextMenu("Expend Y")]
        public void ExpendY()
        {
            if (MapSize.y + 1 > ConstantVariables.MaxMapSizeY) return;
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
            MapData.PathFinderNodes[x, y] = new PathFinderNode(true, pos, x, y);

            LoadAndAssignFloorTileMaterial(new Vector3Int(x, 0, y), newObject);
        }

        private void LoadAndAssignFloorTileMaterial(Vector3Int cellPosition, GameObject newObject)
        {
            var data = MapData.FloorGridDatas[cellPosition.x, cellPosition.z];
            if (data == null)
            {
                Debug.Log("Data Was NULL");
                return;
            }

            data.AssignReferance(newObject.GetComponent<FloorTile>(), cellPosition);
            data.AssignNewID(data.assignedMaterialID);
        }

        private void LoadAndAssignWallMaterial(Vector3Int cellPosition, GameObject newWallObject)
        {
            var data = MapData.WallDatas.Find(x => x.CellPosition == cellPosition);
            if (data == null)
            {
                Debug.Log("Data Was NULL");
                MapData.WallDatas.Add(new WallAssignmentData(cellPosition));
                data = MapData.WallDatas[MapData.WallDatas.Count - 1];
            }

            data.AssignReferance(newWallObject.GetComponent<Wall>());
            data.AssignNewID(data.assignedMaterialID);
        }
    }
}