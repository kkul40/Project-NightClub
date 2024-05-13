using System;
using BuildingSystemFolder;
using UnityEngine;

public class TileIndicator : MonoBehaviour
{
    public PlacingType placingType { get; private set; }
    [SerializeField] private GameObject placingTileIndicator;
    [SerializeField] private GameObject removingTileIndicator;
    [SerializeField] private GameObject directionIndicator;

    private void Start()
    {
        CloseTileIndicator();
    }

    public void SetTileIndicator(PlacingType placingType)
    {
        CloseTileIndicator();
        
        switch (placingType)
        {
            case PlacingType.Place:
                placingTileIndicator.SetActive(true);
                this.placingType = PlacingType.Place;
                break;
            case PlacingType.Direction:
                directionIndicator.SetActive(true);
                this.placingType = PlacingType.Direction;
                break;
            case PlacingType.Remove:
                removingTileIndicator.SetActive(true);
                this.placingType = PlacingType.Remove;
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
        placingType = PlacingType.None;
        placingTileIndicator.SetActive(false);
        removingTileIndicator.SetActive(false);
        directionIndicator.SetActive(false);
    }
}