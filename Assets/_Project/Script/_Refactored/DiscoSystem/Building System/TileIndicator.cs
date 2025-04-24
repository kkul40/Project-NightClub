using UnityEngine;

namespace DiscoSystem.Building_System
{
    public class TileIndicator : MonoBehaviour
    {
        // TODO : refactor this script to make it so it can show 2 or more indicator at once
        [SerializeField] private GameObject parentIndicator;
        [SerializeField] private GameObject placingTileIndicator;
        [SerializeField] private GameObject removingTileIndicator;
        [SerializeField] private GameObject directionIndicator;
        public ePlacingType placingType { get; private set; }

        private void Start()
        {
            CloseTileIndicator();
        }

        public void SetTileIndicator(ePlacingType placingType, Vector2 size)
        {
            CloseTileIndicator();

            switch (placingType)
            {
                case ePlacingType.Place:
                    placingTileIndicator.SetActive(true);
                    this.placingType = ePlacingType.Place;
                    break;
                case ePlacingType.Direction:
                    directionIndicator.SetActive(true);
                    this.placingType = ePlacingType.Direction;
                    break;
                case ePlacingType.Remove:
                    removingTileIndicator.SetActive(true);
                    this.placingType = ePlacingType.Remove;
                    break;
            }
            
            SetSize(size);
        }

        public void SetPositionAndRotation(Vector3 newPos, Quaternion rotation)
        {
            parentIndicator.transform.position = newPos;
            parentIndicator.transform.rotation = rotation;

            // transform.position = newPos.CellCenterPosition(eGridType.PlacementGrid);
        }

        public void SetSize(Vector2 size)
        {
            Vector3 pos = new Vector3(size.x / 2, 0, size.y / 2) -  new Vector3(0.5f, 0f, 0.5f);
            Vector3 sik = new Vector3(size.x, 1, size.y);

            // placingTileIndicator.transform.localPosition = pos;
            placingTileIndicator.transform.localScale = sik;
        }

        public void GetCurrentCenterPosition()
        {
            // TODO Return placing Tile Center Position
        }

        public void CloseTileIndicator()
        {
            placingType = ePlacingType.None;
            placingTileIndicator.SetActive(false);
            removingTileIndicator.SetActive(false);
            directionIndicator.SetActive(false);
        }
    }

    public enum ePlacingType
    {
        None,
        Place,
        Direction,
        Remove
    }
}