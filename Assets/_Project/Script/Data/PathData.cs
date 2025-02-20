using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DefaultNamespace._Refactored.Event;
using DiscoSystem;
using ExtensionMethods;
using PlayerScripts;
using PropBehaviours;
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


            Init();
            // PlacementDataHandler.OnPropPlaced += () => SetFlags(avaliablePathFlag:true);
            KEvent_Building.OnPlacementPlaced += UpdateAllPaths;
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

        private async void UpdateAllPaths()
        {
            Vector2Int size = PathFinderSize();
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    PathFinderNodes[x, y].IsWalkable = await IsWalkable(PathFinderNodes[x, y]);
                }
            }
        }

        private async Task<bool> IsWalkable(PathFinderNode node)
        {
            Ray ray = new Ray(node.WorldPos.Add(y:-0.5f), Vector3.up);
            Debug.DrawRay(ray.origin, ray.direction * 2, Color.red, 0.2f);
            var colliders = Physics.RaycastAll(ray.origin, Vector3.up, ConstantVariables.DoorHeight+ 0.4f);

            foreach (var hit in colliders)
            {
                if (hit.transform.TryGetComponent(out IPropUnit unit))
                {
                    if(unit.IsInitialized)
                        return false;
                }
            }
            return true;
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
        
        public PathFinderNode GetPathNodeByWorldPos(Vector3 worldPos)
        {
            var convert = worldPos.WorldPosToCellPos(eGridType.PathFinderGrid);
            return PathFinderNodes[convert.x, convert.z];
        }
        
        public PathFinderNode[,] GetUsablePathFinderNodes()
        {
            Vector2Int mapSize = PathFinderSize();
            var outputNode = new PathFinderNode[mapSize.x, mapSize.y];

            for (var x = 0; x < mapSize.x; x++)
            for (var y = 0; y < mapSize.y; y++)
            {
                outputNode[x, y] = PathFinderNodes[x, y].Copy();
                    
                if (x > _mapData.WallDoorIndex * ConstantVariables.PathFinderGridSize - ConstantVariables.PathFinderGridSize && 
                    x < _mapData.WallDoorIndex * ConstantVariables.PathFinderGridSize && 
                    y == 0) continue;
                
                if (x == 0 || y == 0 || x == mapSize.x - 1 || y == mapSize.y - 1)
                {
                    outputNode[x, y].IsWall = true;
                }
            }
            
            return outputNode;
        }
        
        public void SetFlags(bool? avaliablePathFlag)
        {
            isAvaliablePathsDirty = avaliablePathFlag ?? false;
        }
    }
}