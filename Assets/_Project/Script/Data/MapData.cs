using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Data
{
    public class MapData
    {
        public Vector2Int CurrentMapSize { get; private set; }
        
        public Vector2Int PathFinderSize
        {
            get
            {
                return new Vector2Int((CurrentMapSize.x * ConstantVariables.PathFinderGridSize) + 1, (CurrentMapSize.y * ConstantVariables.PathFinderGridSize) + 1);
            }
        }

        // TODO Make door ps vector2Int
        public Vector3Int DoorPosition { get; private set; }
        public int WallDoorIndex { get; private set; }

        //TODO Dinamik olarak 2 dimension arraylari ayarla
        public List<WallAssignmentData> WallDatas { get; set; }
        public FloorGridAssignmentData[,] FloorGridDatas { get; set; }

        private PathFinderNode[,] NewPathFinderNodes;

        public MapData()
        {
            // Default
            WallDoorIndex = 1;
            CurrentMapSize = Vector2Int.one;
            DoorPosition = new Vector3Int(WallDoorIndex, 0, -1);
            WallDatas = new List<WallAssignmentData>();
            FloorGridDatas = new FloorGridAssignmentData[ConstantVariables.MaxMapSizeX, ConstantVariables.MaxMapSizeY];

            for (var x = 0; x < ConstantVariables.MaxMapSizeX; x++)
            for (var y = 0; y < ConstantVariables.MaxMapSizeY; y++)
                FloorGridDatas[x, y] = new FloorGridAssignmentData(new Vector3Int(x, 0, y));

            SetUpNewPathFinder();
        }

        private void SetUpNewPathFinder()
        {
            int MaxX = ConstantVariables.MaxMapSizeX * ConstantVariables.PathFinderGridSize + 1;
            int MaxY = ConstantVariables.MaxMapSizeY * ConstantVariables.PathFinderGridSize + 1;
            NewPathFinderNodes = new PathFinderNode[MaxX, MaxY];

            for (var x = 0; x < MaxX; x++)
            {
                for (var y = 0; y < MaxY; y++)
                {
                    NewPathFinderNodes[x, y] = new PathFinderNode();
                    var node = NewPathFinderNodes[x, y];
                    node.IsWalkable = false;
                    node.GridX = x;
                    node.GridY = y;
                    node.WorldPos = new Vector3Int(x, 0, y).CellCenterPosition(eGridType.PathFinderGrid);
                }
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
            for (var x = 0; x < ConstantVariables.MaxMapSizeX; x++)
            for (var y = 0; y < ConstantVariables.MaxMapSizeY; y++)
                FloorGridDatas[x, y] = new FloorGridAssignmentData(gameData.SavedFloorDatas[new Vector3Int(x, 0, y)]);
            
            SetUpNewPathFinder();
            #endregion
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.SavedMapSize = CurrentMapSize;

            gameData.SavedWallDatas = new List<GameData.WallSaveData>();
            foreach (var wall in WallDatas) gameData.SavedWallDatas.Add(wall);

            for (var x = 0; x < CurrentMapSize.x; x++)
            for (var y = 0; y < CurrentMapSize.y; y++)
                gameData.SavedFloorDatas[new Vector3Int(x, 0, y)] = new GameData.FloorSaveData(FloorGridDatas[x, y]);
        }

        public bool SetCurrentMapSize(MapGeneratorSystem mapGeneratorSystem, Vector2Int mapSize)
        {
            CurrentMapSize = mapSize;
            return true;
        }
        
        public void SetPathFinderNode(Vector3Int cellPosition, bool isBig, bool? isWalkable = null, bool? isWall = null)
        {
            SetPathFinderNode(cellPosition.PlacementPosToPathFinderIndex(), isBig, isWalkable, isWall);
        }

        public void SetPathFinderNode(Vector2Int pathNodeIndex, bool isBig, bool? isWalkable = null, bool? isWall = null)
        {
            int minimumSpace = 0;
            int maximumSpace = 1;
            
            int setMaxCellPosX = pathNodeIndex.x + (isBig ? ConstantVariables.PathFinderGridSize - minimumSpace : ConstantVariables.PathFinderGridSize - maximumSpace);
            int setMaxCellPosY = pathNodeIndex.y + (isBig ? ConstantVariables.PathFinderGridSize - minimumSpace : ConstantVariables.PathFinderGridSize - maximumSpace);

            int setMinCellPosX = pathNodeIndex.x + (isBig ? minimumSpace : maximumSpace);
            int setMinCellPosY = pathNodeIndex.y + (isBig ? minimumSpace : maximumSpace);
            
            for (int i = setMinCellPosX; i <= setMaxCellPosX; i++)
            for (int j = setMinCellPosY; j <= setMaxCellPosY; j++)
            {
                if (isWall ?? false)
                    NewPathFinderNodes[i, j].IsWall = isWall ?? NewPathFinderNodes[i, j].IsWall;
                
                NewPathFinderNodes[i, j].IsWalkable = isWalkable ?? NewPathFinderNodes[i, j].IsWalkable;
            }
        }

        public PathFinderNode GetRandomPathFinderNode()
        {
            return NewPathFinderNodes[Random.Range(0, PathFinderSize.x), Random.Range(0, PathFinderSize.y)];
        }

        public PathFinderNode[,] GetNewPathFinderNote()
        {
            var outputNode = new PathFinderNode[PathFinderSize.x, PathFinderSize.y];

            for (var x = 0; x < PathFinderSize.x; x++)
            for (var y = 0; y < PathFinderSize.y; y++)
            {
                outputNode[x, y] = NewPathFinderNodes[x, y].Copy();

                if (x > WallDoorIndex * ConstantVariables.PathFinderGridSize - ConstantVariables.PathFinderGridSize && 
                    x < WallDoorIndex * ConstantVariables.PathFinderGridSize && 
                    y == 0) continue;
                
                if (x == 0 || y == 0 || x == PathFinderSize.x - 1 || y == PathFinderSize.y - 1)
                {
                    outputNode[x, y].IsWall = true;
                }
            }
            
            return outputNode;
        }

        public PathFinderNode GetPathNodeByWorldPos(Vector3 worldPos)
        {
            // var convert = GridHandler.Instance.GetWorldToCell(worldPos, eGridType.PathFinderGrid);
            var convert = worldPos.WorldPosToCellPos(System.eGridType.PathFinderGrid);
            return NewPathFinderNodes[convert.x, convert.z];
        }

        public FloorGridAssignmentData GetFloorGridAssignmentByCellPos(Vector3Int cellpos)
        {
            if (cellpos.x > CurrentMapSize.x || cellpos.z > CurrentMapSize.y)
            {
                Debug.LogError("FloorGridData Index Is Not Valid");
                return null;
            }

            return FloorGridDatas[cellpos.x, cellpos.z];
        }

        // public Vector3 EnterencePosition => GridHandler.Instance.GetCellCenterWorld(DoorPosition + new Vector3Int(-1, 0, 1), eGridType.PlacementGrid) - new Vector3(0, 0.5f, 0);
        public Vector3 EnterencePosition => DoorPosition.ToFloat().AddVector(new Vector3Int(-1, 0, 1)).WorldPosToCellPos(eGridType.PlacementGrid).CellCenterPosition(eGridType.PlacementGrid);

        public Vector3 SpawnPositon => EnterencePosition - new Vector3(0, 0, 3);
    }
}