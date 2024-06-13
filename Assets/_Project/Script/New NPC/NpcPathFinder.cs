using System.Collections.Generic;
using Data;
using UnityEngine;

namespace New_NPC
{
    public class NpcPathFinder
    {
        // TODO Use DirthFlag Here
        private TileNode[,] tileNode;
        
        public NpcPathFinder()
        {
            tileNode = DiscoData.Instance.mapData.TileNodes;
        }
        
        public List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos)
        {
            tileNode = DiscoData.Instance.mapData.TileNodes;

            TileNode startNode = NodeFromWorldPoint(startPos);
            TileNode targetNode = NodeFromWorldPoint(targetPos);

            List<TileNode> openSet = new List<TileNode>();
            HashSet<TileNode> closedSet = new HashSet<TileNode>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                TileNode currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost)
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    return RetracePath(startNode, targetNode);
                }

                foreach (TileNode neighbor in GetNeighbors(currentNode))
                {
                    if (!neighbor.IsWalkable || closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);
                    if (newMovementCostToNeighbor < neighbor.GCost || !openSet.Contains(neighbor))
                    {
                        neighbor.GCost = newMovementCostToNeighbor;
                        neighbor.HCost = GetDistance(neighbor, targetNode);
                        neighbor.Parent = currentNode;

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }

            Debug.Log("No Path Found");
            return null; // Return null if no path is found
        }
        
        public TileNode NodeFromWorldPoint(Vector3 worldPosition)
        {
            int x = (int)worldPosition.x;
            int y = (int)worldPosition.z;
            return tileNode[x, y];
        }
        
        List<Vector3> RetracePath(TileNode startNode, TileNode endNode)
        {
            List<TileNode> path = new List<TileNode>();
            TileNode currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            path.Reverse();

            List<Vector3> waypoints = new List<Vector3>();
            foreach (TileNode node in path)
            {
                waypoints.Add(node.WorldPos);
            }

            return waypoints; // Return the path as a list of Vector3 positions
        }
        
        public List<TileNode> GetNeighbors(TileNode node)
        {
            List<TileNode> neighbors = new List<TileNode>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.GridX + x;
                    int checkY = node.GridY + y;

                    if (checkX >= 0 && checkX < DiscoData.MapData.MaxMapSizeX && checkY >= 0 && checkY < DiscoData.MapData.MaxMapSizeY)
                    {
                        neighbors.Add(tileNode[checkX, checkY]);
                    }
                }
            }
            return neighbors;
        }

        int GetDistance(TileNode nodeA, TileNode nodeB)
        {
            int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
            int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }

    

    public enum eTileNode
    {
        Walkable,
        Blocked,
        Closed
    }
}