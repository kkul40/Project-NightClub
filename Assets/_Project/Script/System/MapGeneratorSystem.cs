using System.Collections;
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

        public override void Initialize()
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
            OnMapSizeChanged?.Invoke(MapSize);

            float delay = 0.05f;
            StartCoroutine(SetUpFloor(delay));
            StartCoroutine(SetUpWall(delay));
        }

        private IEnumerator SetUpFloor(float delay)
        {
            int x = 0;
            int y = 0;
            bool xDone = false;
            bool yDone = false;
            
            while (!xDone || !yDone)
            {
                yield return new WaitForSeconds(delay);
                
                if (y < MapSize.y) // Y Duzelminde Ekleme
                {
                    for (int i = 0; i <= y; i++)
                    {
                        if (i >= MapSize.x)
                            continue;
                        
                        InstantiateFloorTile(i, y);
                    }
                }
                else
                {
                    yDone = true;
                }

                if (x < MapSize.x) // X Duzleminde Ekleme
                {
                    for (int i = 0; i < y; i++)
                    {
                        if (i >= MapSize.y)
                            continue;
                        
                        InstantiateFloorTile(x, i);
                    }
                }
                else
                {
                    xDone = true;
                }
                
                x++;
                y++;
            }
        }

        private IEnumerator SetUpWall(float delay)
        {
            int x = 1;
            int y = 1;

            bool xDone = false;
            bool yDone = false;
            while (!xDone || !yDone)
            {
                yield return new WaitForSeconds(delay);

                if (x <= MapSize.x)
                {
                    if (x == MapData.WallDoorIndex)
                    {
                        var newWallDoorObject = CreateObject(wallDoorPrefab, new Vector3(x - 0.5f, 0, 0),
                            Quaternion.identity);

                        LoadAndAssignWallMaterial(new Vector3Int(MapData.WallDoorIndex, 0, 0), newWallDoorObject);

                        newWallDoorObject.transform.SetParent(SceneGameObjectHandler.Instance.GetWallHolder);
                    }
                    else
                    {
                        InstantiateXWall(x);
                    }
                }
                else
                {
                    xDone = true;
                }

                if (y <= MapSize.y)
                {
                    InstantiateYWall(y);
                }
                else
                {
                    yDone = true;
                }
                
                x++;
                y++;
            }
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
            var newWallObject = CreateObject(wallPrefab, pos2, Quaternion.Euler(0, 90, 0));
            newWallObject.transform.SetParent(SceneGameObjectHandler.Instance.GetWallHolder);
            LoadAndAssignWallMaterial(new Vector3Int(0, 0, y), newWallObject);

            return newWallObject;
        }

        private GameObject InstantiateXWall(int x)
        {
            var pos2 = new Vector3(x - 0.5f, 0, 0);
            var newWallObject = CreateObject(wallPrefab, pos2, Quaternion.identity);
            newWallObject.transform.SetParent(SceneGameObjectHandler.Instance.GetWallHolder);

            LoadAndAssignWallMaterial(new Vector3Int(x, 0, 0), newWallObject);

            return newWallObject;
        }

        private void InstantiateFloorTile(int x, int y)
        {
            var offset = new Vector3(0.5f, 0, 0.5f);
            var pos = new Vector3Int(x, 0, y);
            var newObject = CreateObject(floorTilePrefab, pos + offset, Quaternion.identity);
            newObject.transform.SetParent(SceneGameObjectHandler.Instance.GetFloorTileHolder);
            MapData.SetPathFinderNode(pos.PlacementPosToPathFinderIndex(), true, isWalkable: true);

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
        
        private GameObject CreateObject(GameObject gameObject,Vector3 pos, Quaternion quaternion)
        {
            var ob = Instantiate(gameObject, pos, quaternion);
            ob.AnimatedPlacement(pos);
            return ob;
        }
    }
}