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

        // TODO Make door ps vector2Int
        public Vector3Int DoorPosition { get; private set; }
        public int WallDoorIndex { get; private set; }

        //TODO Dinamik olarak 2 dimension arraylari ayarla
        public List<WallAssignmentData> WallDatas { get; set; }
        public FloorGridAssignmentData[,] FloorGridDatas { get; set; }
        
        private PathFinderNode[,] PathFinderNodes;

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
                PathFinderNodes[x, y] = new PathFinderNode();
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
                PathFinderNodes[x, y] = new PathFinderNode();
            }

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

        public PathFinderNode GetTileNodeByCellPos(Vector3Int cellpos)
        {
            if (cellpos.x > CurrentMapSize.x || cellpos.z > CurrentMapSize.y)
            {
                Debug.LogError("TileNode Index Is Not Valid");
                return null;
            }

            return PathFinderNodes[cellpos.x, cellpos.z];
        }

        public void SetPathfinderNode(int x, int y, bool? isAvaliable = null, bool? isWalkable = null, Vector3? position = null, int? gridX = null, int? gridY = null)
        {
            if (x > CurrentMapSize.x || y > CurrentMapSize.y)
            {
                Debug.LogError("TileNode Index Is Not Valid");
                return;
            }
            
            var node = PathFinderNodes[x, y];
            node.IsWalkable = isWalkable ?? node.IsWalkable;
            node.WorldPos = position ?? node.WorldPos;
            node.GridX = gridX ?? node.GridX;
            node.GridY = gridY ?? node.GridY;
        }

        public PathFinderNode GetRandomPathFinderNode()
        {
            return PathFinderNodes[Random.Range(0, DiscoData.Instance.MapData.CurrentMapSize.x), Random.Range(0, DiscoData.Instance.MapData.CurrentMapSize.y)];
        }

        public PathFinderNode[,] GetPathFinderNode()
        {
            // TODO Herseferinde bunu yeniden olusturmak yerine arada bir guncellemeyi dene
            PathFinderNode[,] outputNode = new PathFinderNode[CurrentMapSize.x, CurrentMapSize.y];

            for (int x = 0; x < CurrentMapSize.x; x++)
                for (int y = 0; y < CurrentMapSize.y; y++)
                    outputNode[x, y] = PathFinderNodes[x, y].Copy();

            return outputNode;
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

        public Vector3 EnterencePosition => GridHandler.Instance.GetCellCenterWorld(DoorPosition + new Vector3Int(-1,0 ,1)) - new Vector3(0,0.5f, 0);
        public Vector3 SpawnPositon => EnterencePosition - new Vector3(0, 0, 3);
    }
}