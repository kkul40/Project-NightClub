using System;
using System.Collections;
using _Initializer;
using Data;
using Disco_Building;
using Disco_ScriptableObject;
using DiscoSystem.Building_System;
using DiscoSystem.Building_System.GameEvents;
using ExtensionMethods;
using PropBehaviours;
using UnityEngine;

namespace DiscoSystem
{
    public class MapGeneratorSystem : MonoBehaviour
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

        private MapData _mapData => DiscoData.Instance.MapData;

        public IEnumerator InitializeAsync()
        {
            ServiceLocator.Register(this);
            GameEvent.Subscribe<Event_ExpendMapSize>(handle => ExpendXY(handle.X, handle.Y));

            var delay = 0.05f;
            
            // SetUP Shadow Wall
            sCeiling = Instantiate(shadowCeilingPrefab).transform;
            sCeiling.SetParent(ServiceLocator.Get<SceneGameObjectHandler>().transform);
            sCeiling.position = new Vector3(-0.05f, 3.005f, -0.05f);

            sWall_X = Instantiate(shadowWall_X_Prefab).transform;
            sWall_X.SetParent(ServiceLocator.Get<SceneGameObjectHandler>().transform);
            
            sWall_Y = Instantiate(shadowWall_Y_Prefab).transform;
            sWall_Y.SetParent(ServiceLocator.Get<SceneGameObjectHandler>().transform);

            StartCoroutine(SetUpFloor(delay));
            yield return StartCoroutine(SetUpWall(delay));
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

                if (y < _mapData.CurrentMapSize.y) // Y Duzelminde Ekleme
                    for (var i = 0; i <= y; i++)
                    {
                        if (i >= _mapData.CurrentMapSize.x)
                            continue;

                        InstantiateFloorTile(i, y);
                    }
                else
                    yDone = true;

                if (x < _mapData.CurrentMapSize.x) // X Duzleminde Ekleme
                    for (var i = 0; i < y; i++)
                    {
                        if (i >= _mapData.CurrentMapSize.y)
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

                if (x <= _mapData.CurrentMapSize.x)
                {
                    if (_mapData.IsWallDoorOnX && x == _mapData.WallDoorIndex)
                    {
                        var newWallDoorObject = InstantiateXWallDoor(x);

                        LoadAndAssignWallMaterial(new Vector3Int(_mapData.WallDoorIndex, 0, 0), newWallDoorObject);
                        
                        newWallDoorObject.transform.SetParent(ServiceLocator.Get<SceneGameObjectHandler>().GetWallHolder);
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

                if (y <= _mapData.CurrentMapSize.y)
                {
                    if (!_mapData.IsWallDoorOnX && y == _mapData.WallDoorIndex)
                    {
                        var newWallDoorObject = InstantiateYWallDoor(y);

                        LoadAndAssignWallMaterial(new Vector3Int(0, 0, _mapData.WallDoorIndex), newWallDoorObject);

                        newWallDoorObject.transform.SetParent(ServiceLocator.Get<SceneGameObjectHandler>().GetWallHolder);
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

        public GameObject InstantiateYWallDoor(int y)
        {
            var newWallDoorObject = CreateObject(wallDoorPrefab, new Vector3(0, 0, y - 0.5f),
                RotationData.Left.rotation, true);
            return newWallDoorObject;
        }

        public GameObject InstantiateXWallDoor(int x)
        {
            var newWallDoorObject = CreateObject(wallDoorPrefab, new Vector3(x - 0.5f, 0, 0),
                RotationData.Down.rotation, true);
            return newWallDoorObject;
        }

        [ContextMenu("Expend X")]
        public void ExpendX()
        {
            if (!_mapData.ChangeMapSize(1, 0)) return;
            
            if (_mapData.CurrentMapSize.x> ConstantVariables.MaxMapSizeX) return;
            InstantiateXWall(_mapData.CurrentMapSize.x);
            for (var i = 0; i < _mapData.CurrentMapSize.y; i++) InstantiateFloorTile(_mapData.CurrentMapSize.x - 1, i);
        }

        [ContextMenu("Expend Y")]
        public void ExpendY()
        {
            if (!_mapData.ChangeMapSize(0, 1)) return;

            if (_mapData.CurrentMapSize.y> ConstantVariables.MaxMapSizeY) return;
            InstantiateYWall(_mapData.CurrentMapSize.y);
            for (var i = 0; i < _mapData.CurrentMapSize.x; i++) InstantiateFloorTile(i, _mapData.CurrentMapSize.y - 1);
        }

        [ContextMenu("Expend Both")]
        public void ExpendXY(int x, int y)
        {
            for (int i = 0; i < x; i++)
                ExpendX();

            for (int i = 0; i < y; i++)
                ExpendY();
        }

        public GameObject InstantiateYWall(int y)
        {
            var pos2 = new Vector3(0, 0, y - 0.5f);
            var newWallObject = CreateObject(wallPrefab, pos2, RotationData.Left.rotation, true);
            newWallObject.transform.SetParent(ServiceLocator.Get<SceneGameObjectHandler>().GetWallHolder);
            LoadAndAssignWallMaterial(new Vector3Int(0, 0, y), newWallObject);

            Vector3 scale = sCeiling.localScale;
            sCeiling.localScale = new Vector3(scale.x, 1, y + 0.05f); 
            
            scale = sWall_Y.localScale;
            sWall_Y.localScale = new Vector3(scale.x, scale.y, y);
            sWall_X.position = new Vector3(0, 0, y);

            return newWallObject;
        }

        public GameObject InstantiateXWall(int x)
        {
            var pos2 = new Vector3(x - 0.5f, 0, 0);
            var newWallObject = CreateObject(wallPrefab, pos2, RotationData.Down.rotation, true);
            newWallObject.transform.SetParent(ServiceLocator.Get<SceneGameObjectHandler>().GetWallHolder);
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
            newObject.transform.SetParent(ServiceLocator.Get<SceneGameObjectHandler>().GetFloorTileHolder);

            LoadAndAssignFloorTileMaterial(new Vector3Int(x, 0, y), newObject);
        }

        private void LoadAndAssignFloorTileMaterial(Vector3Int cellPosition, GameObject newObject)
        {
            var data = _mapData.GetFloorGridData(cellPosition.x, cellPosition.z);
            if (data == null)
            {
                Debug.Log("Data Was NULL");
                return;
            }

            data.AssignReferance(newObject.GetComponent<FloorTile>(), cellPosition);
            
            var found = GameBundle.Instance.FindAItemByID(data.assignedMaterialID) as MaterialItemSo;
            if (found == null)
            {
                data.AssignNewID(ServiceLocator.Get<InitConfig>().GetDefaultTileMaterial);
                return;
            }
            data.AssignNewID(found);
        }

        private void LoadAndAssignWallMaterial(Vector3Int cellPosition, GameObject newWallObject)
        {
            var data = _mapData.AddNewWallData(cellPosition, newWallObject);
            
            var found = GameBundle.Instance.FindAItemByID(data.AssignedMaterialID) as MaterialItemSo;
            if (found == null)
            {
                data.AssignNewID(ServiceLocator.Get<InitConfig>().GetDefaultWallMaterial);
                return;
            }
            data.AssignNewID(GameBundle.Instance.FindAItemByID(data.AssignedMaterialID) as MaterialItemSo);
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