using System;
using System.Collections.Generic;
using System.Linq;
using BuildingSystemFolder;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Object = UnityEngine.Object;

namespace Data
{
    public class PlacementDataHandler
    {
        private List<Prop> propList;
        private Dictionary<Vector3Int, PlacementData> floorLayerPlacements;
        private Dictionary<Vector3Int, PlacementData> surfaceLayerPlacements;

        public PlacementDataHandler()
        {
            propList = new List<Prop>();
            floorLayerPlacements = new Dictionary<Vector3Int, PlacementData>();
            surfaceLayerPlacements = new Dictionary<Vector3Int, PlacementData>();
        }

        public List<Prop> GetPropList => propList;

        public bool ContainsKey(Vector3Int cellPos, ePlacementLayer layer)
        {
            if (cellPos.x < 0 ||
                cellPos.z < 0 ||
                cellPos.x >= MapGeneratorSystem.Instance.MapSize.x ||
                cellPos.z >= MapGeneratorSystem.Instance.MapSize.y)
                return true;

            switch (layer)
            {
                case ePlacementLayer.Surface:
                    return floorLayerPlacements.ContainsKey(cellPos);
                case ePlacementLayer.Floor:
                case ePlacementLayer.Wall:
                    return surfaceLayerPlacements.ContainsKey(cellPos);
                default:
                    Debug.Log($"Placement Layer Not Added : {layer.ToString()}");
                    return true;
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
                Debug.Log(cellPos + GetKey(x, y, direction));
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

            Debug.Log(cellPos + " Cell Pos");

            foreach (var key in keys)
            {
                switch (layer)
                {
                    case ePlacementLayer.Surface:
                        floorLayerPlacements.Add(key, placementData);
                        break;
                    case ePlacementLayer.Floor:
                    case ePlacementLayer.Wall:
                        surfaceLayerPlacements.Add(key, placementData);
                        break;
                }

                Debug.Log(key);
            }

            if (placementData.SceneObject.TryGetComponent(out Prop prop)) propList.Add(prop);
            UpdateProps();
        }

        public void RemovePlacementData(Vector3Int cellPos, ePlacementLayer layer)
        {
            var placementData = new PlacementData();
            switch (layer)
            {
                case ePlacementLayer.Surface:
                    if (floorLayerPlacements.ContainsKey(cellPos))
                    {
                        placementData = floorLayerPlacements[cellPos];
                        floorLayerPlacements.Remove(cellPos);
                    }

                    break;
                case ePlacementLayer.Floor:
                case ePlacementLayer.Wall:
                    if (surfaceLayerPlacements.ContainsKey(cellPos))
                    {
                        placementData = surfaceLayerPlacements[cellPos];
                        surfaceLayerPlacements.Remove(cellPos);
                    }

                    break;
            }

            var go = placementData.SceneObject;
            if (go.TryGetComponent(out Prop prop)) propList.Remove(prop);
            Object.Destroy(go);
            UpdateProps();
        }

        private void UpdateProps()
        {
            foreach (var prop in propList)
                if (prop.transform.TryGetComponent(out IPropUpdate propUpdate))
                    propUpdate.PropUpdate();
        }

        public List<PlacementData> GetPlacementData(ePlacementLayer layer)
        {
            switch (layer)
            {
                case ePlacementLayer.Surface:
                    return floorLayerPlacements.Values.ToList();

                    break;
                case ePlacementLayer.Floor:
                case ePlacementLayer.Wall:
                    return surfaceLayerPlacements.Values.ToList();
                    break;
                default:
                    Debug.LogError(layer.ToString() + " Is Missing");
                    return new List<PlacementData>();
                    break;
            }
        }
    }

    public enum ePlacementLayer
    {
        Surface,
        Floor,
        Wall,
    }
}