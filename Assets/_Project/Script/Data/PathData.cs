using System;
using System.Building_System.GameEvents;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExtensionMethods;
using PropBehaviours;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Data
{
    public class PathData
    {
        private MapData _mapData;
        private PathFinderNode[,] PathFinderNodes;

        private PathFinderNode[,] CachedPaths;

        private bool isPathsDirty = true;
        
        private List<PathFinderNode> AvaliableWallPaths;

        // Flags
        private bool isAvaliablePathsDirty = true;
        
        public List<PathFinderNode> GetAvaliableWallPaths
        {
            get
            {
                return AvaliableWallPaths;
            }
        }
        
        public PathData(int sizeX, int sizeY, MapData mapData)
        {
            PathFinderNodes = new PathFinderNode[sizeX, sizeY];
            _mapData = mapData;

            AvaliableWallPaths = new List<PathFinderNode>();
            CachedPaths = new PathFinderNode[sizeX, sizeY];

            SetUpPaths();
            // PlacementDataHandler.OnPropPlaced += () => SetFlags(avaliablePathFlag:true);
            GameEvent.Subscribe<Event_PropPlaced>( handle => UpdateAllPaths());
            GameEvent.Subscribe<Event_PropRemoved>( handle => UpdateAllPaths());
            GameEvent.Subscribe<Event_PropRelocated>(handle => UpdateAllPaths());
            GameEvent.Subscribe<Event_ExpendMapSize>(handle => UpdateAllPaths());
        }

        private void SetUpPaths()
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
                    node.IsWalkable = true;
                    node.GridX = x;
                    node.GridY = y;
                    node.WorldPos = new Vector3Int(x, 0, y).CellCenterPosition(eGridType.PathFinderGrid);

                    PathFinderNodes[x, y] = node;
                }
            }

            isPathsDirty = true;
        }

        private void UpdateAllPaths()
        {
            Vector2Int size = PathFinderSize();
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    PathFinderNodes[x, y].IsWalkable = IsWalkable(PathFinderNodes[x, y]);
                }
            }

            isPathsDirty = true;
        }

        private bool IsWalkable(PathFinderNode node)
        {
            Ray ray = new Ray(node.WorldPos.Add(y:-0.5f), Vector3.up);
            Debug.DrawRay(ray.origin, ray.direction * 2, Color.red);
            var colliders = Physics.RaycastAll(ray.origin, Vector3.up, ConstantVariables.DoorHeight + 0.4f);

            foreach (var hit in colliders)
            {
                if (hit.transform.TryGetComponent(out IPropUnit unit))
                {
                    if (!CheckWalkable(unit)) return false;
                }
            }
            return true;
        }

        private bool CheckWalkable(IPropUnit unit)
        {
            if (unit.IsInitialized)
            {
                if (unit.PlacementLayer != ePlacementLayer.BaseSurface) return false;
            }
            
            return true;
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
        
        public PathFinderNode[,] GetPaths()
        {
            if (isPathsDirty)
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

                CachedPaths = outputNode;
                isPathsDirty = false;
                return CachedPaths;
            }
            
            return CachedPaths;
        }
        
        public void SetFlags(bool? avaliablePathFlag)
        {
            isAvaliablePathsDirty = avaliablePathFlag ?? false;
        }
    }
}