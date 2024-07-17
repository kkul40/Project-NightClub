using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using BuildingSystem;
using BuildingSystem.SO;
using PropBehaviours;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Object = UnityEngine.Object;

namespace Data
{
    public class PlacementDataHandler
    {
        private List<IPropUnit> propList;
        private Dictionary<Vector3Int, PlacementData> surfaceLayerPlacements;

        private Dictionary<Vector3Int, PlacementData> propLayerPlacements;

        public List<IPropUnit> GetPropList => propList;

        public PlacementDataHandler()
        {
            propList = new List<IPropUnit>();
            surfaceLayerPlacements = new Dictionary<Vector3Int, PlacementData>();
            propLayerPlacements = new Dictionary<Vector3Int, PlacementData>();
        }

        public bool ContainsKey(Vector3Int cellPos, ePlacementLayer layer)
        {
            if (cellPos.x < 0 ||
                cellPos.z < 0 ||
                cellPos.x >= DiscoData.Instance.MapData.CurrentMapSize.x ||
                cellPos.z >= DiscoData.Instance.MapData.CurrentMapSize.y)
                return true;

            switch (layer)
            {
                case ePlacementLayer.Surface:
                    return surfaceLayerPlacements.ContainsKey(cellPos);
                case ePlacementLayer.Floor:
                case ePlacementLayer.Wall:
                    return propLayerPlacements.ContainsKey(cellPos);
                default:
                    Debug.Log($"Placement Layer Not Added : {layer.ToString()}");
                    return false;
            }
        }

        public bool ContainsKey(Vector3Int cellPos, Vector2Int size, RotationData rotationData, ePlacementLayer layer)
        {
            var keys = CalculatePosition(cellPos, size,
                rotationData.direction);

            foreach (var key in keys)
            {
                if (!ContainsKey(key, layer)) continue;
                return true;
            }

            return false;
        }

        private List<Vector3Int> CalculatePosition(Vector3Int cellPos, Vector2Int size, Direction direction)
        {
            var keys = new List<Vector3Int>();
            for (var x = 0; x < size.x; x++)
            for (var y = 0; y < size.y; y++)
                keys.Add(cellPos + GetKey(x, y, direction));

            return keys;
        }

        private Vector3Int GetKey(int x, int y, Direction direction)
        {
            var key = Vector3Int.zero;
            switch (direction)
            {
                case Direction.Up:
                    key.x = -x;
                    key.z = -y;
                    break;
                case Direction.Down:
                    key.x = x;
                    key.z = y;
                    break;
                case Direction.Left:
                    key.x = y;
                    key.z = -x;
                    break;
                case Direction.Right:
                    key.x = -y;
                    key.z = x;
                    break;
            }

            return key;
        }

        public void AddPlacementData(Vector3Int cellPos, PlacementData placementData, ePlacementLayer layer)
        {
            var keys = CalculatePosition(cellPos, placementData.Size,
                placementData.RotationData.direction);

            foreach (var key in keys)
            {
                switch (layer)
                {
                    case ePlacementLayer.Surface:
                        surfaceLayerPlacements.Add(key, placementData);
                        DiscoData.Instance.MapData.SetTileNodeByCellPos(cellPos).IsWalkable = true;
                        break;
                    case ePlacementLayer.Floor:
                    case ePlacementLayer.Wall:
                        propLayerPlacements.Add(key, placementData);
                        DiscoData.Instance.MapData.SetTileNodeByCellPos(cellPos).IsWalkable = false;
                        break;
                }
                Debug.Log("Key : " + key);
            }

            if (placementData.SceneObject.TryGetComponent(out IPropUnit prop))
            {
                propList.Add(prop);
                prop.Initialize(placementData.ID, cellPos, placementData.RotationData, layer);
            }

            UpdateProps();
        }

        public void RemovePlacementData(Vector3Int cellPos, ePlacementLayer layer)
        {
            var placementData = new PlacementData();
            switch (layer)
            {
                case ePlacementLayer.Surface:
                    if (surfaceLayerPlacements.ContainsKey(cellPos))
                    {
                        placementData = surfaceLayerPlacements[cellPos];
                        surfaceLayerPlacements.Remove(cellPos);
                    }

                    break;
                case ePlacementLayer.Floor:
                case ePlacementLayer.Wall:
                    if (propLayerPlacements.ContainsKey(cellPos))
                    {
                        placementData = propLayerPlacements[cellPos];
                        propLayerPlacements.Remove(cellPos);
                    }

                    break;
            }

            var keys = CalculatePosition(placementData.PlacedCellPos, placementData.Size,
                placementData.RotationData.direction);

            foreach (var key in keys)
            {
                switch (layer)
                {
                    case ePlacementLayer.Surface:
                        surfaceLayerPlacements.Remove(key);
                        break;
                    case ePlacementLayer.Floor:
                    case ePlacementLayer.Wall:
                        propLayerPlacements.Remove(key);
                        break;
                }

                DiscoData.Instance.MapData.SetTileNodeByCellPos(cellPos).IsWalkable = true;
            }

            DiscoData.Instance.inventory.AddItem(placementData.storeItemSo);
            var go = placementData.SceneObject;
            if (go.TryGetComponent(out IPropUnit prop)) propList.Remove(prop);
            Object.Destroy(go);
            UpdateProps();
        }

