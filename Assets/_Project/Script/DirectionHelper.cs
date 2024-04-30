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
    
    public static Direction GetDirectionFromQuaternion(Quaternion q)
    {
        Vector3 forwardVector = q * Vector3.forward;
        Vector3[] directionVectors = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        Direction[] directions = { Direction.Down, Direction.Up, Direction.Right, Direction.Left };
    
        int closestDirectionIndex = 0;
        float closestAngle = Vector3.Angle(directionVectors[0], forwardVector);
    
        for (int i = 1; i < directionVectors.Length; i++)
        {
            float angle = Vector3.Angle(directionVectors[i], forwardVector);
        
            if (angle < closestAngle)
            {
                closestAngle = angle;
                closestDirectionIndex = i;
            }
        }
        return directions[closestDirectionIndex];
    }
}
public enum Direction
{
    Up,
    Down,
    Left,
    Right,
}