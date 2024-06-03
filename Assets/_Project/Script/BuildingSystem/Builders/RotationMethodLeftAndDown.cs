using System;

namespace BuildingSystem.Builders
{
    public class RotationMethodLeftAndDown : IRotationMethod
    {
        public void OnStart(BuildingNeedsData buildingNeedsData)
        {
            var d = buildingNeedsData.RotationData.direction;
            if (d == Direction.Right || d == Direction.Up)
            {
                buildingNeedsData.RotationData = RotationData.Down;
            }
        }

        public void OnRotate(BuildingNeedsData buildingNeedsData)
        {
            if (InputSystem.Instance.E)
            {
                buildingNeedsData.RotationData = RotationData.Down;
            }

            if (InputSystem.Instance.Q)
            {
                buildingNeedsData.RotationData = RotationData.Left;
            }
        }
    }
}