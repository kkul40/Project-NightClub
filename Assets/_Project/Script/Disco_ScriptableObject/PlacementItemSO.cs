using Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Disco_ScriptableObject
{
    [CreateAssetMenu(fileName = "New Placement Item", menuName = "Item/Placement Item")]
    public class PlacementItemSO : StoreItemSO
    {
        public GameObject Prefab;
        public Vector2Int Size;
        public bool IsBig;

        [HorizontalGroup("Split", 290)] [EnumToggleButtons] [HideLabel]
        public ePlacementLayer PlacementLayer;

        public enum eRotationType
        {
            None,
            ThreeSixty,
            LeftRight,
            Auto
        }

        [HorizontalGroup("Split", 290)] [EnumToggleButtons] [HideLabel]
        public eRotationType eRotation;
    }
}