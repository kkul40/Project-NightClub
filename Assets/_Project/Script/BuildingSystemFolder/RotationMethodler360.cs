using ScriptableObjects;

namespace BuildingSystemFolder
{
    public class RotationMethodler360 : IRotationMethod
    {
        public RotationData GetRotation(RotationData rotationData)
        {
            if (InputSystem.Instance.E)
            {
                var tempQ = rotationData.rotation;
                var rData = DirectionHelper.RotateClockWise(tempQ, rotationData.direction);

                rotationData = rData;

                return rotationData;
            }

            if (InputSystem.Instance.Q)
            {
                var tempQ = rotationData.rotation;
                var rData = DirectionHelper.RotateCounterClockWise(tempQ, rotationData.direction);

                rotationData = rData;

                return rotationData;
            }

            return rotationData;
        }
    }
}