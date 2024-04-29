using BuildingSystemFolder;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Placable/New WallProp")]
    public class WallPropSo : PlacablePropSo
    {
        public GameObject Prefab;
        public Vector2Int ObjectSize = Vector2Int.one;
        
        public override IBuilder GetBuilder()
        {
            return new WallPropPlacer();
        }
    }
}