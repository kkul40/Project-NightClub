using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class MapData
    {
        public Vector2Int CurrentMapSize = Vector2Int.zero;

        // TODO Make door ps vector2Int
        public Vector3Int DoorPosition { get; private set; }

        public PathFinderNode[,] PathFinderNodes { get; }
        public FloorGridAssignmentData[,] FloorGridDatas { get; }
        public List<WallAssignmentData> WallDatas { get; set; }
        public int WallDoorIndex { get; private set; }

        // TODO Bu Verileri SaveData dan cek
        public MapData(GameData gameData)
        {
            // Load Data
            LoadMapData(gameData);

            PathFinderNodes = new PathFinderNode[ConstantVariables.MaxMapSizeX, ConstantVariables.MaxMapSizeY];
            FloorGridDatas = new FloorGridAssignmentData[ConstantVariables.MaxMapSizeX, ConstantVariables.MaxMapSizeY];

            for (var i = 0; i < ConstantVariables.MaxMapSizeX; i++)
            for (var j = 0; j < ConstantVariables.MaxMapSizeY; j++)
            {
                PathFinderNodes[i, j] = new PathFinderNode(false, -Vector3.one, -1, -1);
                FloorGridDatas[i, j] = new FloorGridAssignmentData(-Vector3Int.one);
            }
        }

        private void LoadMapData(GameData gameData)
        {
            CurrentMapSize = gameData.SavedMapSize;
            WallDoorIndex = gameData.WallDoorIndexOnX;
            DoorPosition = new Vector3Int(WallDoorIndex, 0, -1);

            WallDatas = new List<WallAssignmentData>();
            foreach (var saveD in gameData.SavedWallDatas)
                WallDatas.Add(new WallAssignmentData(saveD.CellPosition, saveD.AssignedMaterialID));
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