using System;
using UnityEngine;

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
        transform.position = newPos;
    }

    public void RoateDirectionIndicator(Quaternion quaternion)
    {
        directionIndicator.transform.rotation = quaternion;
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
    Remove,
}