        private void UpdateProps()
        {
            foreach (var prop in propList)
                if (prop.transform.TryGetComponent(out IPropUpdate propUpdate))
                    propUpdate.PropUpdate();
        }

        public List<PlacementData> GetPlacementDatas(ePlacementLayer layer)
        {
            switch (layer)
            {
                case ePlacementLayer.Surface:
                    return surfaceLayerPlacements.Values.ToList();
                case ePlacementLayer.Floor:
                case ePlacementLayer.Wall:
                    return propLayerPlacements.Values.ToList();
                default:
                    Debug.LogError(layer.ToString() + " Is Missing");
                    return new List<PlacementData>();
            }
        }

        public GameObject GetSceneObject(Vector3Int cellPos, ePlacementLayer layer)
        {
            switch (layer)
            {
                case ePlacementLayer.Surface:
                    if (surfaceLayerPlacements.ContainsKey(cellPos))
                        return surfaceLayerPlacements[cellPos].SceneObject;
                    break;
                case ePlacementLayer.Floor:
                case ePlacementLayer.Wall:
                    if (propLayerPlacements.ContainsKey(cellPos))
                        return propLayerPlacements[cellPos].SceneObject;
                    break;
                default:
                    Debug.LogError(layer.ToString() + " Is Missing");
                    return null;
            }

            return null;
        }
        
        public void LoadGameProps(GameData gameData)
        {
            surfaceLayerPlacements.Clear();
            
            Vector3Int test = -Vector3Int.one;
            foreach (var pair in gameData.savedPropLayerPlacements)
            {
                if (test != pair.Value.PlacedCellPosition)
                {
                    test = pair.Value.PlacedCellPosition;
                    var value = pair.Value;
                    var placementItemSo = DiscoData.Instance.AllInGameItems.FirstOrDefault(x => x.ID == value.PropID) as PlacementItemSO;
                    Debug.Log("Size : " + placementItemSo.Size);
                
                    InstantiateProp(placementItemSo, value.PlacedCellPosition, new RotationData(value.EularAngles, value.Direction));
                }
            }
        }

        public void SaveGameProps(ref GameData gameData)
        {
            Debug.Log(propLayerPlacements.Count);
            gameData.savedPropLayerPlacements.Clear();
            
            Vector3Int test = -Vector3Int.one;
            foreach (var pair in propLayerPlacements)
            {
                if (test != pair.Value.PlacedCellPos)
                {
                    test = pair.Value.PlacedCellPos;
                    gameData.savedPropLayerPlacements.Add(pair.Key, new GameData.PlacementSaveData(pair.Value));
                }
            }
            
            Debug.Log(gameData.savedPropLayerPlacements.Count);
        }

        private void InstantiateProp(PlacementItemSO placementItemso, Vector3Int cellPosition, RotationData rotationData) 
        {
            var createdObject = Object.Instantiate(placementItemso.Prefab,
                GridHandler.Instance.GetCellCenterWorld(cellPosition) +
                (placementItemso.PlacementLayer == ePlacementLayer.Floor
                    ? ConstantVariables.PropObjectOffset
                    : ConstantVariables.WallObjectOffset),
                rotationData.rotation);

            createdObject.transform.SetParent(SceneGameObjectHandler.Instance.GetSurfaceHolderTransform);

            PlacementData placementData = new PlacementData(placementItemso, cellPosition, createdObject, placementItemso.Size, rotationData);
            AddPlacementData(cellPosition, placementData, placementItemso.PlacementLayer);
        }

        /// <summary>
        /// Only For Debug Purposes
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public List<Vector3Int> GetUsedKeys(ePlacementLayer layer)
        {
            switch (layer)
            {
                case ePlacementLayer.Surface:
                    return surfaceLayerPlacements.Keys.ToList();
                case ePlacementLayer.Floor:
                case ePlacementLayer.Wall:
                    return propLayerPlacements.Keys.ToList();
                default:
                    Debug.LogError(layer.ToString() + " Is Missing");
                    return new List<Vector3Int>();
            }
        }
    }

    public enum ePlacementLayer
    {
        Surface,
        Floor,
        Wall,
        Null
    }

    public class NewPlacementDataHandler
    {
        public class PlacementData2
        {
            public StoreItemSO StoreItemSo;
        }

        private List<IPropUnit> propList;

        // Data - PlacedPosition
        private Dictionary<PlacementData2, Vector3Int> allPlacements;

        public NewPlacementDataHandler()
        {
            propList = new List<IPropUnit>();
            allPlacements = new Dictionary<PlacementData2, Vector3Int>();
        }

        public bool ContainsKey(Vector3Int cellPos, eFloorGridAssignmentType layer)
        {
            if (cellPos.x < 0 ||
                cellPos.z < 0 ||
                cellPos.x >= DiscoData.Instance.MapData.CurrentMapSize.x ||
                cellPos.z >= DiscoData.Instance.MapData.CurrentMapSize.y)
                return true;


            return false;
        }
    }
}