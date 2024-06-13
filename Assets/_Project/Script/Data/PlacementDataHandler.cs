using System;
using System.Collections.Generic;
using System.Linq;
using BuildingSystem;
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

        public PlacementDataHandler()
        {
            propList = new List<IPropUnit>();
            surfaceLayerPlacements = new Dictionary<Vector3Int, PlacementData>();
            propLayerPlacements = new Dictionary<Vector3Int, PlacementData>();
        }

        public List<IPropUnit> GetPropList => propList;

        public bool ContainsKey(Vector3Int cellPos, ePlacementLayer layer)
        {
            if (cellPos.x < 0 ||
                cellPos.z < 0 ||
                cellPos.x >= DiscoData.Instance.mapData.CurrentMapSize.x ||
                cellPos.z >= DiscoData.Instance.mapData.CurrentMapSize.y)
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
            {
                keys.Add(cellPos + GetKey(x, y, direction));
            }

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
                        DiscoData.Instance.mapData.SetTileNodeByCellPos(cellPos).IsWalkable = true;
                        break;
                    case ePlacementLayer.Floor:
                    case ePlacementLayer.Wall:
                        propLayerPlacements.Add(key, placementData);
                        DiscoData.Instance.mapData.SetTileNodeByCellPos(cellPos).IsWalkable = false;
                        break;
                }
            }


            if (placementData.SceneObject.TryGetComponent(out IPropUnit prop))
            {
                propList.Add(prop);
                prop.Initialize(placementData.ID ,cellPos, placementData.RotationData, layer);
            }
            UpdateProps();
        }

        public void RemovePlacementData(Vector3Int cellPos, ePlacementLayer layer)
        {
            PlacementData placementData = new PlacementData();
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
                DiscoData.Instance.mapData.SetTileNodeByCellPos(cellPos).IsWalkable = true;
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

        public GameObject GetSceneObject(Vector3Int cellPos ,ePlacementLayer layer)
        {
            switch (layer)
            {
                case ePlacementLayer.Surface:
                    if(surfaceLayerPlacements.ContainsKey(cellPos))
                        return surfaceLayerPlacements[cellPos].SceneObject;
                    break;
                case ePlacementLayer.Floor:
                case ePlacementLayer.Wall:
                    if(propLayerPlacements.ContainsKey(cellPos))
                        return propLayerPlacements[cellPos].SceneObject;
                    break;
                default:
                    Debug.LogError(layer.ToString() + " Is Missing");
                    return null;
            }
            return null;
        }
    }

    public enum ePlacementLayer
    {
        Surface,
        Floor,
        Wall,
        Null,
    }
}