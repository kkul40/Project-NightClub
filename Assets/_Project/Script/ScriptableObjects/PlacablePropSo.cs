using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New PlacablePropSo")]
    public class PlacablePropSo : ItemSo
    {
        [field : SerializeField]
        public GameObject Prefab { get; private set; }
        public Vector2Int ObjectSize = Vector2Int.one;
        public LayerMask placableLayerMask;
        public PlacementType placementType;

        public GameObject GetPrefab() => Prefab;
    }

    public enum PlacementType
    {
        FloorProp,
        WallProp,
        WallPaper,
        Tile,
    }
}