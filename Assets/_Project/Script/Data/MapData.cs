using System;
using System.Collections.Generic;
using Disco_ScriptableObject;
using ExtensionMethods;
using PropBehaviours;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Data
{
    public class MapData
    {
        // TODO Make door ps vector2Int
        public int WallDoorIndex { get; private set; }
        public bool IsWallDoorOnX { get; private set; }
        public Vector2Int CurrentMapSize { get; private set; }
        public Vector3Int DoorPosition { get; private set; }

        //TODO Dinamik olarak 2 dimension arraylari ayarla
        public List<WallAssignmentData> WallDatas { get; set; }
        private FloorGridAssignmentData[,] FloorGridDatas;
        private PathFinderNode[,] PathFinderNodes;

        // Referanced
        private List<PathFinderNode> AvaliableWallPaths;
        private bool DirtyFlag_AvaliablePathsNearWall = true;

        public Vector2Int PathFinderSize
        {
            get
            {
                return new Vector2Int((CurrentMapSize.x * ConstantVariables.PathFinderGridSize) + 1,
                    (CurrentMapSize.y * ConstantVariables.PathFinderGridSize) + 1);
            }
        }

        // TODO Do More Optimization
        public List<PathFinderNode> GetAvaliableWallPaths
        {
            get
            {
                if (DirtyFlag_AvaliablePathsNearWall)
                {
                    UpdateAvaliableWallPaths();
                    DirtyFlag_AvaliablePathsNearWall = false;
                }
                
                return AvaliableWallPaths;
            }
        }

        public MapData()
        {
            return;
            // Intensional Broken Data
            CurrentMapSize = Vector2Int.one;
            
            ChangeDoorPosition(1, true);

            if (IsWallDoorOnX)
                DoorPosition = new Vector3Int(WallDoorIndex - 1, 0, 0);
            else
                DoorPosition = new Vector3Int(0, 0, WallDoorIndex - 1);
            
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
            PathFinderNodes = new PathFinderNode[MaxX, MaxY];

            for (var x = 0; x < MaxX; x++)
            {
                for (var y = 0; y < MaxY; y++)
                {
                    PathFinderNodes[x, y] = new PathFinderNode();
                    var node = PathFinderNodes[x, y];
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

        #region Saving And Loading...
        public void LoadData(GameData gameData)
        {
            CurrentMapSize = gameData.SavedMapSize;
            WallDoorIndex = gameData.WallDoorIndex;
            
            ChangeDoorPosition(gameData.WallDoorIndex, gameData.IsWallOnX);
            
            WallDatas = new List<WallAssignmentData>();
            foreach (var wall in gameData.SavedWallDatas) WallDatas.Add(new WallAssignmentData(wall));

            FloorGridDatas = new FloorGridAssignmentData[ConstantVariables.MaxMapSizeX, ConstantVariables.MaxMapSizeY];
            for (var x = 0; x < ConstantVariables.MaxMapSizeX; x++)
            for (var y = 0; y < ConstantVariables.MaxMapSizeY; y++)
                FloorGridDatas[x, y] = new FloorGridAssignmentData(gameData.SavedFloorDatas[new Vector3Int(x, 0, y)]);

            SetUpNewPathFinder();
        }

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

        public bool SetCurrentMapSize(MapGeneratorSystem mapGeneratorSystem, Vector2Int mapSize)
        {
            CurrentMapSize = mapSize;
            
            DirtyFlag_AvaliablePathsNearWall = true;
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

            DirtyFlag_AvaliablePathsNearWall = true;
        }

        public void UpdatePathFinderNode(Vector3Int cellPosition, bool isBig, bool? isWalkable = null, bool? isWall = null)
        {
            UpdatePathFinderNode(cellPosition.PlacementPosToPathFinderIndex(), isBig, isWalkable, isWall);
        }

        public void UpdatePathFinderNode(Vector2Int pathNodeIndex, bool isBig, bool? isWalkable = null,
            bool? isWall = null)
        {
            int minimumSpace = 0;
            int maximumSpace = 1;

            int setMaxCellPosX = pathNodeIndex.x +
                                 (isBig
                                     ? ConstantVariables.PathFinderGridSize - minimumSpace
                                     : ConstantVariables.PathFinderGridSize - maximumSpace);
            int setMaxCellPosY = pathNodeIndex.y +
                                 (isBig
                                     ? ConstantVariables.PathFinderGridSize - minimumSpace
                                     : ConstantVariables.PathFinderGridSize - maximumSpace);

            int setMinCellPosX = pathNodeIndex.x + (isBig ? minimumSpace : maximumSpace);
            int setMinCellPosY = pathNodeIndex.y + (isBig ? minimumSpace : maximumSpace);

            for (int i = setMinCellPosX; i <= setMaxCellPosX; i++)
            for (int j = setMinCellPosY; j <= setMaxCellPosY; j++)
            {
                if (isWall ?? false)
                    PathFinderNodes[i, j].IsWall = isWall ?? PathFinderNodes[i, j].IsWall;

                PathFinderNodes[i, j].IsWalkable = isWalkable ?? PathFinderNodes[i, j].IsWalkable;
            }

            // Flag
            DirtyFlag_AvaliablePathsNearWall = true;
        }

        public void UpdateAvaliableWallPaths()
        {
            AvaliableWallPaths = new List<PathFinderNode>();

            int howFarFromWall = 1;
            for (int x = ConstantVariables.PathFinderGridSize / 2; x < PathFinderSize.x; x += ConstantVariables.PathFinderGridSize)
            {
                PathFinderNode node = PathFinderNodes[x,howFarFromWall];
                if (!ContainsKeyOnPath(node, ePlacementLayer.BaseSurface) && !ContainsKeyOnPath(node, ePlacementLayer.FloorProp))
                    AvaliableWallPaths.Add(node);
            }
            for (int y = ConstantVariables.PathFinderGridSize / 2; y < PathFinderSize.y; y += ConstantVariables.PathFinderGridSize)
            {
                PathFinderNode node = PathFinderNodes[howFarFromWall,y];
                if (!ContainsKeyOnPath(node, ePlacementLayer.BaseSurface) && !ContainsKeyOnPath(node, ePlacementLayer.FloorProp))
                    AvaliableWallPaths.Add(node);
            }

            for (int i = AvaliableWallPaths.Count - 1; i >= 0; i--)
            {
                if (AvaliableWallPaths[i].WorldPos.WorldPosToCellPos(eGridType.PlacementGrid) == EnterencePosition().WorldPosToCellPos(eGridType.PlacementGrid))
                {
                    AvaliableWallPaths.RemoveAt(i);
                    break;
                }
            }
        }

        private bool ContainsKeyOnPath(PathFinderNode node, ePlacementLayer layer)
        {
            return DiscoData.Instance.placementDataHandler.ContainsKey(node.WorldPos.WorldPosToCellPos(eGridType.PlacementGrid), layer);
        }

        public PathFinderNode GetRandomPathFinderNode()
        {
            return PathFinderNodes[Random.Range(0, PathFinderSize.x), Random.Range(0, PathFinderSize.y)];
        }

        public PathFinderNode[,] GetPathFinderNodes()
        {
            var outputNode = new PathFinderNode[PathFinderSize.x, PathFinderSize.y];

            for (var x = 0; x < PathFinderSize.x; x++)
            for (var y = 0; y < PathFinderSize.y; y++)
            {
                outputNode[x, y] = PathFinderNodes[x, y].Copy();

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
            var convert = worldPos.WorldPosToCellPos(System.eGridType.PathFinderGrid);
            return PathFinderNodes[convert.x, convert.z];
        }
        
        public FloorGridAssignmentData GetFloorGridData(int x, int y)
        {
            if (x < 0 || y < 0) return null;
            if (x > CurrentMapSize.x || y > CurrentMapSize.y) return null;
        
            return FloorGridDatas[x, y];
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

        /// <summary>
        /// Adds New Wall Data and Returns It
        /// </summary>
        /// <param name="cellPosition"></param>
        /// <param name="wallObject"></param>
        /// <returns>Return Added WallAssignmentData</returns>
        public WallAssignmentData AddNewWallData(Vector3Int cellPosition, GameObject wallObject)
        {
            var data = GetWallDataByCellPos(cellPosition);
            if (data == null)
            {
                Debug.Log("Data Was NULL");
                WallDatas.Add(new WallAssignmentData(cellPosition));
                data = WallDatas[^1];
            }

            data.AssignReferance(wallObject.GetComponent<Wall>());
            
            DirtyFlag_AvaliablePathsNearWall = true;

            return data;
        }

        public void RemoveWallData(Vector3Int cellPosition)
        {
            var data = GetWallDataByCellPos(cellPosition);

            if (data.assignedWall != null)
            {
                MonoBehaviour.Destroy(data.assignedWall.gameObject);
                Debug.Log("Destoryed");
            }
            
            WallDatas.Remove(data);
            
            DirtyFlag_AvaliablePathsNearWall = true;
        }

        public WallAssignmentData GetWallDataByCellPos(Vector3Int cellPosition)
        {
            DirtyFlag_AvaliablePathsNearWall = true;
            return WallDatas.Find(x => x.CellPosition == cellPosition);
        }

        public WallAssignmentData GetWallDataByWall(Wall wall)
        {
            return WallDatas.Find(x => x.assignedWall.GetInstanceID() == wall.GetInstanceID());
        }

        public WallAssignmentData GetLastIndexWallData()
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

        public Vector3 SpawnPositon => EnterencePosition() - (IsWallDoorOnX ? new Vector3(0, 0, 3) : new Vector3(3, 0, 0));
    }
}