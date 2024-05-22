using UnityEngine;

namespace System
{
    public class GridHandler : Singleton<GridHandler>
    {
        [SerializeField] private Grid grid;

        public Vector3Int GetMouseCellPosition(Vector3 mousePosition)
        {
            var cellPos = grid.WorldToCell(mousePosition);
            return cellPos;
        }

        public Vector3 CellToWorldPosition(Vector3Int cellPos)
        {
            return grid.CellToWorld(cellPos);
        }

        public Vector3 GetCellCenterWorld(Vector3Int cellPos)
        {
            return grid.GetCellCenterWorld(cellPos);
        }

        public Vector3Int GetWorldToCell(Vector3 worldPos)
        {
            return grid.WorldToCell(worldPos);
        }
    }
}