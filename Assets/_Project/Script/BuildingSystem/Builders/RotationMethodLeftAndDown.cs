using System;

namespace BuildingSystem.Builders
{
    public class RotationMethodLeftAndDown : IRotationMethod
    {
        public void Rotate(BuildingNeedsData buildingNeedsData)
        {
            if (buildingNeedsData.RotationData.direction == Direction.Up ||
                buildingNeedsData.RotationData.direction == Direction.Right)
            {
                var rData = DirectionHelper.RotateToDirection(Direction.Down);

                buildingNeedsData.RotationData = rData;
            }
            
            if (InputSystem.Instance.E)
            {
                var rData = DirectionHelper.RotateToDirection(Direction.Down);

                buildingNeedsData.RotationData = rData;
            }

            if (InputSystem.Instance.Q)
            {
                var rData = DirectionHelper.RotateToDirection(Direction.Left);

                buildingNeedsData.RotationData = rData;
            }
        }
    }
}