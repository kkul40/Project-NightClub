using Disco_Building;
using UnityEngine;

namespace ExtensionMethods
{
    public static class RotationDataExtensionMethod
    {
        public static RotationData RotateClockWise(Quaternion currentQuaternion, Direction direction)
        {
            var d = direction;
            var quaternion = RotateClockWise(currentQuaternion, ref d);
            return new RotationData(quaternion, d);
        }

        public static RotationData RotateCounterClockWise(Quaternion currentQuaternion, Direction direction)
        {
            var d = direction;
            var quaternion = RotateCounterClockWise(currentQuaternion, ref d);
            return new RotationData(quaternion, d);
        }

        public static Quaternion RotateClockWise(Quaternion currentQuaternion, ref Direction direction)
        {
            var eulerAngle = currentQuaternion.eulerAngles;

            var quaternion = Quaternion.identity;

            if (eulerAngle == new Vector3(0, 0, 0))
            {
                quaternion.eulerAngles = new Vector3(0, 90, 0);
                direction = Direction.Left;
                return quaternion;
            }

            if (eulerAngle == new Vector3(0, 90, 0))
            {
                quaternion.eulerAngles = new Vector3(0, 180, 0);
                direction = Direction.Up;
                return quaternion;
            }

            if (eulerAngle == new Vector3(0, 180, 0))
            {
                quaternion.eulerAngles = new Vector3(0, 270, 0);
                direction = Direction.Right;
                return quaternion;
            }

            direction = Direction.Down;
            return quaternion;
        }

        public static Quaternion RotateCounterClockWise(Quaternion currentQuaternion, ref Direction direction)
        {
            var eulerAngle = currentQuaternion.eulerAngles;

            var quaternion = Quaternion.identity;

            if (eulerAngle == new Vector3(0, 0, 0))
            {
                quaternion.eulerAngles = new Vector3(0, 270, 0);
                direction = Direction.Right;
                return quaternion;
            }

            if (eulerAngle == new Vector3(0, 270, 0))
            {
                quaternion.eulerAngles = new Vector3(0, 180, 0);
                direction = Direction.Up;
                return quaternion;
            }

            if (eulerAngle == new Vector3(0, 180, 0))
            {
                quaternion.eulerAngles = new Vector3(0, 90, 0);
                direction = Direction.Left;
                return quaternion;
            }

            direction = Direction.Down;
            return quaternion;
        }

        public static Direction GetDirectionFromQuaternion(this Quaternion q)
        {
            var forwardVector = q * Vector3.forward;
            Vector3[] directionVectors = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
            Direction[] directions = { Direction.Down, Direction.Up, Direction.Right, Direction.Left };

            var closestDirectionIndex = 0;
            var closestAngle = Vector3.Angle(directionVectors[0], forwardVector);

            for (var i = 1; i < directionVectors.Length; i++)
            {
                var angle = Vector3.Angle(directionVectors[i], forwardVector);

                if (angle < closestAngle)
                {
                    closestAngle = angle;
                    closestDirectionIndex = i;
                }
            }

            return directions[closestDirectionIndex];
        }

        public static Quaternion Combine(this Quaternion quaternion, Quaternion quaternionToCombine)
        {
            return quaternion * quaternionToCombine;
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}