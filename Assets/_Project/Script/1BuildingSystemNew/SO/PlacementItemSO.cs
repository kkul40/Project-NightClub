using Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace _1BuildingSystemNew
{
    [CreateAssetMenu(menuName = "New Item/New Placement Item")]
    public class PlacementItemSO : StoreItemSO
    {
        public GameObject Prefab;
        public Vector2Int Size;
        
        public ePlacementLayer PlacementLayer;
        public enum eRotationType {None, ThreeSixty, LeftRight}
        public eRotationType eRotation;
    }
}