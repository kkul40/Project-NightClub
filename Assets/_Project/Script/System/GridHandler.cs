using UnityEngine;

namespace BuildingSystemFolder
{
    public class GridHandler : MonoBehaviour
    {
        [SerializeField] private Grid grid;
        
        public Vector3Int GetMouseCellPosition(InputSystem inputSystem)
        {
            var mousePos = inputSystem.GetMouseMapPosition();
            Vector3Int cellPos = grid.WorldToCell(mousePos);
            return cellPos;
        }
        
        public Vector3 CellToWorldPosition(Vector3Int cellPos) => grid.CellToWorld(cellPos);
        public Vector3 GetCellCenterWorld(Vector3Int cellPos) => grid.GetCellCenterWorld(cellPos);
        public Vector3Int GetWorldToCell(Vector3 worldPos) => grid.WorldToCell(worldPos);
    }
}