using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridData : MonoBehaviour
{
    private Dictionary<Vector3Int, PlacementData> placedObject = new ();

    public void AddObjectAt(Vector3Int gridPos, Vector2Int objectSize, int ID, int placedObjectIndex)
    {
        List<Vector3Int> positionToOccupy = CalculatePosition(gridPos, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);
        foreach (var pos in positionToOccupy)
        {
            if(placedObject.ContainsKey(pos))
            {
                throw new Exception($"Dictionary already contains this cell position {pos}");
                placedObject[pos] = data;
            }
        }
    }

    private List<Vector3Int> CalculatePosition(Vector3Int gridPos, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPos + new Vector3Int(x,0,y));
            }
        }

        return returnVal;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPos, Vector2Int objectSize)
    {
        List<Vector3Int> positionOccupy = CalculatePosition(gridPos, objectSize);
        foreach (var pos in positionOccupy)
        {
            if (placedObject.ContainsKey(pos))
                return false;
        }

        return true;
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPosition;
    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }
    
    //Constacter
    public PlacementData(List<Vector3Int> occupiedPosition,int id, int placedObjectIndex)
    {
        this.occupiedPosition = occupiedPosition;
        ID = id;
        PlacedObjectIndex = placedObjectIndex;
    }
}
