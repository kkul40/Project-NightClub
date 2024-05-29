using System;

namespace _1BuildingSystemNew.Builders
{
    public class RotationMethodler360 : IRotationMethod
    {
        public void Rotate(BuildingNeedsData buildingNeedsData)
        {
            if (InputSystem.Instance.E)
            {
                var tempQ = buildingNeedsData.RotationData.rotation;
                var rData = DirectionHelper.RotateClockWise(tempQ, buildingNeedsData.RotationData.direction);

                buildingNeedsData.RotationData = rData;
            }

            if (InputSystem.Instance.Q)
            {
                var tempQ = buildingNeedsData.RotationData.rotation;
                var rData = DirectionHelper.RotateCounterClockWise(tempQ, buildingNeedsData.RotationData.direction);

                buildingNeedsData.RotationData = rData;
            }
        }
    }
}