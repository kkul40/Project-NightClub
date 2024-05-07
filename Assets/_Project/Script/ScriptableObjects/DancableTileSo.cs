using BuildingSystemFolder;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Placable/New Dancable Area")]
    public class DancableTileSo : PlacablePropSo
    {
        public GameObject Prefab;
        
        public override IBuilder GetBuilder()
        {
            return new DanceFloorPlacer();
        }
    }
}