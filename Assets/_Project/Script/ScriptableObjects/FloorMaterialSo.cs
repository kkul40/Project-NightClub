using BuildingSystemFolder;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Placable/New FloorMaterial")]
    public class FloorMaterialSo : PlacablePropSo
    {
        public Material Material;
        
        public override IBuilder GetBuilder()
        {
            return new FloorMaterialPlacer();
        }
    }
}