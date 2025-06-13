using UnityEngine;

namespace DiscoSystem.NewPathFinder
{
    public class PathNode
    {
        public Vector3 WorldPosition { get; }
        public int X { get; }
        public int Y { get; }

        public bool IsWalkable;
        public bool IsWall;
        public bool OnlyEmployee; // Only Employee Can Walk
        public bool OnlyActivity;

        public PathNode(int x, int y, Vector3 worldPosition)
        {
            X = x;
            Y = y;
            WorldPosition = worldPosition;
        }

        public bool Compare(PathNode node)
        {
            if (node.X == X && node.Y == Y) return true;
            return false;
        }

        public bool Compare(Vector2Int index)
        {
            if (index.x == X && index.y == Y) return true;
            return false;
        }

        public Vector2Int Coordinates => new Vector2Int(X, Y);
    }
}