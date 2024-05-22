using ScriptableObjects;

namespace BuildingSystemFolder
{
    public class RotationMethodLeftAndDown : IRotationMethod
    {
        public RotationData GetRotation(RotationData rotationData)
        {
            if (InputSystem.Instance.E)
            {
                var rData = DirectionHelper.RotateToDirection(Direction.Down);

                rotationData = rData;

                return rotationData;
            }

            if (InputSystem.Instance.Q)
            {
                var rData = DirectionHelper.RotateToDirection(Direction.Left);

                rotationData = rData;

                return rotationData;
            }

            return rotationData;
        }
    }
}