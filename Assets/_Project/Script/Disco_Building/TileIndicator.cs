using System;
using ExtensionMethods;
using UnityEngine;

namespace Disco_Building
{
    public class TileIndicator : MonoBehaviour
    {
        [SerializeField] private GameObject placingTileIndicator;
        [SerializeField] private GameObject removingTileIndicator;
        [SerializeField] private GameObject directionIndicator;
        public ePlacingType placingType { get; private set; }

        private void Start()
        {
            CloseTileIndicator();
        }

        public void SetTileIndicator(ePlacingType placingType)
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
        }

        public void SetPosition(Vector3 newPos)
        {
            transform.position = newPos.CellCenterPosition(eGridType.PlacementGrid);
        }

        public void SetSize(Vector2 size)
        {
            placingTileIndicator.transform.localPosition = new Vector3(size.x / 2, 0, size.y / 2) -  new Vector3(0.5f, 0f, 0.5f);
            placingTileIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
        }

        public void GetCurrentCenterPosition()
        {
            // TODO Return placing Tile Center Position
        }

        public void RoateDirectionIndicator(Quaternion quaternion)
        {
            transform.rotation = quaternion;
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