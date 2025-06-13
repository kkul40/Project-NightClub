using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace DiscoSystem.NewPathFinder
{
    [BurstCompile]
    public struct Node
    {
        public int2 Coordinates;
        public float G;
        public float H;
        public int2 Parent;

        public float F => G + H;

        public Node(int2 coordinates)
        {
            Coordinates = coordinates;
            G = float.MaxValue;
            H = 0f;
            Parent = coordinates;
        }
        
        public bool2 Compare(int2 index) => Coordinates == index;
        public bool2 Compare(Node node) => node.Coordinates == Coordinates;
    }
    
    [BurstCompile]
    public struct AStarJob : IJob
    {
        public int2 startCoord;
        public int2 destCoord;
        public NativeArray<byte> validGrid; // 1=walkable, 0=not walkable
        public int gridWidth; // X
        public int gridHeight; // Y

        public NativeList<int3> indexPath; // Output Path

        private int ToIndex(int2 pos) => pos.x * gridWidth + pos.y;

        public void Execute()
        {
            var allNodes = new NativeHashMap<int2, Node>(gridWidth * gridHeight, Allocator.Temp);
            var openSet = new NativeList<int2>(Allocator.Temp);
            var closedSet = new NativeHashSet<int2>(gridWidth * gridHeight, Allocator.Temp);

            // Node startNode = new Node(startCoord)
            // {
            //     G = 0,
            //     H = Heuristic(startCoord, destCoord)
            // };

            //
            Node startNode = new Node(FindNearestValidNode(startCoord));
            if (!math.all(startNode.Coordinates == new int2(-1,-1)))
            {
                startNode.G = 0;
                startNode.H = Heuristic(startCoord, destCoord);
            }
            destCoord = FindNearestValidNode(destCoord);
            if (!math.all(destCoord == new int2(-1, -1)))
            {
            }
            //

            allNodes[startCoord] = startNode;
            openSet.Add(startCoord);

            while (openSet.Length > 0)
            {
                // Find lowest F
                int lowestIndex = 0;
                float lowestF = allNodes[openSet[0]].F;

                for (int i = 1; i < openSet.Length; i++)
                {
                    float nodeF = allNodes[openSet[i]].F;
                    if (nodeF < lowestF)
                    {
                        lowestF = nodeF;
                        lowestIndex = i;
                    }
                }

                int2 currentCoord = openSet[lowestIndex];
                Node currentNode = allNodes[currentCoord];

                if (currentCoord.Equals(destCoord))
                {
                    ReconstructPath(allNodes, currentCoord);
                    allNodes.Dispose();
                    openSet.Dispose();
                    closedSet.Dispose();
                    return;
                }

                openSet.RemoveAtSwapBack(lowestIndex);
                closedSet.Add(currentCoord);

                foreach (var neighbor in GetNeighbors(currentCoord))
                {
                    if (!IsInsideGrid(neighbor)) continue;
                    if (!IsValid(neighbor)) continue;
                    if (closedSet.Contains(neighbor)) continue;

                    float tentativeG = currentNode.G + 1;

                    Node neighborNode;
                    if (!allNodes.TryGetValue(neighbor, out neighborNode))
                    {
                        neighborNode = new Node(neighbor)
                        {
                            H = Heuristic(neighbor, destCoord)
                        };
                    }

                    if (tentativeG < neighborNode.G)
                    {
                        neighborNode.G = tentativeG;
                        neighborNode.Parent = currentCoord;
                        allNodes[neighbor] = neighborNode;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }

            allNodes.Dispose();
            openSet.Dispose();
            closedSet.Dispose();
        }

        private void ReconstructPath(NativeHashMap<int2, Node> allNodes, int2 currentCoord)
        {
            var tempPath = new NativeList<int3>(Allocator.Temp);
            while (true)
            {
                tempPath.Add(new int3(currentCoord.x, 0, currentCoord.y));
                Node node = allNodes[currentCoord];
                if (node.Parent.Equals(currentCoord))
                    break;
                currentCoord = node.Parent;
            }
            for (int i = tempPath.Length - 1; i >= 0; i--)
                indexPath.Add(tempPath[i]);

            tempPath.Dispose();
        }

        private int2 FindNearestValidNode(int2 position)
        {
            NativeQueue<int2> nodeQueue = new NativeQueue<int2>(Allocator.TempJob);
            NativeHashSet<int2> visitedNodes = new NativeHashSet<int2>(50, Allocator.TempJob);

            nodeQueue.Enqueue(position);
            visitedNodes.Add(position);

            while (nodeQueue.Count > 0)
            {
                var current = nodeQueue.Dequeue();

                if (IsValid(current))
                {
                    nodeQueue.Dispose();
                    visitedNodes.Dispose();
                    return current;
                }

                foreach (var neighbor in GetNeighbors(current))
                {
                    if (!visitedNodes.Contains(neighbor))
                    {
                        nodeQueue.Enqueue(neighbor);
                        visitedNodes.Add(neighbor);
                    }
                }
            }
            
            nodeQueue.Dispose();
            visitedNodes.Dispose();
            return new int2(-1,-1);
        }

        private bool IsInsideGrid(int2 coord)
        {
            return coord.x >= 0 && coord.x < gridWidth && coord.y >= 0 && coord.y < gridHeight;
        }

        private bool IsValid(int2 coord)
        {
            if (!IsInsideGrid(coord)) return false;
            // if (coord.x > gridWidth || coord.y > gridHeight) return false;
            // if (coord.x < 1 || coord.y > 1) return false; // 1 Because There is Wall in 0s
            return validGrid[ToIndex(coord)] == 1;
        }

        private NativeArray<int2> GetNeighbors(int2 pos)
        {
            return new NativeArray<int2>(new int2[]
            {
                pos + new int2( 0,  1),
                pos + new int2( 0, -1),
                pos + new int2( 1,  0),
                pos + new int2(-1,  0)
            }, Allocator.Temp);
        }

        private float Heuristic(int2 a, int2 b)
        {
            return math.abs(a.x - b.x) + math.abs(a.y - b.y);
        }
    }
}