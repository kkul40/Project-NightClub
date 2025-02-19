using System;
using System.Collections;
using Data;
using DefaultNamespace._Refactored.Event;
using Disco_Building;
using Disco_ScriptableObject;
using ExtensionMethods;
using PropBehaviours;
using UnityEngine;

namespace DiscoSystem
{
    public class MapGeneratorSystem : Singleton<MapGeneratorSystem>, ISaveLoad
    {
        [SerializeField] private GameObject floorTilePrefab;
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject wallDoorPrefab;

        [SerializeField] private GameObject shadowCeilingPrefab;
        [SerializeField] private GameObject shadowWall_X_Prefab;
        [SerializeField] private GameObject shadowWall_Y_Prefab;

        private Transform sCeiling;
        private Transform sWall_X;
        private Transform sWall_Y;
        
        
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
            KEvent_Map.TriggerMapSizeChanged(MapSize);

            var delay = 0.05f;
            
            // SetUP Shadow Wall
            sCeiling = Instantiate(shadowCeilingPrefab).transform;
            sCeiling.SetParent(SceneGameObjectHandler.Instance.transform);
            sCeiling.position = new Vector3(-0.05f, 3.005f, -0.05f);

            sWall_X = Instantiate(shadowWall_X_Prefab).transform;
            sWall_X.SetParent(SceneGameObjectHandler.Instance.transform);
            
            sWall_Y = Instantiate(shadowWall_Y_Prefab).transform;
            sWall_Y.SetParent(SceneGameObjectHandler.Instance.transform);

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

            KEvent_Map.TriggerMapSizeChanged(MapSize);
        }

        [ContextMenu("Expend Y")]
        public void ExpendY()
        {
            if (MapSize.y + 1 > ConstantVariables.MaxMapSizeY) return;
            InstantiateYWall(MapSize.y + 1);

            for (var i = 0; i < MapSize.x; i++) InstantiateFloorTile(i, MapSize.y);
            MapSize += Vector2Int.up;

            KEvent_Map.TriggerMapSizeChanged(MapSize);
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

            Vector3 scale = sCeiling.localScale;
            sCeiling.localScale = new Vector3(scale.x, 1, y + 0.05f); 
            
            scale = sWall_Y.localScale;
            sWall_Y.localScale = new Vector3(scale.x, scale.y, y);
            sWall_X.position = new Vector3(0, 0, y);

            return newWallObject;
        }

        private GameObject InstantiateXWall(int x)
        {
            var pos2 = new Vector3(x - 0.5f, 0, 0);
            var newWallObject = CreateObject(wallPrefab, pos2, RotationData.Down.rotation, true);
            newWallObject.transform.SetParent(SceneGameObjectHandler.Instance.GetWallHolder);
            LoadAndAssignWallMaterial(new Vector3Int(x, 0, 0), newWallObject);

            Vector3 scale = sCeiling.localScale;
            sCeiling.localScale = new Vector3(x + 0.05f, 1, scale.z); 
            
            scale = sWall_X.localScale;
            sWall_X.localScale = new Vector3(x, scale.y, scale.z);
            sWall_Y.position = new Vector3(x, 0, 0);
            
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
                ob.AnimatedPlacement(ePlacementAnimationType.MoveDown);
            else
                ob.transform.position = pos;
            
            return ob;
        }

        private void ExpendShadow(int scaleX, int scaleY)
        {
            sCeiling.localScale = new Vector3(scaleX - 1, 0, scaleY - 1);
            
            sWall_X.localScale = new Vector3(scaleX, 0, 0);
            sWall_X.position = new Vector3(scaleX, 0, 0);
            
            sWall_Y.localScale = new Vector3(0, 0, scaleY);
            sWall_Y.position = new Vector3(0, 0, scaleY);
        }

        public GameObject GetWallDoorPrefab => wallDoorPrefab;
        public GameObject GetWallPrefab => wallPrefab;
    }
}