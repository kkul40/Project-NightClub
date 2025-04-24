using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using ExtensionMethods;
using UnityEngine;

namespace DiscoSystem.Character
{
    public class NpcPathFinder : IPathFinder
    {
        public Transform FinderTransform { get; }
        public Vector3 TargetPosition { get; }
       
        public PathUserType PathUserType { get; }
        public bool HasReachedDestination { get; private set; }
        public List<Vector3> GetWayPoints => _wayPoints;

        private PathFinderNode[,] _tileNode;
        private List<Vector3> _wayPoints;
        private IEnumerator _routine;

        public NpcPathFinder(Transform finderTransform, PathUserType pathUserType)
        {
            FinderTransform = finderTransform;
            PathUserType = pathUserType;
            TargetPosition = -Vector3.one;
            HasReachedDestination = false;

            _wayPoints = new List<Vector3>();
        }

        public bool IsPathAvaliable(Vector3 destination)
        {
            _wayPoints = FindPath(FinderTransform.position, destination);
            if (_wayPoints.Count == 0) return false;

            return true;
        }

        public PathFinderNode GetDestinationNode()
        {
            if (_wayPoints.Count > 0) return WorldPointToNode(_wayPoints[^1]);

            return null;
        }
        
        public bool GoToDestination(Vector3 destination, Action onCompleteCallBack = null)
        {
            Cancel();

            if (destination.WorldPosToCellPos(eGridType.PathFinderGrid) ==
                FinderTransform.position.WorldPosToCellPos(eGridType.PlacementGrid)) return true;

            _wayPoints = FindPath(FinderTransform.position, destination);

            if (_wayPoints == null || _wayPoints.Count == 0)
                return false;

            _routine = CoFollowPath(_wayPoints, onCompleteCallBack);
            DiscoData.Instance.StartCoroutine(_routine);
            
            return true;
        }

        public bool GoToRandomDestination()
        {
            _tileNode = DiscoData.Instance.MapData.Path.GetPaths();

            int iteration = 0;
            while (true)
            {
                if (Helper.IterateTo100(ref iteration)) return false;
                
                PathFinderNode target = DiscoData.Instance.MapData.GetRandomPathFinderNode();
                
                if(target.OnlyActivity) continue;

                if (IsPathAvaliable(target.WorldPos))
                {
                    GoToDestination(target.WorldPos);
                    return true;
                }
            }
        }

        private IEnumerator CoFollowPath(List<Vector3> path, Action onCompleteCallBack = null)
        {
            HasReachedDestination = false;

            for (var i = 0; i < path.Count; i++)
            {
                yield return null;

                var newPath = path[i];
                SetRotationToTarget(newPath);

                while (Vector3.Distance(FinderTransform.position, newPath) > 0.1f)
                {
                    FinderTransform.position = Vector3.MoveTowards(FinderTransform.position, newPath, Time.deltaTime * 1.5f);
                    yield return null;
                }
            }

            _wayPoints.Clear();
            HasReachedDestination = true;
            onCompleteCallBack?.Invoke();
        }
        
        private void SetRotationToTarget(Vector3 lookatTarget)
        {
            var direction = lookatTarget - FinderTransform.position;
            direction.Normalize();
            var lookRotation = Quaternion.LookRotation(direction);
            FinderTransform.DORotate(lookRotation.eulerAngles, 0.5f);
        }
        
        private List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos)
        {
            _tileNode = DiscoData.Instance.MapData.Path.GetPaths();

            var startNode = WorldPointToNode(startPos);
            var targetNode = WorldPointToNode(targetPos);
            
            if (!startNode.GetIsWalkable)
            {
                startNode = FindNearestWalkableNode(startNode);
                if (startNode == null)
                {
                    Debug.Log("No walkable start node found, return null");
                    return new List<Vector3>(); // No walkable start node found, return null
                }
            }
            
            if (!targetNode.GetIsWalkable || !IsUserAllowed(targetNode))
            {
                targetNode = FindNearestWalkableNode(targetNode);
                if (targetNode == null)
                {
                    Debug.Log("No walkable target node found, return null");
                    return new List<Vector3>(); // No walkable target node found, return null
                }
            }
            
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
                    if(!IsUserAllowed(neighbor)) continue;

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

            Debug.Log("No Path Found");
            return new List<Vector3>();
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

            return waypoints;
        }
        
        private PathFinderNode FindNearestWalkableNode(PathFinderNode startNode)
        {
            Queue<PathFinderNode> nodeQueue = new Queue<PathFinderNode>();
            HashSet<PathFinderNode> visitedNodes = new HashSet<PathFinderNode>();

            nodeQueue.Enqueue(startNode);
            visitedNodes.Add(startNode);

            while (nodeQueue.Count > 0)
            {
                var currentNode = nodeQueue.Dequeue();

                if (currentNode.GetIsWalkable && IsUserAllowed(currentNode))
                {
                    Debug.DrawRay(currentNode.WorldPos, Vector3.up, Color.magenta, 2f);
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
        
        private List<PathFinderNode> GetNeighbors(PathFinderNode node)
        {
            var neighbors = new List<PathFinderNode>();
            var size = DiscoData.Instance.MapData.PathFinderSize;

            for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.GridX + x;
                int checkY = node.GridY + y;

                if (checkX >= 0 && checkX < size.x && checkY >= 0 && checkY < size.y)
                {
                    neighbors.Add(_tileNode[checkX, checkY]);
                }
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
        
        private PathFinderNode WorldPointToNode(Vector3 worldPosition)
        {
            if (worldPosition == -Vector3.one) return new PathFinderNode();
            
            var cell = worldPosition.WorldPosToCellPos(eGridType.PathFinderGrid);

            cell.x = Mathf.Clamp(cell.x, 0, DiscoData.Instance.MapData.PathFinderSize.x -1);
            cell.z = Mathf.Clamp(cell.z, 0, DiscoData.Instance.MapData.PathFinderSize.y -1);
            
            return _tileNode[cell.x, cell.z];
        }
        
        private PathFinderNode FindNearestValidNodeWithinGrid(Vector3 targetPos)
        {
            // Convert the target position to a node in the grid
            var nearestNode = WorldPointToNode(targetPos);
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
        
        private bool IsNodeWithinGrid(PathFinderNode node)
        {
            if (node == null) return false;
            if (node.GridX > DiscoData.Instance.MapData.PathFinderSize.x || node.GridX < 0) return false;
            if (node.GridY > DiscoData.Instance.MapData.PathFinderSize.y || node.GridY < 0) return false;

            return true;
        }

        public void Cancel()
        {
            if (_routine == null) return;

            HasReachedDestination = false;
            DiscoData.Instance?.StopCoroutine(_routine);
            _wayPoints.Clear();
            _routine = null;
        }

        public void ErrorHelp(Action onCompleteCallBack = null)
        {
            Cancel();
            
            var startNode = WorldPointToNode(FinderTransform.position);
            Debug.Log(startNode.WorldPos);

            var nearestOne = FindNearestWalkableNode(startNode);
            
            _wayPoints.Clear();
            _wayPoints.Add(nearestOne.WorldPos);
            
            Debug.Log(nearestOne);
            
            _routine = CoFollowPath(_wayPoints, onCompleteCallBack);
            DiscoData.Instance.StartCoroutine(_routine);
        }

        public bool IsUserAllowed(PathFinderNode node)
        {
            switch (PathUserType)
            {
                case PathUserType.Player:
                    return true;
                case PathUserType.Employee:
                    return true;
                case PathUserType.Customer:
                    if (node.OnlyEmployee) return false;
                    return true;
            }

            return true;
        }
    }
}