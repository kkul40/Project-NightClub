using System.Collections;
using Data;
using Disco_Building;
using Disco_ScriptableObject;
using ExtensionMethods;
using PropBehaviours;
using UnityEngine;

namespace System
{
    public class MapGeneratorSystem : Singleton<MapGeneratorSystem>, ISaveLoad
    {
        [SerializeField] private GameObject floorTilePrefab;
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject wallDoorPrefab;
        public MapData MapData { get; private set; }
        public PlacementDataHandler placementDataHandler { get; private set; }

        // TODO Gereksizse kaldir
        private Vector2Int MapSize
        {
            get => MapData.CurrentMapSize;
            set => MapData.SetCurrentMapSize(this, value);
        }

        public void LoadData(GameData gameData)
        {
            MapData = new MapData(gameData);
            OnMapSizeChanged?.Invoke(MapSize);

            var delay = 0.05f;
            StartCoroutine(SetUpFloor(delay));
            StartCoroutine(SetUpWall(delay, () => placementDataHandler.LoadGameProps(gameData)));
        }

        public void SaveData(ref GameData gameData)
        {
            MapData.SaveData(ref gameData);
            placementDataHandler.SaveGameProps(ref gameData);
        }

        public override void Initialize(GameInitializer gameInitializer)
        {
            MapData = new MapData();
            placementDataHandler = new PlacementDataHandler();
        }

        public static event Action<Vector2Int> OnMapSizeChanged;


        private IEnumerator SetUpFloor(float delay, Action callBack = null)
        {
            var x = 0;
            var y = 0;
            var xDone = false;
            var yDone = false;

            while (!xDone || !yDone)
            {
                yield return new WaitForSeconds(delay);

                if (y < MapSize.y) // Y Duzelminde Ekleme
                    for (var i = 0; i <= y; i++)
                    {
                        if (i >= MapSize.x)
                            continue;

                        InstantiateFloorTile(i, y);
                    }
                else
                    yDone = true;

                if (x < MapSize.x) // X Duzleminde Ekleme
                    for (var i = 0; i < y; i++)
                    {
                        if (i >= MapSize.y)
                            continue;

                        InstantiateFloorTile(x, i);
                    }
                else
                    xDone = true;

                x++;
                y++;
            }

            callBack?.Invoke();
        }

        private IEnumerator SetUpWall(float delay, Action callBack = null)
        {
            var x = 1;
            var y = 1;

            var xDone = false;
            var yDone = false;
            while (!xDone || !yDone)
            {
                yield return new WaitForSeconds(delay);

                if (x <= MapSize.x)
                {
                    if (MapData.IsWallDoorOnX && x == MapData.WallDoorIndex)
                    {
                        var newWallDoorObject = CreateObject(wallDoorPrefab, new Vector3(x - 0.5f, 0, 0),
                            RotationData.Down.rotation, true);

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
                    if (!MapData.IsWallDoorOnX && y == MapData.WallDoorIndex)
                    {
                        var newWallDoorObject = CreateObject(wallDoorPrefab, new Vector3(0, 0, y - 0.5f),
                            RotationData.Left.rotation, true);

                        LoadAndAssignWallMaterial(new Vector3Int(0, 0, MapData.WallDoorIndex), newWallDoorObject);

                        newWallDoorObject.transform.SetParent(SceneGameObjectHandler.Instance.GetWallHolder);
                    }
                    else
                    {
                        InstantiateYWall(y);    
                    }
                }
                else
                    yDone = true;

                x++;
                y++;
            }

            callBack?.Invoke();
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
            var newWallObject = CreateObject(wallPrefab, pos2, RotationData.Left.rotation, true);
            newWallObject.transform.SetParent(SceneGameObjectHandler.Instance.GetWallHolder);
            LoadAndAssignWallMaterial(new Vector3Int(0, 0, y), newWallObject);

            return newWallObject;
        }

        private GameObject InstantiateXWall(int x)
        {
            var pos2 = new Vector3(x - 0.5f, 0, 0);
            var newWallObject = CreateObject(wallPrefab, pos2, RotationData.Down.rotation, true);
            newWallObject.transform.SetParent(SceneGameObjectHandler.Instance.GetWallHolder);

            LoadAndAssignWallMaterial(new Vector3Int(x, 0, 0), newWallObject);

            return newWallObject;
        }

        private void InstantiateFloorTile(int x, int y)
        {
            var offset = new Vector3(0.5f, 0, 0.5f);
            var pos = new Vector3Int(x, 0, y);
            var newObject = CreateObject(floorTilePrefab, pos + offset, Quaternion.identity, true);
            newObject.transform.SetParent(SceneGameObjectHandler.Instance.GetFloorTileHolder);
            MapData.Path.UpdatePathFinderNode(pos.PlacementPosToPathFinderIndex(), true, true);

            LoadAndAssignFloorTileMaterial(new Vector3Int(x, 0, y), newObject);
        }

        private void LoadAndAssignFloorTileMaterial(Vector3Int cellPosition, GameObject newObject)
        {
            var data = MapData.GetFloorGridData(cellPosition.x, cellPosition.z);
            if (data == null)
            {
                Debug.Log("Data Was NULL");
                return;
            }

            data.AssignReferance(newObject.GetComponent<FloorTile>(), cellPosition);
            
            var found = DiscoData.Instance.FindAItemByID(data.assignedMaterialID) as MaterialItemSo;
            if (found == null)
            {
                data.AssignNewID(InitConfig.Instance.GetDefaultTileMaterial);
                return;
            }
            data.AssignNewID(DiscoData.Instance.FindAItemByID(data.assignedMaterialID) as MaterialItemSo);
        }

        private void LoadAndAssignWallMaterial(Vector3Int cellPosition, GameObject newWallObject)
        {
            var data = MapData.AddNewWallData(cellPosition, newWallObject);
            
            var found = DiscoData.Instance.FindAItemByID(data.assignedMaterialID) as MaterialItemSo;
            if (found == null)
            {
                data.AssignNewID(InitConfig.Instance.GetDefaultWallMaterial);
                return;
            }
            data.AssignNewID(DiscoData.Instance.FindAItemByID(data.assignedMaterialID) as MaterialItemSo);
        }

        public GameObject CreateObject(GameObject gameObject, Vector3 pos, Quaternion quaternion, bool playAnimation)
        {
            var ob = Instantiate(gameObject, pos, quaternion);
            
            if(playAnimation)
                ob.AnimatedPlacement(pos);
            else
                ob.transform.position = pos;
            
            return ob;
        }

        public GameObject GetWallDoorPrefab => wallDoorPrefab;
        public GameObject GetWallPrefab => wallPrefab;
    }
}