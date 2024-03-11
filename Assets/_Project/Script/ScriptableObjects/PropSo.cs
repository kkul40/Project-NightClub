using UnityEngine;

[CreateAssetMenu(fileName = "New Prop")]
public class PropSo : ItemSo
{
    public GameObject Prefab;
    public PlacableType placableType;

    public LayerMask placableLayer;

    private void OnValidate()
    {
        // Convert integer of an enum to layerMask
        placableLayer = 1 << (int)placableType;
    }
    
    public LayerMask GetPlacableLayer => placableLayer;
}