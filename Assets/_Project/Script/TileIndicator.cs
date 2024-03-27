using BuildingSystemFolder;
using UnityEngine;

public class TileIndicator : MonoBehaviour
{
    [SerializeField] private GameObject placingTileIndicator;
    [SerializeField] private GameObject removingTileIndicator;
    [SerializeField] private GameObject directionIndicator;

    private bool isDirty = true;

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
                break;
            case PlacingType.Direction:
                directionIndicator.SetActive(true);
                break;
            case PlacingType.Remove:
                removingTileIndicator.SetActive(true);
                break;
        }
        isDirty = true;
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
        if (!isDirty) return;
        
        placingTileIndicator.SetActive(false);
        removingTileIndicator.SetActive(false);
        directionIndicator.SetActive(false);
        isDirty = false;
    }
}