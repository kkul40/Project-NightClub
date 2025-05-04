using System;
using System.Collections.Generic;
using System.Linq;
using Data.New;
using DiscoSystem;
using DiscoSystem.Building_System.GameEvents;
using ExtensionMethods;
using PropBehaviours;
using SerializableTypes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Data
{
    [Serializable]
    public class MapData : IDisposable
    {
    // TODO Make door ps vector2Int
    public int WallDoorIndex { get; private set; }
    public bool IsWallDoorOnX { get; private set; }
    public Vector2Int CurrentMapSize { get; private set; }
    public Vector3Int DoorPosition { get; private set; }

    public PathData Path;

    //TODO Dinamik olarak 2 dimension arraylari ayarla

    public Dictionary<Vector3Int, WallData> NewWallData;
    
    private FloorData[,] FloorGridDatas;

    // Referanced
    private List<PathFinderNode> AvaliableWallPaths;

    // Flags

    public Vector2Int PathFinderSize => Path.PathFinderSize();

    // TODO Do More Optimization


    // public MapData()
    // {
    //     // Intensional Broken Data
    //     // Debug.LogError("Initialized With No Data");
    //
    //     CurrentMapSize = Vector2Int.one;
    //     Path = new PathData(ConstantVariables.MaxMapSizeX, ConstantVariables.MaxMapSizeY, this);
    //
    //     ChangeDoorPosition(1, true);
    //
    //     if (IsWallDoorOnX)
    //         DoorPosition = new Vector3Int(WallDoorIndex - 1, 0, 0);
    //     else
    //         DoorPosition = new Vector3Int(0, 0, WallDoorIndex - 1);
    //
    //     WallDatas = new List<WallData>();
    //     FloorGridDatas = new FloorData[ConstantVariables.MaxMapSizeX, ConstantVariables.MaxMapSizeY];
    //
    //     for (var x = 0; x < ConstantVariables.MaxMapSizeX; x++)
    //     for (var y = 0; y < ConstantVariables.MaxMapSizeY; y++)
    //         FloorGridDatas[x, y] = new FloorData(new Vector3Int(x, 0, y));
    //     
    //     KEvent_Map.TriggerMapSizeChanged(CurrentMapSize);
    // }

    public MapData(NewGameData gameData)
    {
        CurrentMapSize = gameData.mapData.mapSize;
        WallDoorIndex = gameData.mapData.wallDoorIndex;
        Path = new PathData(ConstantVariables.MaxMapSizeX, ConstantVariables.MaxMapSizeY, this);

        ChangeDoorPosition(gameData.mapData.wallDoorIndex, gameData.mapData.isWallOnX);

        NewWallData = new Dictionary<Vector3Int, WallData>();
        foreach (var wall in gameData.mapData.wallDatas)
            NewWallData.Add(wall.Key, new WallData(wall.Key, wall.Value.assignedMaterialID));

        FloorGridDatas = new FloorData[ConstantVariables.MaxMapSizeX, ConstantVariables.MaxMapSizeY];
        for (var x = 0; x < ConstantVariables.MaxMapSizeX; x++)
        for (var y = 0; y < ConstantVariables.MaxMapSizeY; y++)
        {
            FloorData data;
            Vector3Int cellPos = new Vector3Int(x, 0, y);
            
            if (gameData.mapData.floorDatas.TryGetValue(cellPos, out var floorData))
                data = new FloorData(floorData);
            else
                data = new FloorData(cellPos);

            FloorGridDatas[x, y] = data;
        }
        
        GameEvent.Subscribe<Event_OnGameSave>(handle => SaveData(ref handle.GameData));
    }
    
    public void Dispose()
    {
        // TODO release managed resources here
    }

    #region Saving And Loading...

    private void SaveData(ref NewGameData gameData)
    {
        gameData.mapData.mapSize = CurrentMapSize;

        gameData.mapData.isWallOnX = IsWallDoorOnX;
        gameData.mapData.wallDoorIndex = WallDoorIndex;

        gameData.mapData.wallDatas.Clear();
        foreach (var wall in NewWallData)
            gameData.mapData.wallDatas.Add(wall.Key, new Save_MapData.Save_WallData(wall.Value.AssignedMaterialID));

        gameData.mapData.floorDatas = new SerializableDictionary<Vector3Int, Save_MapData.Save_FloorData>();
        
        for (var x = 0; x < CurrentMapSize.x; x++)
        for (var y = 0; y < CurrentMapSize.y; y++)
        {
            Save_MapData.Save_FloorData floorData = Save_MapData.Save_FloorData.Convert(FloorGridDatas[x, y]);
            gameData.mapData.floorDatas.Add(new Vector3Int(x, 0, y), floorData);
        }

        gameData.mapData.placementDatas = new List<Save_MapData.Save_PlacementData>();
        foreach (var placedItem in DiscoData.Instance.PlacedItems.Values)
        {
            Save_MapData.Save_PlacementData data = new Save_MapData.Save_PlacementData();
            data.PropID = placedItem.Item1;
            data.EularAngles = placedItem.Item2.eulerAngles;
            data.PlacedPosition = placedItem.Item2.position;
            
            gameData.mapData.placementDatas.Add(data);
        }
        
        Debug.Log("Map Data Saved");
    }

    #endregion

    public bool ChangeMapSize(int x, int y)
    {
        if (CurrentMapSize.x + x > ConstantVariables.MaxMapSizeX) return false;
        CurrentMapSize += new Vector2Int(x, 0);
        
        if (CurrentMapSize.y + y > ConstantVariables.MaxMapSizeY) return false;
        CurrentMapSize += new Vector2Int(0, y);
        
        GameEvent.Trigger(new Event_MapSizeChanged(CurrentMapSize));
        return true;
    }

    public void RevertMapSize(int x, int y)
    {
        for (int i = CurrentMapSize.x - x; i < CurrentMapSize.x; i++)
        {
            for (int j = 0; j < CurrentMapSize.y; j++)
            {
                FloorData data = FloorGridDatas[i, j];
                Object.DestroyImmediate(data.assignedFloorTile.gameObject);
            }
        }
        
        for (int i = CurrentMapSize.y - y; i < CurrentMapSize.y; i++)
        {
            for (int j = 0; j < CurrentMapSize.x; j++)
            {
                FloorData data = FloorGridDatas[j, i];
                if (data.assignedFloorTile == null) continue;
                Object.DestroyImmediate(data.assignedFloorTile.gameObject);
            }
        }

        for (int i = NewWallData.ToList().Count - 1; i >= 0; i--)
        {
            var pair = NewWallData.ToList()[i];
            if (pair.Key.x > CurrentMapSize.x - x || pair.Key.z > CurrentMapSize.y - y)
                RemoveWallData(pair.Key);
        }

        CurrentMapSize -= new Vector2Int(x, y);
        GameEvent.Trigger(new Event_MapSizeChanged(CurrentMapSize));
    }

    public bool CheckMapExpendable(int x, int y)
    {
        if (CurrentMapSize.x + x > ConstantVariables.MaxMapSizeX) return false;
        if (CurrentMapSize.y + y > ConstantVariables.MaxMapSizeY) return false;

        return true;
    }

    public void ChangeDoorPosition(int newDoorIndex, bool isWallOnX)
    {
        IsWallDoorOnX = isWallOnX;
        WallDoorIndex = newDoorIndex;

        if (IsWallDoorOnX)
            DoorPosition = new Vector3Int(WallDoorIndex - 1, 0, 0);
        else
            DoorPosition = new Vector3Int(0, 0, WallDoorIndex - 1);
        
        GameEvent.Trigger(new Event_PropRelocated(null));
    }

    public PathFinderNode GetRandomPathFinderNode()
    {
        return Path.GetRandomPathNode();
    }

    public FloorData GetFloorGridData(int x, int y)
    {
        if (x < 0 || y < 0) return null;
        if (x >= CurrentMapSize.x || y >= CurrentMapSize.y) return null;

        return FloorGridDatas[x, y];
    }

    public FloorData GetFloorGridAssignmentByCellPos(Vector3Int cellpos)
    {
        if (cellpos.x > CurrentMapSize.x || cellpos.z > CurrentMapSize.y)
        {
            Debug.LogError("FloorGridData Index Is Not Valid");
            return null;
        }

        return FloorGridDatas[cellpos.x, cellpos.z];
    }

    /// <summary>
    /// Adds New Wall Data and Returns It
    /// </summary>
    /// <param name="cellPosition"></param>
    /// <param name="wallObject"></param>
    /// <returns>Return Added WallAssignmentData</returns>
    public WallData AddNewWallData(Vector3Int cellPosition, GameObject wallObject)
    {
        if (NewWallData.ContainsKey(cellPosition))
        {
            NewWallData[cellPosition].AssignReferance(wallObject.GetComponent<Wall>());
            return NewWallData[cellPosition];
        }

        NewWallData.Add(cellPosition, new WallData(cellPosition, -1));
        NewWallData[cellPosition].AssignReferance(wallObject.GetComponent<Wall>());
        return NewWallData[cellPosition];
    }

    public void RemoveWallData(Vector3Int cellPosition)
    {
        var data = GetWallDataByCellPos(cellPosition);

        if (data.AssignedWall != null)
        {
            Object.Destroy(data.AssignedWall.gameObject);
        }

        NewWallData.Remove(cellPosition);
    }

    public WallData GetWallDataByCellPos(Vector3Int cellPosition)
    {
        return NewWallData[cellPosition];
    }

    public WallData GetWallDoor()
    {
        foreach (var pair in NewWallData)
        {
            if (pair.Value.AssignedWall is WallDoor door)
            {
                return pair.Value;
            }
        }

        return null;
    }

    public WallData GetWallDataByWall(Wall wall)
    {
        foreach (var value in NewWallData.Values)
        {
            if (value.AssignedWall.GetInstanceID() == wall.GetInstanceID())
                return value;
        }

        return null;
    }

    public Vector3 EnterencePosition(Vector3Int? doorPositon = null)
    {
        Vector3 enterancePosition = doorPositon ?? DoorPosition;
        if (IsWallDoorOnX)
            enterancePosition.AddVector(new Vector3(-1, 0, 1));
        else
            enterancePosition.AddVector(new Vector3(1, 0, -1));

        return enterancePosition.WorldPosToCellPos(eGridType.PlacementGrid).CellCenterPosition(eGridType.PlacementGrid);
    }

    public Vector3 SpawnPositon => EnterencePosition() - (IsWallDoorOnX ? new Vector3(0, 0, 1) : new Vector3(1, 0, 0));
    }
}