using UnityEngine;

namespace BuildingSystem.Builders
{
    public class NullRotationMethod : IRotationMethod
    {
        public void OnStart(BuildingNeedsData buildingNeedsData)
        {
            buildingNeedsData.RotationData = RotationData.Default;
        }

        public void OnRotate(BuildingNeedsData buildingNeedsData)
        {
        }
    }
}