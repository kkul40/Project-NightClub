using System;

namespace _1BuildingSystemNew.Builders
{
    public class RotationMethodLeftAndDown : IRotationMethod
    {
        public void Rotate(BuildingNeedsData buildingNeedsData)
        {
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