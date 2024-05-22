using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Placable/New FloorMaterial")]
    public class FloorMaterialDataSo : PlacableItemDataSo
    {
        public Material Material;
    }
}