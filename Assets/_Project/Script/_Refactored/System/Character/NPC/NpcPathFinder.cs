using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using ExtensionMethods;
using UnityEngine;

namespace System.Character.NPC
{
    public class NpcPathFinder : IPathFinder
    {
        // TODO Use DirthFlag Here
        private PathFinderNode[,] _tileNode;
        private Transform _assignedNPC;
        public List<Vector3> FoundPath { get; private set; }

        private IEnumerator _routine = null;

        public Transform mTransform => _assignedNPC.transform;

        public bool HasReachedDestination { get; private set; } = false;
        public Vector3 TargetPosition { get; private set; }

        private float animationTweak = 0.5f;

        public NpcPathFinder(Transform assign)
        {
            _assignedNPC = assign;
            TargetPosition = -Vector3.one;
        }

        public bool CheckIfPathAvaliable(Vector3 targetDestination)
        {
            var path = FindPath(_assignedNPC.position, targetDestination);

            if (path == null)
            {
                Debug.Log("No Pathf Found : " + targetDestination);
                return false;
            }

            return true;
        }

        public bool GoTargetDestination(Vector3 targetPos, Action OnCompleteCallBack = null)
        {
            CancelDestination();
            
            FoundPath = FindPath(_assignedNPC.position, targetPos);

            if (FoundPath == null)
            {
                Debug.Log("No Pathf Found : " + targetPos);
                return false;
            }

            TargetPosition = targetPos;
            _routine = CoFollowPath(FoundPath, OnCompleteCallBack);
            DiscoData.Instance.StartCoroutine(_routine);
            return true;
        }

        public void CancelDestination()
        {
            if (_routine == null) return;

            HasReachedDestination = false;
            DiscoData.Instance.StopCoroutine(_routine);
            FoundPath = new List<Vector3>();
            _routine = null;
        }

        private IEnumerator CoFollowPath(List<Vector3> path, Action OnCompleteCallBack = null)
        {
            for (var i = 0; i < path.Count; i++)
            {
                yield return null;

                var newPath = path[i];
                SetRotationToTarget(newPath);
                while (Vector3.Distance(_assignedNPC.position, newPath) > 0.1f)
                {
                    _assignedNPC.position = Vector3.MoveTowards(_assignedNPC.position, newPath, Time.deltaTime * 1.5f);
                    yield return null;
                }
            }

            FoundPath = new List<Vector3>();
            HasReachedDestination = true;
            OnCompleteCallBack?.Invoke();
        }

        private void SetRotationToTarget(Vector3 lookatTarget)
        {
            var direction = lookatTarget - _assignedNPC.position;
            direction.Normalize();
            var lookRotation = Quaternion.LookRotation(direction);
            _assignedNPC.DORotate(lookRotation.eulerAngles, animationTweak);
        }

