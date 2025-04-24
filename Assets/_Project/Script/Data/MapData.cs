using System;
using System.Collections.Generic;
using DiscoSystem;
using DiscoSystem.Building_System.GameEvents;
using ExtensionMethods;
using PropBehaviours;
using SaveAndLoad;
using UnityEngine;

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
    public List<WallData> WallDatas;
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

    public MapData(GameData gameData)
    {
        CurrentMapSize = gameData.SavedMapSize;
        WallDoorIndex = gameData.WallDoorIndex;
        Path = new PathData(ConstantVariables.MaxMapSizeX, ConstantVariables.MaxMapSizeY, this);

        ChangeDoorPosition(gameData.WallDoorIndex, gameData.IsWallOnX);

        WallDatas = new List<WallData>();
        foreach (var wall in gameData.SavedWallDatas) WallDatas.Add(new WallData(wall));

        FloorGridDatas = new FloorData[ConstantVariables.MaxMapSizeX, ConstantVariables.MaxMapSizeY];
        for (var x = 0; x < ConstantVariables.MaxMapSizeX; x++)
        for (var y = 0; y < ConstantVariables.MaxMapSizeY; y++)
            FloorGridDatas[x, y] = new FloorData(gameData.SavedFloorDatas[new Vector3Int(x, 0, y)]);
    }
    
    public void Dispose()
    {
        // TODO release managed resources here
    }

    #region Saving And Loading...

    public void SaveData(ref GameData gameData)
    {
        gameData.SavedMapSize = CurrentMapSize;

        gameData.IsWallOnX = IsWallDoorOnX;
        gameData.WallDoorIndex = WallDoorIndex;

        gameData.SavedWallDatas = new List<GameDataExtension.WallSaveData>();
        foreach (var wall in WallDatas) gameData.SavedWallDatas.Add(wall.ConvertToWallSaveData());

        for (var x = 0; x < CurrentMapSize.x; x++)
        for (var y = 0; y < CurrentMapSize.y; y++)
            gameData.SavedFloorDatas[new Vector3Int(x, 0, y)] = FloorGridDatas[x, y].ConvertToFloorSaveData();
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
        var data = GetWallDataByCellPos(cellPosition);
        if (data == null)
        {
            Debug.Log("Data Was NULL");
            WallDatas.Add(new WallData(cellPosition));
            data = WallDatas[^1];
        }

        data.AssignReferance(wallObject.GetComponent<Wall>());
        return data;
    }

    public void RemoveWallData(Vector3Int cellPosition)
    {
        var data = GetWallDataByCellPos(cellPosition);

        if (data.assignedWall != null)
        {
            MonoBehaviour.Destroy(data.assignedWall.gameObject);
        }

        WallDatas.Remove(data);
    }

    public WallData GetWallDataByCellPos(Vector3Int cellPosition)
    {
        return WallDatas.Find(x => x.CellPosition == cellPosition);
    }

    public WallData GetWallDataByWall(Wall wall)
    {
        return WallDatas.Find(x => x.assignedWall.GetInstanceID() == wall.GetInstanceID());
    }

    public WallData GetLastIndexWallData()
    {
        return WallDatas[^1];
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