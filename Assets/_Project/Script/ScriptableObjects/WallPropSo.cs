using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Placable/New WallProp")]
    public class WallPropDataSo : PlacableItemDataSo
    {
        public GameObject Prefab;
        public Vector2Int ObjectSize = Vector2Int.one;
    }
}