using UnityEngine;

public class TileIndicator : MonoBehaviour
{
    [SerializeField] private GameObject placingTileIndicator;
    [SerializeField] private GameObject removingTileIndicator;

    private bool isDirty = true;
    
    public void SetTileIndicator(PlacingType placingType)
    {
        CloseTileIndicator();
        switch (placingType)
        {
            case PlacingType.Place:
                placingTileIndicator.SetActive(true);
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

    public void CloseTileIndicator()
    {
        if (!isDirty) return;
        
        placingTileIndicator.SetActive(false);
        removingTileIndicator.SetActive(false);
        isDirty = false;
    }
}
