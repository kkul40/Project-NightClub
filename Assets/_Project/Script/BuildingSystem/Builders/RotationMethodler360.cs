using System;

namespace BuildingSystem.Builders
{
    public class RotationMethodler360 : IRotationMethod
    {
        public void OnStart(BuildingNeedsData buildingNeedsData)
        {
        }

        public void OnRotate(BuildingNeedsData buildingNeedsData)
        {
            if (InputSystem.Instance.TurnLeft)
            {
                var tempQ = buildingNeedsData.RotationData.rotation;
                var rData = DirectionHelper.RotateClockWise(tempQ, buildingNeedsData.RotationData.direction);

                buildingNeedsData.RotationData = rData;
            }

            if (InputSystem.Instance.TurnRight)
            {
                var tempQ = buildingNeedsData.RotationData.rotation;
                var rData = DirectionHelper.RotateCounterClockWise(tempQ, buildingNeedsData.RotationData.direction);

                buildingNeedsData.RotationData = rData;
            }
        }
    }
}