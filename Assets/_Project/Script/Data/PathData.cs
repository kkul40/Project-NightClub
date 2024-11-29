using System;
using System.Collections.Generic;
using ExtensionMethods;
using NUnit.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Data
{
    public class PathData
    {
        private MapData _mapData;
        private PathFinderNode[,] PathFinderNodes;
        
        private List<PathFinderNode> AvaliableWallPaths;

        // Flags
        private bool isAvaliablePathsDirty = true;
        
        public List<PathFinderNode> GetAvaliableWallPaths
        {
            get
            {
                if (isAvaliablePathsDirty)
                {
                    UpdateAvaliableWallPaths();
                    isAvaliablePathsDirty = false;
                }
                
                return AvaliableWallPaths;
            }
        }
        
        public PathData(int sizeX, int sizeY, MapData mapData)
        {
            PathFinderNodes = new PathFinderNode[sizeX, sizeY];
            _mapData = mapData;

            PlacementDataHandler.OnPropPlaced += () => SetFlags(avaliablePathFlag:true);
            Init();
        }

        private void Init()
        {
            int MaxX = ConstantVariables.MaxMapSizeX * ConstantVariables.PathFinderGridSize + 1;
            int MaxY = ConstantVariables.MaxMapSizeY * ConstantVariables.PathFinderGridSize + 1;
            PathFinderNodes = new PathFinderNode[MaxX, MaxY];

            for (var x = 0; x < MaxX; x++)
            {
                for (var y = 0; y < MaxY; y++)
                {
                    var node = new PathFinderNode();
                    node.Init();
                    node.IsWalkable = false;
                    node.GridX = x;
                    node.GridY = y;
                    node.WorldPos = new Vector3Int(x, 0, y).CellCenterPosition(eGridType.PathFinderGrid);

                    PathFinderNodes[x, y] = node;
                }
            }
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
                PathFinderNodes[i,j].IsWall = isWall ?? PathFinderNodes[i,j].IsWall;
                PathFinderNodes[i,j].IsWalkable = isWalkable ?? PathFinderNodes[i,j].IsWalkable;
            }

            // Flag
            isAvaliablePathsDirty = true;
        }
        
        public void UpdateAvaliableWallPaths()
        {
            AvaliableWallPaths = new List<PathFinderNode>();

            Vector2Int pathSize = PathFinderSize();
            
            int howFarFromWall = 1;
            for (int x = ConstantVariables.PathFinderGridSize / 2; x < pathSize.x; x += ConstantVariables.PathFinderGridSize)
            {
                PathFinderNode node = PathFinderNodes[x, howFarFromWall];
                if (!ContainsKeyOnPath(node))
                    AvaliableWallPaths.Add(node);
            }
            for (int y = ConstantVariables.PathFinderGridSize / 2; y < pathSize.y; y += ConstantVariables.PathFinderGridSize)
            {
                PathFinderNode node = PathFinderNodes[howFarFromWall, y];
                if (!ContainsKeyOnPath(node))
                    AvaliableWallPaths.Add(node);
            }

            for (int i = AvaliableWallPaths.Count - 1; i >= 0; i--)
            {
                if (AvaliableWallPaths[i].WorldPos.WorldPosToCellPos(eGridType.PlacementGrid) == _mapData.EnterencePosition().WorldPosToCellPos(eGridType.PlacementGrid))
                {
                    AvaliableWallPaths.RemoveAt(i);
                    break;
                }
            }
        }
        
        private bool ContainsKeyOnPath(PathFinderNode node)
        {
            if(DiscoData.Instance.placementDataHandler.ContainsKey(node.WorldPos.WorldPosToCellPos(eGridType.PlacementGrid), ePlacementLayer.BaseSurface))
                return true;
            
            if(DiscoData.Instance.placementDataHandler.ContainsKey(node.WorldPos.WorldPosToCellPos(eGridType.PlacementGrid), ePlacementLayer.FloorProp))
                return true;

            if (DiscoData.Instance.placementDataHandler.ContainsKeyOnWall(node.WorldPos.WorldPosToCellPos(eGridType.PlacementGrid), 2))
                return true;

            return false;
        }

        public Vector2Int PathFinderSize()
        {
            Vector2Int mapSize = _mapData.CurrentMapSize;
            return new Vector2Int((mapSize.x * ConstantVariables.PathFinderGridSize) + 1, (mapSize.y * ConstantVariables.PathFinderGridSize) + 1);
        }
        
        public PathFinderNode GetPath(int x, int y)
        {
            return PathFinderNodes[x, y];
        }

        public PathFinderNode GetRandomPathNode()
        {
            Vector2Int Size = PathFinderSize();
            return PathFinderNodes[Random.Range(0, Size.x), Random.Range(0, Size.y)];
        }
        
        public void SetFlags(bool? avaliablePathFlag)
        {
            isAvaliablePathsDirty = avaliablePathFlag ?? false;
        }
    }
}