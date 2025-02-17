using System;
using DiscoSystem;
using ExtensionMethods;

namespace Disco_Building.Builders
{
    public class RotationMethodler360 : IRotationMethod
    {
        public void OnStart(BuildingNeedsData buildingNeedsData)
        {
        }

        public void OnRotate(BuildingNeedsData buildingNeedsData)
        {
            if (InputSystem.Instance.RotateLeft)
            {
                var tempQ = buildingNeedsData.RotationData.rotation;
                var rData = RotationDataExtensionMethod.RotateClockWise(tempQ, buildingNeedsData.RotationData.direction);

                buildingNeedsData.RotationData = rData;
            }

            if (InputSystem.Instance.RotateRight)
            {
                var tempQ = buildingNeedsData.RotationData.rotation;
                var rData = RotationDataExtensionMethod.RotateCounterClockWise(tempQ, buildingNeedsData.RotationData.direction);

                buildingNeedsData.RotationData = rData;
            }
        }
    }
}