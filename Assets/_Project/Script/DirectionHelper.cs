using UnityEngine;

public static class DirectionHelper
{
    public static Quaternion RotateClockWise(Quaternion currentQuaternion, ref Direction direction)
    {
        var eulerAngle = currentQuaternion.eulerAngles;

        Quaternion quaternion = Quaternion.identity;

        if (eulerAngle == new Vector3(0, 0, 0))
        {
            quaternion.eulerAngles = new Vector3(0, 90, 0);
            direction = Direction.Left;
            return quaternion;
        }
        else if (eulerAngle == new Vector3(0, 90, 0))
        {
            quaternion.eulerAngles = new Vector3(0, 180, 0);
            direction = Direction.Up;
            return quaternion;
        }
        else if (eulerAngle == new Vector3(0, 180, 0))
        {
            quaternion.eulerAngles = new Vector3(0, 270, 0);
            direction = Direction.Right;
            return quaternion;
        }
        else
        {
            direction = Direction.Down;
            return quaternion;
        }
    }
    
    public static Quaternion RotateCounterClockWise(Quaternion currentQuaternion, ref Direction direction)
    {
        var eulerAngle = currentQuaternion.eulerAngles;

        Quaternion quaternion = Quaternion.identity;

        if (eulerAngle == new Vector3(0, 0, 0))
        {
            quaternion.eulerAngles = new Vector3(0, 270, 0);
            direction = Direction.Right;
            return quaternion;
        }
        else if (eulerAngle == new Vector3(0, 270, 0))
        {
            quaternion.eulerAngles = new Vector3(0, 180, 0);
            direction = Direction.Up;
            return quaternion;
        }
        else if (eulerAngle == new Vector3(0, 180, 0))
        {
            quaternion.eulerAngles = new Vector3(0, 90, 0);
            direction = Direction.Left;
            return quaternion;
        }
        else
        {
            direction = Direction.Down;
            return quaternion;
        }
    }
    
}