using System.Building_System.GameEvents;
using UnityEngine;

namespace System
{
    public class GridSystem : MonoBehaviour
    {
        [SerializeField] private Shader gridSahder;
        [SerializeField] private Color gridColor;
        [SerializeField] private GameObject GridTransform;
        [SerializeField] private GameObject floorGridPlane;
        [SerializeField] private GameObject leftWallGridPlane;
        [SerializeField] private GameObject rightWallGridPlane;

        public void Initialize()
        {
            ToggleGrid(false);
            
            GameEvent.Subscribe<Event_MapSizeChanged>(handle => AlignGridWithMapSize(handle.Size));
            GameEvent.Subscribe<Event_ToggleBuildingMode>(handle => ToggleGrid(handle.Toggle));
        }


        private void ToggleGrid(bool toggle)
        {
            GridTransform.SetActive(toggle);
        }

        private void AlignGridWithMapSize(Vector2Int mapSize)
        {
            // Floor Shader
            var minimumOffset = 0.015f;
            var shaderMaterial = new Material(gridSahder);
            shaderMaterial.SetColor("_Color", gridColor);
            shaderMaterial.SetVector("_CellSize", new Vector4(mapSize.x, mapSize.y, 0, 0));
            floorGridPlane.GetComponent<MeshRenderer>().material = shaderMaterial;
            floorGridPlane.transform.position = new Vector3((float)mapSize.x / 2 + minimumOffset, minimumOffset, (float)mapSize.y / 2 + minimumOffset);
            floorGridPlane.transform.localScale = new Vector3((float)mapSize.x / 10, 1, (float)mapSize.y / 10);

            // Left Wall Shader
            var shaderLeftWallMaterial = new Material(gridSahder);
            shaderLeftWallMaterial.SetColor("_Color", gridColor);
            shaderLeftWallMaterial.SetVector("_CellSize", new Vector4(mapSize.x, 3, 0, 0));
            leftWallGridPlane.GetComponent<MeshRenderer>().material = shaderLeftWallMaterial;
            leftWallGridPlane.transform.position = new Vector3((float)mapSize.x / 2 + minimumOffset, 1.5f, minimumOffset);
            leftWallGridPlane.transform.localScale = new Vector3((float)mapSize.x / 10, 1, (float)3 / 10);

            // Right Wall Shader
            var shaderRightWallMaterial = new Material(gridSahder);
            shaderRightWallMaterial.SetColor("_Color", gridColor);
            shaderRightWallMaterial.SetVector("_CellSize", new Vector4(mapSize.y, 3, 0, 0));
            rightWallGridPlane.GetComponent<MeshRenderer>().material = shaderRightWallMaterial;
            rightWallGridPlane.transform.position = new Vector3(minimumOffset, 1.5f, (float)mapSize.y / 2 + minimumOffset);
            rightWallGridPlane.transform.localScale = new Vector3((float)mapSize.y / 10, 1, (float)3 / 10);
        }
    }

    public enum eGridType
    {
        PlacementGrid,
        PathFinderGrid
    }
}