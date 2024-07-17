using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class MapData
    {
        public Vector2Int CurrentMapSize { get; private set; }

        // TODO Make door ps vector2Int
        public Vector3Int DoorPosition { get; private set; }
        public int WallDoorIndex { get; private set; }

        public List<WallAssignmentData> WallDatas { get; set; }
        public FloorGridAssignmentData[,] FloorGridDatas { get; set; }
        public PathFinderNode[,] PathFinderNodes { get; set; }

        public MapData()
        {
            WallDoorIndex = 1;
            CurrentMapSize = Vector2Int.one;
            DoorPosition = new Vector3Int(WallDoorIndex, 0, -1);
            WallDatas = new List<WallAssignmentData>();
            FloorGridDatas = new FloorGridAssignmentData[ConstantVariables.MaxMapSizeX, ConstantVariables.MaxMapSizeY];
            PathFinderNodes = new PathFinderNode[ConstantVariables.MaxMapSizeX, ConstantVariables.MaxMapSizeY];
            for (var x = 0; x < ConstantVariables.MaxMapSizeX; x++)
            for (var y = 0; y < ConstantVariables.MaxMapSizeY; y++)
            {
                PathFinderNodes[x, y] = new PathFinderNode(false, -Vector3.one, -1, -1);
                FloorGridDatas[x, y] = new FloorGridAssignmentData(new Vector3Int(x, 0, y));
            }
        }

        public MapData(GameData gameData)
        {
            LoadData(gameData);
        }

        public void LoadData(GameData gameData)
        {
            #region Loading...

            CurrentMapSize = gameData.SavedMapSize;
            WallDoorIndex = gameData.WallDoorIndexOnX;
            DoorPosition = new Vector3Int(WallDoorIndex, 0, -1);

            WallDatas = new List<WallAssignmentData>();
            foreach (var wall in gameData.SavedWallDatas) WallDatas.Add(new WallAssignmentData(wall));

            FloorGridDatas = new FloorGridAssignmentData[ConstantVariables.MaxMapSizeX, ConstantVariables.MaxMapSizeY];
            PathFinderNodes = new PathFinderNode[ConstantVariables.MaxMapSizeX, ConstantVariables.MaxMapSizeY];
            for (var x = 0; x < ConstantVariables.MaxMapSizeX; x++)
            for (var y = 0; y < ConstantVariables.MaxMapSizeY; y++)
            {
                FloorGridDatas[x, y] = new FloorGridAssignmentData(gameData.SavedFloorDatas[new Vector3Int(x, 0, y)]);
                PathFinderNodes[x, y] = new PathFinderNode(false, -Vector3.one, -1, -1);
            }

            #endregion
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.SavedMapSize = CurrentMapSize;

            gameData.SavedWallDatas = new List<GameData.WallSaveData>();
            foreach (var wall in WallDatas) gameData.SavedWallDatas.Add(new GameData.WallSaveData(wall));

            for (var x = 0; x < CurrentMapSize.x; x++)
            for (var y = 0; y < CurrentMapSize.y; y++)
                gameData.SavedFloorDatas[new Vector3Int(x, 0, y)] = new GameData.FloorSaveData(FloorGridDatas[x, y]);
        }

        public bool SetCurrentMapSize(MapGeneratorSystem mapGeneratorSystem, Vector2Int mapSize)
        {
            CurrentMapSize = mapSize;
            return true;
        }

        public PathFinderNode SetTileNodeByCellPos(Vector3Int cellpos)
        {
            if (cellpos.x > CurrentMapSize.x || cellpos.z > CurrentMapSize.y)
            {
                Debug.LogError("TileNode Index Is Not Valid");
                return null;
            }

            return PathFinderNodes[cellpos.x, cellpos.z];
        }

        public Vector3 EnterencePosition => GridHandler.Instance.GetCellCenterWorld(DoorPosition - Vector3Int.right);
    }
}