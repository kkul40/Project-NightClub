using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

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

        public List<Prop> GetPropList
        {
            get { return propList; }
        }   

        public bool ContainsKey(Vector3Int cellPos, PlacementLayer layer)
        {
            if (cellPos.x < 0 || 
                cellPos.z < 0 || 
                cellPos.x >= MapGeneratorSystem.Instance.MapSize.x ||
                cellPos.z >= MapGeneratorSystem.Instance.MapSize.y) 
                return true;
            
            switch (layer)
            {
                case PlacementLayer.FloorLevel:
                    return floorLayerPlacements.ContainsKey(cellPos);
                case PlacementLayer.Surface:
                    return surfaceLayerPlacements.ContainsKey(cellPos);
                default:
                    Debug.Log($"Placement Layer Not Added : {layer.ToString()}");
                    return true;
            }
        }
        
        public bool ContainsKey(Vector3Int cellPos, Vector2Int size, RotationData rotationData, PlacementLayer layer)
        {
            List<Vector3Int> keys = CalculatePosition(cellPos, size,
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
            List<Vector3Int> keys = new List<Vector3Int>();
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    keys.Add(cellPos + GetKey(x, y, direction));
                    Debug.Log(cellPos + GetKey(x, y, direction));
                }
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

        public void AddPlacementData(Vector3Int cellPos, PlacementData placementData, PlacementLayer layer)
        {
            List<Vector3Int> keys = CalculatePosition(cellPos, placementData.placableItemData.Size,
                placementData.RotationData.direction);
            
            Debug.Log(cellPos + " Cell Pos");
            
            foreach (var key in keys)
            {
                switch (layer)
                {
                    case PlacementLayer.FloorLevel:
                        floorLayerPlacements.Add(key, placementData);
                        break;
                    case PlacementLayer.Surface:
                        surfaceLayerPlacements.Add(key, placementData);
                        break;
                }
                Debug.Log(key);
            }

            if (placementData.SceneObject.TryGetComponent(out Prop prop))
            {
                propList.Add(prop);
            }
            UpdateProps();
        }
        
        public void RemovePlacementData(Vector3Int cellPos, PlacementLayer layer)
        {
            PlacementData placementData = new PlacementData();
            switch (layer)
            {
                case PlacementLayer.FloorLevel:
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

            var go = placementData.SceneObject;
            if (go.TryGetComponent(out Prop prop))
            {
                propList.Remove(prop);
            }
            Object.Destroy(go);
            UpdateProps();
        }
        
        // private List<Vector3Int> GenerateKeysByObjectSize(Vector3Int cellPos, Vector2Int objectSize, Direction direction)
        // {
        //     var keys = new List<Vector3Int>();
        //     keys.Add(cellPos);
        //     CheckHorizontalKeys(cellPos, objectSize, direction, ref keys);
        //     return keys;
        // }
        // private void CheckHorizontalKeys(Vector3Int cellPos, Vector2Int objectSize, Direction direction,
        //     ref List<Vector3Int> keys)
        // {
        //     CheckVerticalKeys(cellPos, objectSize, direction, ref keys);
        //
        //     for (var i = 1; i < objectSize.x; i++)
        //     {
        //         var newCellPos = -Vector3Int.one;
        //         switch (direction)
        //         {
        //             case Direction.Down:
        //                 newCellPos = cellPos + Vector3Int.left * i;
        //                 break;
        //             case Direction.Up:
        //                 newCellPos = cellPos + Vector3Int.right * i;
        //                 break;
        //             case Direction.Left:
        //                 newCellPos = cellPos + Vector3Int.forward * i;
        //                 break;
        //             case Direction.Right:
        //                 newCellPos = cellPos + Vector3Int.back * i;
        //                 break;
        //         }
        //
        //         if (newCellPos != -Vector3Int.one)
        //         {
        //             keys.Add(newCellPos);
        //             CheckVerticalKeys(newCellPos, objectSize, direction, ref keys);
        //         }
        //     }
        // }
        // private void CheckVerticalKeys(Vector3Int cellPos, Vector2Int objectSize, Direction direction,
        //     ref List<Vector3Int> keys)
        // {
        //     for (var i = 1; i < objectSize.y; i++)
        //     {
        //         var newCellPos = -Vector3Int.one;
        //         switch (direction)
        //         {
        //             case Direction.Down:
        //                 newCellPos = cellPos + Vector3Int.forward * i;
        //                 break;
        //             case Direction.Up:
        //                 newCellPos = cellPos + Vector3Int.back * i;
        //                 break;
        //             case Direction.Left:
        //                 newCellPos = cellPos + Vector3Int.right * i;
        //                 break;
        //             case Direction.Right:
        //                 newCellPos = cellPos + Vector3Int.left * i;
        //                 break;
        //         }
        //
        //         if (newCellPos != -Vector3Int.one) keys.Add(newCellPos);
        //     }
        // }
        
        private void UpdateProps()
        {
            foreach (var prop in propList)
                if (prop.transform.TryGetComponent(out IPropUpdate propUpdate))
                    propUpdate.PropUpdate();
        }

        public List<PlacementData> GetPlacementData(PlacementLayer layer)
        {
            switch (layer)
            {
                case PlacementLayer.FloorLevel:
                    return floorLayerPlacements.Values.ToList();

                    break;
                case PlacementLayer.Surface:
                    return surfaceLayerPlacements.Values.ToList();
                    break;
                default:
                    Debug.LogError(layer.ToString() + " Is Missing");
                    return new List<PlacementData>();
                    break;
            }
        }
    }
    
    public enum PlacementLayer
    {
        FloorLevel,
        Surface,
    }
}