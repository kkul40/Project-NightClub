using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Placable/New FloorProp")]
    public class FloorItemDataSo : PlacableItemDataSo
    {
    }

    public class BarItemDataSo : FloorItemDataSo
    {
    }
}