using BuildingSystemFolder;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Placable/New FloorProp")]
    public class FloorPropSo : PlacablePropSo
    {
        public GameObject Prefab;
        public Vector2Int ObjectSize = Vector2Int.one;
        
        public override IBuilder GetBuilder()
        {
            return new FloorPropPlacer(new InputRotationHandlerLeftAndDown());
        }
    }

    public class BarPropSo : FloorPropSo
    {
        public override IBuilder GetBuilder()
        {
            return new FloorPropPlacer(new InputRotationHandlerLeftAndDown());
        }
    }
}