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
            return new FloorPropPlacer();
        }
    }

    [CreateAssetMenu(menuName = "Placable/New Dancable Area")]
    public class DancableTileSo : PlacablePropSo
    {
        public GameObject Prefab;
        
        public override IBuilder GetBuilder()
        {
            throw new System.NotImplementedException();
        }
    }
}