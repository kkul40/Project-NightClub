using Data;
using UnityEngine;

namespace BuildingSystem.SO
{
    [CreateAssetMenu(menuName = "New Item/New Placement Item")]
    public class PlacementItemSO : StoreItemSO
    {
        public GameObject Prefab;
        public Vector2Int Size;

        public ePlacementLayer PlacementLayer;

        public enum eRotationType
        {
            None,
            ThreeSixty,
            LeftRight,
            Auto
        }

        public eRotationType eRotation;
    }
}