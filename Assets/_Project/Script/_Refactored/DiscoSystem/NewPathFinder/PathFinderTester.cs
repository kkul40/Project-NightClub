using System.Collections.Generic;
using Data;
using ExtensionMethods;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace DiscoSystem.NewPathFinder
{
    public class PathFinderTester : MonoBehaviour
    {
        public static PathFinderTester Instance;
        public PathGrid grid;
        public NativePathFinder finder;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            grid = new PathGrid();
            finder = new NativePathFinder();
        }
    }

    public class NativePathFinder
    {
        public List<Vector3> StartAStarJob(Vector3 startPos, Vector3 endPos)
        {
            int gridWidth = Path.PathFinderSize().x;
            int gridHeight = Path.PathFinderSize().y;

            NativeArray<byte> validGrid = new NativeArray<byte>(gridWidth * gridHeight, Allocator.TempJob);
            
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    Vector2Int cell = new Vector2Int(x, y);
                    bool isValid = IsValid(cell);
                    int index = x * gridWidth + y;
                    validGrid[index] = (byte)(isValid ? 1 : 0);
                }
            }

            var path = new NativeList<float3>(Allocator.TempJob);

            AStarJob job = new AStarJob
            {
                startCoord = WorldPosToGrid(startPos),
                destCoord = WorldPosToGrid(endPos),
                gridWidth = gridWidth,
                gridHeight = gridHeight,
                validGrid = validGrid,
                path = path
            };

            JobHandle handle = job.Schedule();
            handle.Complete();
            
            List<Vector3> result = new List<Vector3>();
            foreach (var pos in path)
            {
                Debug.DrawRay(new Vector3(pos.x, 0, pos.z) / ConstantVariables.PathFinderGridSize, Vector3.up * 2f, Color.green, 5f);
                result.Add(new Vector3(pos.x, 0, pos.z) / ConstantVariables.PathFinderGridSize);
            }
            
            path.Dispose();
            validGrid.Dispose();
            return result;
        }

        private int2 WorldPosToGrid(Vector3 pos)
        {
            var index = pos.WorldPosToCellPos(eGridType.PathFinderGrid);
            return new int2(index.x, index.z);
        }

        public NativeList<Vector3> FindPath(Vector3 startPosition, Vector3 destination, Allocator allocator)
        {
            NativeHashMap<Vector2Int, Node> allNodes = new NativeHashMap<Vector2Int, Node>();
            NativeList<Vector2Int> openSet = new NativeList<Vector2Int>();
            NativeHashSet<Vector2Int> closedSet = new NativeHashSet<Vector2Int>();

            Vector2Int startCoord = CreateNodeCoordinates(startPosition);
            Vector2Int destCoord = CreateNodeCoordinates(destination);

            Node startNode = new Node(new int2(startCoord.x, startCoord.y))
            {
                G = 0,
                H = Heuristic(startCoord, destCoord)
            };
            
            allNodes[startCoord] = startNode;
            openSet.Add(startCoord);

            while (openSet.Length > 0)
            {
                int lowestIndex = 0;
                float lowestF = allNodes[openSet[0]].F;
                for (int i = 1; i < openSet.Length; i++)
                {
                    var nodeF = allNodes[openSet[i]].F;
                    if (nodeF < lowestF)
                    {
                        lowestF = nodeF;
                        lowestIndex = i;
                    }
                }
                
                Vector2Int currentCoord = openSet[lowestIndex];
                Node current = allNodes[currentCoord];

                if (currentCoord == destCoord)
                {
                    var path = ReconstructPath(allNodes, currentCoord, allocator);

                    allNodes.Dispose();
                    openSet.Dispose();
                    closedSet.Dispose();
                    return path;
                }
                
                // Debug.DrawRay(currentCoord.CoordinatesToWorldPosition(), Vector3.up * 0.2f, Color.green, 2f);

                openSet.RemoveAt(lowestIndex);
                closedSet.Add(currentCoord);

                foreach (var neighborCoord in GetNeighbors(currentCoord))
                {
                    if (closedSet.Contains(neighborCoord)) continue;
                    if (!IsValid(neighborCoord)) continue;

                    float tentativeG = current.G + 1;

                    Node neighborNode;
                    if (!allNodes.TryGetValue(neighborCoord, out neighborNode))
                    {
                        neighborNode = new Node(new int2(neighborCoord.x, neighborCoord.y))
                        {
                            H = Heuristic(neighborCoord, destCoord)
                        };
                    }

                    if (tentativeG < neighborNode.G)
                    {
                        neighborNode.G = tentativeG;
                        neighborNode.Parent = new int2(neighborCoord.x, neighborCoord.y);
                        allNodes[neighborCoord] = neighborNode;

                        if (!openSet.Contains(neighborCoord))
                            openSet.Add(neighborCoord);
                    }
                }
            }

            Debug.LogError("Path Not Found");
            allNodes.Dispose();
            openSet.Dispose();
            closedSet.Dispose();
            return new NativeList<Vector3>(allocator);
        }

        private NativeList<Vector3> ReconstructPath(NativeHashMap<Vector2Int, Node> allNodes, Vector2Int currentCoord, Allocator allocator)
        {
            NativeList<Vector3> path = new NativeList<Vector3>();

            while (currentCoord != -Vector2Int.one)
            {
                path.Add(currentCoord.CoordinatesToWorldPosition());
                int2 parent = allNodes[currentCoord].Parent;
                currentCoord = new Vector2Int(parent.x, parent.y);
            }

            NativeList<Vector3> reversePath = new NativeList<Vector3>();

            for (int i = path.Length -1; i >= 0; i--)
                reversePath.Add(path[i]);
            // foreach (var cor in path)
            // {
            //     Debug.DrawRay(cor, Vector3.up * 2f, Color.magenta, 5);
            // }
            Debug.LogError("Path Generated");
            return reversePath;
        }

        private float Heuristic(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }
        
        private NativeArray<Vector2Int> GetNeighbors(Vector2Int pos)
        {
            // No allocation - fixed size
            return new NativeArray<Vector2Int>(new Vector2Int[]
            {
                pos + Vector2Int.up,
                pos + Vector2Int.down,
                pos + Vector2Int.left,
                pos + Vector2Int.right
            }, Allocator.Temp);
        }

        private Vector2Int CreateNodeCoordinates(Vector3 position)
        {
            var index = position.WorldPosToCellPos(eGridType.PathFinderGrid);
            index.x = Mathf.Clamp(index.x, 0, DiscoData.Instance.MapData.PathFinderSize.x -1);
            index.z = Mathf.Clamp(index.z, 0, DiscoData.Instance.MapData.PathFinderSize.y -1);
            return new Vector2Int(index.x, index.z);
        }

        private bool IsValid(Vector2Int index)
        {
            PathFinderNode node = GetNode(index);
            if (node.IsWall) return false;
            
            return node.IsWalkable;
        }

        private PathData Path => DiscoData.Instance.MapData.Path;
        
        private PathFinderNode GetNode(Vector2Int index)
        {
            return Path.GetPath(index.x, index.y);
        }
    }
}