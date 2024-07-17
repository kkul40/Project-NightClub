using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class PathFinderNode
    {
        public Vector3 WorldPos;
        public PathFinderNode Parent;
        public bool IsWalkable;

        public int GridX;
        public int GridY;

        public int GCost;
        public int HCost;

        public int FCost => GCost + HCost;

        public PathFinderNode(bool isWalkable, Vector3 position, int gridX, int gridY)
        {
            IsWalkable = isWalkable;
            WorldPos = position;
            GridX = gridX;
            GridY = gridY;
        }
    }
}