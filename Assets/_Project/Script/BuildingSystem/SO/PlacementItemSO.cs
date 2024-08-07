using Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BuildingSystem.SO
{
    [CreateAssetMenu(menuName = "New Item/New Placement Item")]
    public class PlacementItemSO : StoreItemSO
    {
        public GameObject Prefab;
        public Vector2Int Size;

        [HorizontalGroup("Split", 290), EnumToggleButtons, HideLabel]
        public ePlacementLayer PlacementLayer;

        public enum eRotationType
        {
            None,
            ThreeSixty,
            LeftRight,
            Auto
        }

        [HorizontalGroup("Split", 290), EnumToggleButtons, HideLabel]
        public eRotationType eRotation;
    }
}