        private List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos)
        {
            _tileNode = DiscoData.Instance.MapData.Path.GetPaths();

            var startNode = NodeFromWorldPoint(startPos);
            var targetNode = NodeFromWorldPoint(targetPos);

            if (!startNode.GetIsWalkable)
            {
                startNode = FindNearestWalkableNode(startNode);
                if (startNode == null)
                {
                    Debug.Log("No walkable start node found, return null");
                    return null; // No walkable start node found, return null
                }
            }

            if (!targetNode.GetIsWalkable)
            {
                targetNode = FindNearestWalkableNode(targetNode);
                if (startNode == null)
                {
                    Debug.Log("No walkable target node found, return null");
                    return null; // No walkable target node found, return null
                }
            }
            
            // if (targetNode == null || !IsNodeWithinGrid(targetNode))
            // {
            //     targetNode = FindNearestValidNodeWithinGrid(targetPos);
            //     if (targetNode == null)
            //     {
            //         Debug.Log("No valid target node found, return null");
            //         return null; // No valid target node found, return null
            //     }
            // }
            
            var openSet = new List<PathFinderNode>();
            var closedSet = new HashSet<PathFinderNode>();
            openSet.Add(startNode);

            PathFinderNode closestNode = startNode;
            
            while (openSet.Count > 0)
            {
                var currentNode = openSet[0];
                for (var i = 1; i < openSet.Count; i++)
                    if (openSet[i].FCost < currentNode.FCost ||
                        (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost))
                        currentNode = openSet[i];

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);
                
                if (currentNode.HCost < closestNode.HCost)
                {
                    closestNode = currentNode;
                }

                if (currentNode == targetNode) return RetracePath(startNode, targetNode);

                foreach (var neighbor in GetNeighbors(currentNode))
                {
                    if (!neighbor.GetIsWalkable || closedSet.Contains(neighbor)) continue;

                    var newMovementCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);
                    if (newMovementCostToNeighbor < neighbor.GCost || !openSet.Contains(neighbor))
                    {
                        neighbor.GCost = newMovementCostToNeighbor;
                        neighbor.HCost = GetDistance(neighbor, targetNode);
                        neighbor.Parent = currentNode;

                        if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                    }
                }
            }

            return RetracePath(startNode, closestNode);;
        }
        
        private PathFinderNode FindNearestWalkableNode(PathFinderNode startNode)
        {
            // Use a queue to perform a breadth-first search
            Queue<PathFinderNode> nodeQueue = new Queue<PathFinderNode>();
            HashSet<PathFinderNode> visitedNodes = new HashSet<PathFinderNode>();

            // Start the search from the startNode
            nodeQueue.Enqueue(startNode);
            visitedNodes.Add(startNode);

            while (nodeQueue.Count > 0)
            {
                var currentNode = nodeQueue.Dequeue();

                // Check if the current node is walkable
                if (currentNode.IsWalkable && !currentNode.IsWall)
                {
                    return currentNode; // Found the nearest walkable node
                }

                // Enqueue all unvisited neighbors
                foreach (var neighbor in GetNeighbors(currentNode))
                {
                    if (!visitedNodes.Contains(neighbor))
                    {
                        nodeQueue.Enqueue(neighbor);
                        visitedNodes.Add(neighbor);
                    }
                }
            }

            return null; // No walkable node found
        }
        
        private bool IsNodeWithinGrid(PathFinderNode node)
        {
            if (node == null) return false;
            if (node.GridX > DiscoData.Instance.MapData.PathFinderSize.x || node.GridX < 0) return false;
            if (node.GridY > DiscoData.Instance.MapData.PathFinderSize.y || node.GridY < 0) return false;

            return true;
        }

        // Method to find the nearest valid (walkable) node within the grid if the target is outside the grid
        private PathFinderNode FindNearestValidNodeWithinGrid(Vector3 targetPos)
        {
            // Convert the target position to a node in the grid
            var nearestNode = NodeFromWorldPoint(targetPos);
            if (nearestNode != null && IsNodeWithinGrid(nearestNode) && nearestNode.IsWalkable && !nearestNode.IsWall)
            {
                return nearestNode;
            }

            // If the direct conversion doesn't yield a valid node, search for the nearest valid node
            var neighbors = GetNeighbors(nearestNode);
            PathFinderNode closestValidNode = null;
            float closestDistance = float.MaxValue;

            foreach (var neighbor in neighbors)
            {
                if (IsNodeWithinGrid(neighbor) && neighbor.IsWalkable && !neighbor.IsWall)
                {
                    var distance = GetDistance(nearestNode, neighbor);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestValidNode = neighbor;
                    }
                }
            }

            return closestValidNode;
        }

        private PathFinderNode NodeFromWorldPoint(Vector3 worldPosition)
        {
            if (worldPosition == -Vector3.one) return new PathFinderNode();
            
            // var cell = GridHandler.Instance.GetWorldToCell(worldPosition, eGridType.PathFinderGrid);
            var cell = worldPosition.WorldPosToCellPos(eGridType.PathFinderGrid);

            cell.x = Mathf.Clamp(cell.x, 0, DiscoData.Instance.MapData.PathFinderSize.x -1);
            cell.z = Mathf.Clamp(cell.z, 0, DiscoData.Instance.MapData.PathFinderSize.y -1);
            
            return _tileNode[cell.x, cell.z];
        }

        private List<Vector3> RetracePath(PathFinderNode startNode, PathFinderNode endNode)
        {
            var path = new List<PathFinderNode>();
            var currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            path.Reverse();

            var waypoints = new List<Vector3>();
            foreach (var node in path)
                waypoints.Add(node.WorldPos);

            return waypoints; // Return the path as a list of Vector3 positions
        }

        private List<Vector3> NullPathReturn(Vector3 target)
        {
            Debug.LogError("Path Could Not Found***");
            var path = new List<Vector3>();
            path.Add(target);
            return path;
        }

        private List<PathFinderNode> GetNeighbors(PathFinderNode node)
        {
            var neighbors = new List<PathFinderNode>();

            for (var x = -1; x <= 1; x++)
            for (var y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                var checkX = node.GridX + x;
                var checkY = node.GridY + y;

                if (checkX >= 0 && checkX < DiscoData.Instance.MapData.PathFinderSize.x - 1 && checkY >= 0 &&
                    checkY < DiscoData.Instance.MapData.PathFinderSize.y - 1)
                    neighbors.Add(_tileNode[checkX, checkY]);
            }

            return neighbors;
        }

        private int GetDistance(PathFinderNode nodeA, PathFinderNode nodeB)
        {
            var dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
            var dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

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