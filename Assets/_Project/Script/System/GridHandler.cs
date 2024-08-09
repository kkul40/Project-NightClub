using UnityEngine;
using UnityEngine.Serialization;

namespace System
{
    public class GridHandler : Singleton<GridHandler>
    {
        [SerializeField] private Grid grid1x1;
        [SerializeField] private Grid grid4x4;
        [SerializeField] private Shader gridSahder;
        [SerializeField] private Color gridColor;
        [SerializeField] private GameObject gridPlane;
        [SerializeField] private GameObject floorGridPlane;
        [SerializeField] private GameObject leftWallGridPlane;
        [SerializeField] private GameObject rightWallGridPlane;

        private void OnEnable()
        {
            MapGeneratorSystem.OnMapSizeChanged += AlignGridWithMapSize;
        }

        private void OnDisable()
        {
            MapGeneratorSystem.OnMapSizeChanged -= AlignGridWithMapSize;
        }

        public Vector3Int GetMouseCellPosition(Vector3 mousePosition)
        {
            var cellPos = grid1x1.WorldToCell(mousePosition);
            return cellPos;
        }

        public Vector3 CellToWorldPosition(Vector3Int cellPos)
        {
            return grid1x1.CellToWorld(cellPos);
        }

        public Vector3 GetCellCenterWorld(Vector3Int cellPos)
        {
            return grid1x1.GetCellCenterWorld(cellPos);
        }

        public Vector3Int GetWorldToCell(Vector3 worldPos)
        {
            return grid1x1.WorldToCell(worldPos);
        }

        public void ToggleGrid(bool toggle)
        {
            gridPlane.SetActive(toggle);
        }

        private void AlignGridWithMapSize(Vector2Int mapSize)
        {
            // Floor Shader
            var minimumOffset = 0.001f;
            var shaderMaterial = new Material(gridSahder);
            shaderMaterial.SetColor("_Color", gridColor);
            shaderMaterial.SetVector("_CellSize", new Vector4(mapSize.x, mapSize.y, 0, 0));
            floorGridPlane.GetComponent<MeshRenderer>().material = shaderMaterial;
            floorGridPlane.transform.position = new Vector3((float)mapSize.x / 2, minimumOffset, (float)mapSize.y / 2);
            floorGridPlane.transform.localScale = new Vector3((float)mapSize.x / 10, 1, (float)mapSize.y / 10);

            // Left Wall Shader
            var shaderLeftWallMaterial = new Material(gridSahder);
            shaderLeftWallMaterial.SetColor("_Color", gridColor);
            shaderLeftWallMaterial.SetVector("_CellSize", new Vector4(mapSize.x, 3, 0, 0));
            leftWallGridPlane.GetComponent<MeshRenderer>().material = shaderLeftWallMaterial;
            leftWallGridPlane.transform.position = new Vector3((float)mapSize.x / 2, 1.5f, minimumOffset);
            leftWallGridPlane.transform.localScale = new Vector3((float)mapSize.x / 10, 1, (float)3 / 10);

            // Right Wall Shader
            var shaderRightWallMaterial = new Material(gridSahder);
            shaderRightWallMaterial.SetColor("_Color", gridColor);
            shaderRightWallMaterial.SetVector("_CellSize", new Vector4(mapSize.y, 3, 0, 0));
            rightWallGridPlane.GetComponent<MeshRenderer>().material = shaderRightWallMaterial;
            rightWallGridPlane.transform.position = new Vector3(minimumOffset, 1.5f, (float)mapSize.y / 2);
            rightWallGridPlane.transform.localScale = new Vector3((float)mapSize.y / 10, 1, (float)3 / 10);
        }
    }
}