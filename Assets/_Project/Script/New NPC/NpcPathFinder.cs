using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Analytics;

namespace New_NPC
{
    public interface IPathFinder
    {
        bool HasReachedDestination { get; }
        bool GoTargetDestination(Vector3 targetDestination, bool checkNodes = true);
        void CancelDestination();
        void SetRotation(Quaternion newRotation);
    }

    public class BartenderPathFinder : IPathFinder
    {
        public bool HasReachedDestination { get; private set; }
        private Vector3 target;
        private Transform mTransform;

        public BartenderPathFinder(Transform mTransform)
        {
            this.mTransform = mTransform;
        }

        public bool GoTargetDestination(Vector3 targetDestination, bool checkNodes = true)
        {
            target = targetDestination;
            HasReachedDestination = false;
            SetRotationToTarget(target);
            DOTween.instance.StartCoroutine(CoWalkPosition());
            return true;
        }

        public void CancelDestination()
        {
        }

        public void SetRotation(Quaternion newRotation)
        {
            mTransform.DORotate(newRotation.eulerAngles, 0.5f);
        }
        
        private void SetRotationToTarget(Vector3 lookatTarget)
        {
            var direction = lookatTarget - mTransform.position;
            direction.Normalize();
            var lookRotation = Quaternion.LookRotation(direction);
            mTransform.DORotate(lookRotation.eulerAngles, 0.5f);
        }

        IEnumerator CoWalkPosition()
        {
            while (Vector3.Distance(mTransform.position, target) > 0.01f)
            {
                mTransform.position = Vector3.MoveTowards(mTransform.position, target, Time.deltaTime * 1.5f);
                yield return null;
            }
            HasReachedDestination = true;
        }
    }

    public class NpcPathFinder : IPathFinder
    {
        // TODO Use DirthFlag Here
        private PathFinderNode[,] _tileNode;
        private Transform _assignedNPC;
        private List<Vector3> _currentPath;

        private IEnumerator _routine = null;
        public bool HasReachedDestination => _routine == null;

        public NpcPathFinder(Transform assign)
        {
            _assignedNPC = assign;
        }

        public bool GoTargetDestination(Vector3 targetPos, bool checkNodes = true)
        {
            CancelDestination();

            if (checkNodes)
                _currentPath = FindPath(_assignedNPC.position, targetPos);
            else
                _currentPath = NullPathReturn(targetPos);

            _routine = CoFollowPath(_currentPath);
            DiscoData.Instance.StartCoroutine(_routine);
            return true;
        }

        public void CancelDestination()
        {
            if (_routine == null) return;

            DiscoData.Instance.StopCoroutine(_routine);
            _currentPath = new List<Vector3>();
            _routine = null;
        }

        private IEnumerator CoFollowPath(List<Vector3> path)
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

            _routine = null;
        }

        private void SetRotationToTarget(Vector3 lookatTarget)
        {
            var direction = lookatTarget - _assignedNPC.position;
            direction.Normalize();
            var lookRotation = Quaternion.LookRotation(direction);
            _assignedNPC.DORotate(lookRotation.eulerAngles, 0.5f);
        }

        public void SetRotation(Quaternion rotationToSet)
        {
            _assignedNPC.DORotate(rotationToSet.eulerAngles, 0.5f);
        }

        private List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos)
        {
            _tileNode = DiscoData.Instance.MapData.GetPathFinderNode();

            var startNode = NodeFromWorldPoint(startPos);
            var targetNode = NodeFromWorldPoint(targetPos);

            var openSet = new List<PathFinderNode>();
            var closedSet = new HashSet<PathFinderNode>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                var currentNode = openSet[0];
                for (var i = 1; i < openSet.Count; i++)
                    if (openSet[i].FCost < currentNode.FCost ||
                        (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost))
                        currentNode = openSet[i];

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode) return RetracePath(startNode, targetNode);

                foreach (var neighbor in GetNeighbors(currentNode))
                {
                    if (!neighbor.IsWalkable || closedSet.Contains(neighbor)) continue;

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
            
            return NullPathReturn(targetPos);
        }

        private PathFinderNode NodeFromWorldPoint(Vector3 worldPosition)
        {
            if (worldPosition == -Vector3.one) return new PathFinderNode();

            var x = (int)worldPosition.x;
            var y = (int)worldPosition.z;
            return _tileNode[x, y];
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
            var path = new List<Vector3>();
            path.Add(target);
            foreach (var a in path)
            {
                Debug.Log(a);
            }
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

                if (checkX >= 0 && checkX < MapGeneratorSystem.Instance.MapData.CurrentMapSize.x && checkY >= 0 &&
                    checkY < MapGeneratorSystem.Instance.MapData.CurrentMapSize.y)
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