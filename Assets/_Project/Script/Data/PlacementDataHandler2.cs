using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class PlacementDataHandler2
    {
        private List<Prop> propList;
        private Dictionary<Vector3Int, PlacementData> floorLayerPlacements;
        private Dictionary<Vector3Int, PlacementData> surfaceLayerPlacements;

        public PlacementDataHandler2()
        {
            propList = new List<Prop>();
            floorLayerPlacements = new Dictionary<Vector3Int, PlacementData>();
            surfaceLayerPlacements = new Dictionary<Vector3Int, PlacementData>();
        }
        
        public bool ContainsKey(Vector3Int cellPos, PlacementLayer layer)
        {
            switch (layer)
            {
                case PlacementLayer.Floor:
                    return floorLayerPlacements.ContainsKey(cellPos);
                case PlacementLayer.Surface:
                    return surfaceLayerPlacements.ContainsKey(cellPos);
            }
            return false;
        }
        
        public bool ContainsKey(Vector3Int cellPos, PlacementData placementData, PlacementLayer layer)
        {
            List<Vector3Int> keys = GenerateKeysByObjectSize(cellPos, placementData.placableItemData.Size,
                placementData.RotationData.direction);

            bool contains = true;
            
            foreach (var key in keys)
            {
                if (!ContainsKey(key, layer))
                {
                    contains = false;
                    return false;
                }
            }
            return contains;
        }
        
        private void AddPlacementData(Vector3Int cellPos, PlacementData placementData, PlacementLayer layer)
        {
            switch (layer)
            {
                case PlacementLayer.Floor:
                    floorLayerPlacements.Add(cellPos, placementData);
                    break;
                case PlacementLayer.Surface:
                    surfaceLayerPlacements.Add(cellPos, placementData);
                    break;
            }

            if (placementData.SceneObject.TryGetComponent(out Prop prop))
            {
                propList.Add(prop);
            }
        }
        
        public void RemovePlacementData(Vector3Int cellPos, PlacementLayer layer)
        {
            PlacementData placementData = new PlacementData();
            switch (layer)
            {
                case PlacementLayer.Floor:
                    if (floorLayerPlacements.ContainsKey(cellPos))
                    {
                        placementData = floorLayerPlacements[cellPos];
                        floorLayerPlacements.Remove(cellPos);
                    }
                    break;
                case PlacementLayer.Surface:
                    if (surfaceLayerPlacements.ContainsKey(cellPos))
                    {
                        placementData = surfaceLayerPlacements[cellPos];
                        surfaceLayerPlacements.Remove(cellPos);
                    }
                    break;
            }
            
            if (placementData.SceneObject.TryGetComponent(out Prop prop))
            {
                propList.Remove(prop);
            }
        }
        
        private List<Vector3Int> GenerateKeysByObjectSize(Vector3Int cellPos, Vector2Int objectSize, Direction direction)
        {
            var keys = new List<Vector3Int>();
            keys.Add(cellPos);
            CheckHorizontalKeys(cellPos, objectSize, direction, ref keys);
            return keys;
        }
        private void CheckHorizontalKeys(Vector3Int cellPos, Vector2Int objectSize, Direction direction,
            ref List<Vector3Int> keys)
        {
            CheckVerticalKeys(cellPos, objectSize, direction, ref keys);

            for (var i = 1; i < objectSize.x; i++)
            {
                var newCellPos = -Vector3Int.one;
                switch (direction)
                {
                    case Direction.Down:
                        newCellPos = cellPos + Vector3Int.left * i;
                        break;
                    case Direction.Up:
                        newCellPos = cellPos + Vector3Int.right * i;
                        break;
                    case Direction.Left:
                        newCellPos = cellPos + Vector3Int.forward * i;
                        break;
                    case Direction.Right:
                        newCellPos = cellPos + Vector3Int.back * i;
                        break;
                }

                if (newCellPos != -Vector3Int.one)
                {
                    keys.Add(newCellPos);
                    CheckVerticalKeys(newCellPos, objectSize, direction, ref keys);
                }
            }
        }
        private void CheckVerticalKeys(Vector3Int cellPos, Vector2Int objectSize, Direction direction,
            ref List<Vector3Int> keys)
        {
            for (var i = 1; i < objectSize.y; i++)
            {
                var newCellPos = -Vector3Int.one;
                switch (direction)
                {
                    case Direction.Down:
                        newCellPos = cellPos + Vector3Int.forward * i;
                        break;
                    case Direction.Up:
                        newCellPos = cellPos + Vector3Int.back * i;
                        break;
                    case Direction.Left:
                        newCellPos = cellPos + Vector3Int.right * i;
                        break;
                    case Direction.Right:
                        newCellPos = cellPos + Vector3Int.left * i;
                        break;
                }

                if (newCellPos != -Vector3Int.one) keys.Add(newCellPos);
            }
        }
    }
    
    public enum PlacementLayer
    {
        Floor,
        Surface,
    }
}