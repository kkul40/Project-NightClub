using System;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlacamentDataHandler
{
    private Dictionary<Vector3Int, PlacementData> placementDatas;
    private List<Prop> propList;

    public PlacamentDataHandler()
    {
        placementDatas = new Dictionary<Vector3Int, PlacementData>();
        propList = new List<Prop>();
    }
    
    public bool ContainsKey(Vector3Int cellPos)
    {
        Vector3Int border = GameData.Instance.MapGenerator.MapSize;
        
        if (cellPos.x < 0 || cellPos.z < 0) return true;

        if (cellPos.x >= border.x || cellPos.z >= border.z) return true; // TODO Kapinin kordinatlari bunlar ve baska bir yere tasi
        
        if (cellPos.x == 4 && cellPos.z == 0) return true;
        
        if (placementDatas.ContainsKey(cellPos))
        {
            return true;
        }
        return false;
    }

    public bool ContainsKey(Vector3Int cellPos, Vector2Int objectSize, Direction direction)
    {
        List<Vector3Int> keys = new List<Vector3Int>();
        
        keys = GeneratePlacableKeys(cellPos, objectSize, direction);

        foreach (var key in keys)
        {
            if (ContainsKey(key)) return true;
        }

        return false;
    }

    public void RemovePlacementData(Vector3Int cellPos)
    {
        if (!ContainsKey(cellPos)) return;

        GameObject go = placementDatas[cellPos].SceneObject;
        
        TryRemoveProp(go);
        Object.Destroy(go);
        UpdateProps();

        var value = placementDatas[cellPos];
        
        List<Vector3Int> keys = new List<Vector3Int>();
        foreach (var key in placementDatas.Keys)
        {
            if (placementDatas[key] == value)
            {
                keys.Add(key);
            }
        }
        foreach (var key in keys)
        {
            placementDatas.Remove(key);
        }
    }
    
    public void AddPlacementData(Vector3Int cellPos, PlacementData placementData)
    {
        if (ContainsKey(cellPos)) return;

        var objectSize = placementData.ObjectSize;
        var direction = placementData.Direction;
        List<Vector3Int> keys = new List<Vector3Int>();

        keys = GeneratePlacableKeys(cellPos, objectSize, direction);

        bool keysAreValid = true;
        foreach (var key in keys)
        {
            keysAreValid = !ContainsKey(key);

            if (!keysAreValid)
            {
                return;
            }
        }

        if (keysAreValid)
        {
            foreach (var key in keys)
            {
                placementDatas.Add(key, placementData);
            }
            TryAddProp(placementData.SceneObject);
            UpdateProps();
        }
    }

    private List<Vector3Int> GeneratePlacableKeys(Vector3Int cellPos, Vector2Int objectSize, Direction direction)
    {
        List<Vector3Int> keys = new List<Vector3Int>();
        keys.Add(cellPos);
        CheckHorizontalKeys(cellPos, objectSize, direction, ref keys);
        return keys;
    }

    private void CheckHorizontalKeys(Vector3Int cellPos, Vector2Int objectSize, Direction direction, ref List<Vector3Int> keys)
    {
        CheckVerticalKeys(cellPos, objectSize, direction, ref keys);
        
        for (int i = 1; i < objectSize.x; i++)
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

    private void CheckVerticalKeys(Vector3Int cellPos, Vector2Int objectSize, Direction direction, ref List<Vector3Int> keys)
    {
        for (int i = 1; i < objectSize.y; i++)
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
            
            if (newCellPos != -Vector3Int.one)
            {
                keys.Add(newCellPos);
            }
        }
    }

    private void TryAddProp(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Prop prop))
        {
            propList.Add(prop);
        }
    }

    private void TryRemoveProp(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Prop prop))
        {
            propList.Remove(prop);
        }
    }
    
    public GameObject GetPlacedObjectByCellPosition(Vector3Int cellPos)
    {
        if (ContainsKey(cellPos))
        {
            return placementDatas[cellPos].SceneObject;
        }
        return null;
    }
    
    public List<PlacementData> GetPlacementData()
    {
        return placementDatas.Values.ToList();
    }
    
    private void UpdateProps()
    {
        foreach (var prop in propList)
        {
            if (prop.transform.TryGetComponent(out IPropUpdate propUpdate))
            {
                propUpdate.PropUpdate();
            }
        }
    }

    public List<Prop> GetPropList => propList;
}