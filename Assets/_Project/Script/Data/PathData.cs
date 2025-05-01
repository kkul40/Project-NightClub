using System;
using System.Collections.Generic;
using Disco_ScriptableObject;
using DiscoSystem;
using DiscoSystem.Building_System.GameEvents;
using ExtensionMethods;
using Prop_Behaviours;
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
                if(isAvaliablePathsDirty)
                    UpdateAvaliableWallPaths();
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
            GameEvent.Subscribe<Event_MapSizeChanged>(handle => UpdateAllPaths());
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
                    PathFinderNode node = PathFinderNodes[x, y];
                    
                    bool isWalkable = true;
                    bool onlyEmployee = false;
                    bool onlyActivity = false;
                    
                    SetPathNode(node, ref isWalkable, ref onlyEmployee, ref onlyActivity);

                    node.IsWalkable = isWalkable;
                    node.OnlyEmployee = onlyEmployee;
                    node.OnlyActivity = onlyActivity;
                    
                    //
                    // PathFinderNodes[x, y].IsWalkable = IsWalkable(PathFinderNodes[x, y]);
                    // PathFinderNodes[x, y].OnlyEmployee = IsOnlyEmployee(PathFinderNodes[x, y]);
                    // PathFinderNodes[x, y].OnlyActivity = IsOnlyActivity(PathFinderNodes[x, y]);
                }
            }

            isPathsDirty = true;
            isAvaliablePathsDirty = true;
        }

        private void SetPathNode(PathFinderNode node, ref bool isWalkable, ref bool onlyEmployee, ref bool onlyActivity)
        {
            Ray ray = new Ray(node.WorldPos.Add(y:-0.5f), Vector3.up);
            Debug.DrawRay(ray.origin, ray.direction * 2, Color.red);
            var colliders = Physics.RaycastAll(ray.origin, Vector3.up, ConstantVariables.DoorHeight + 0.4f);

            bool dontCheckWalkable = false;
            bool dontCheckEmployee = false;
            bool dontCheckActivity = false;
            
            foreach (var hit in colliders)
            {
                if (!dontCheckEmployee)
                {
                    if (hit.transform.TryGetComponent(out OnlyEmployeeColider empColl))
                        if (empColl.onlyEmployeeCollider == hit.collider)
                        {
                            onlyEmployee = true;
                            isWalkable = true;
                            
                            dontCheckWalkable = true;
                            dontCheckEmployee = true;
                        }
                }
                
                if (hit.transform.TryGetComponent(out IPropUnit unit))
                {
                    if (!dontCheckActivity)
                    {
                        StoreItemSO item = GameBundle.Instance.FindAItemByID(unit.ID);
                        if (item is PlacementItemSO placemen)
                        {
                            if (placemen.OnlyForActivity)
                            {
                                onlyActivity = true;
                                dontCheckActivity = true;
                            }
                        }
                    }
                    
                    if (!dontCheckWalkable)
                    {
                        if (!CheckWalkable(unit))
                        {
                            isWalkable = false;
                            dontCheckWalkable = true;
                        }
                    }
                }
            }
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
                    StoreItemSO item = GameBundle.Instance.FindAItemByID(unit.ID);
                    if (item is PlacementItemSO placemen)
                    {
                        if (placemen.OnlyForActivity)
                        {
                            Debug.Log(placemen.Name);
                            Debug.Log("Only For Activity Set");
                            node.OnlyActivity = true;
                        }
                    }
                    
                    if (!CheckWalkable(unit))
                        return false;
                }
            }
            node.OnlyActivity = false;
            return true;
        }
        
        private bool IsOnlyEmployee(PathFinderNode node)
        {
            Ray ray = new Ray(node.WorldPos.Add(y:-0.5f), Vector3.up);
            Debug.DrawRay(ray.origin, ray.direction * 2, Color.red);
            var colliders = Physics.RaycastAll(ray.origin, Vector3.up, 2);

            foreach (var hit in colliders)
            {
                if (hit.transform.TryGetComponent(out OnlyEmployeeColider empColl))
                    if (empColl.onlyEmployeeCollider == hit.collider)
                    {
                        node.IsWalkable = true;
                        return true;
                    }
            }
            return false;
        }
        
        private bool IsOnlyActivity(PathFinderNode node)
        {
            Ray ray = new Ray(node.WorldPos.Add(y:-0.5f), Vector3.up);
            Debug.DrawRay(ray.origin, ray.direction * 2, Color.red);
            var colliders = Physics.RaycastAll(ray.origin, Vector3.up, 2);

            foreach (var hit in colliders)
            {
                if (hit.transform.TryGetComponent(out IPropUnit unit))
                {
                    StoreItemSO item = GameBundle.Instance.FindAItemByID(unit.ID);
                    if (item is PlacementItemSO placemen)
                        if (placemen.OnlyForActivity) return true;
                }
            }
            return false;
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

                    int dX = x;
                    int dY = y;

                    if (!_mapData.IsWallDoorOnX)
                    {
                        dX = y;
                        dY = x;
                    }
                    
                    if (dX > _mapData.WallDoorIndex * ConstantVariables.PathFinderGridSize - ConstantVariables.PathFinderGridSize && dX < _mapData.WallDoorIndex * ConstantVariables.PathFinderGridSize && dY == 0) continue;
                    
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
        
        public void UpdateAvaliableWallPaths()
        {
            AvaliableWallPaths = new List<PathFinderNode>();

            Vector2Int pathSize = PathFinderSize();
            
            int howFarFromWall = 1;
            for (int x = ConstantVariables.PathFinderGridSize / 2; x < pathSize.x; x += ConstantVariables.PathFinderGridSize)
            {
                PathFinderNode node = PathFinderNodes[x, howFarFromWall];
                if (node.GetIsWalkable && !node.OnlyEmployee && !node.OnlyActivity)
                    AvaliableWallPaths.Add(node);
            }
            for (int y = ConstantVariables.PathFinderGridSize / 2; y < pathSize.y; y += ConstantVariables.PathFinderGridSize)
            {
                PathFinderNode node = PathFinderNodes[howFarFromWall, y];
                if (node.GetIsWalkable && !node.OnlyEmployee && !node.OnlyActivity)
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

            isAvaliablePathsDirty = false;
        }
    }
}