using Data;
using ExtensionMethods;
using UnityEngine;

namespace DiscoSystem.NewPathFinder
{
    public class PathGrid
    {
        public PathNode[,] grid;
        
        public PathGrid()
        {
            int MaxX = ConstantVariables.MaxMapSizeX * ConstantVariables.PathFinderGridSize + 1;
            int MaxY = ConstantVariables.MaxMapSizeY * ConstantVariables.PathFinderGridSize + 1;
            grid = new PathNode[MaxX, MaxY];

            for (var x = 0; x < MaxX; x++)
            {
                for (var y = 0; y < MaxY; y++)
                {
                    Vector3 worldPosition = new Vector3Int(x, 0, y).CellCenterPosition(eGridType.PathFinderGrid);
                    var node = new PathNode(x, y, worldPosition);
                    grid[x, y] = node;
                }
            }

            UpdatePath();
        }

        public void UpdatePath()
        {
            var pathSize = DiscoData.Instance.MapData.Path.PathFinderSize();

            for (int x = 0; x < pathSize.x; x++)
            {
                for (int y = 0; y < pathSize.y; y++)
                {
                    var source = DiscoData.Instance.MapData.Path.GetPath(x, y);
                    var target = grid[x, y];

                    target.IsWalkable = source.IsWalkable;
                    target.IsWall = source.IsWall;
                    target.OnlyEmployee = source.OnlyEmployee;
                    target.OnlyActivity = source.OnlyActivity;
                }
            }

        }

        public bool IsWalkable(Vector2Int coordinate) => grid[coordinate.x, coordinate.y].IsWalkable;

    }
}