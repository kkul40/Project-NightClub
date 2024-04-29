using BuildingSystemFolder;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Placable/New WallMaterial")]
    public class WallMaterialSo : PlacablePropSo
    {
        public Material Material;
        
        public override IBuilder GetBuilder()
        {
            return new WallMaterialPlacer();
        }
    }
}

