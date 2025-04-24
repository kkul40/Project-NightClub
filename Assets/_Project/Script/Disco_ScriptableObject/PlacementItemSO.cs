using Data;
using DiscoSystem.Building_System.Controller;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Disco_ScriptableObject
{
    [CreateAssetMenu(fileName = "New Placement Item", menuName = "Item/Placement Item")]
    public class PlacementItemSO : StoreItemSO
    {
        public GameObject Prefab;
        public GridSizes GridSizes;
        [HorizontalGroup("Split", 290)] [EnumToggleButtons] [HideLabel]
        public ePlacementLayer PlacementLayer;

        [Header("Path Settings")] 
        public bool OnlyForActivity;

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