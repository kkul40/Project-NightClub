using UnityEngine;

public static class DirectionHelper
{
    public static RotationData RotateClockWise(Quaternion currentQuaternion, Direction direction)
    {
        Direction d = direction;
        Quaternion quaternion = RotateClockWise(currentQuaternion, ref d);

        RotationData rotationData = new RotationData();
        rotationData.rotation = quaternion;
        rotationData.direction = d;
        
        return rotationData;
    }
    
    public static RotationData RotateCounterClockWise(Quaternion currentQuaternion, Direction direction)
    {
        Direction d = direction;
        Quaternion quaternion = RotateCounterClockWise(currentQuaternion, ref d);

        RotationData rotationData = new RotationData();
        rotationData.rotation = quaternion;
        rotationData.direction = d;
        
        return rotationData;
    }
    
    public static RotationData RotateToDirection(Direction direction)
    {
        RotationData rotationData = new RotationData();

        Quaternion quaternion = Quaternion.identity;

        switch (direction)
        {
            case Direction.Up:
                quaternion.eulerAngles = new Vector3(0, 180, 0);
                break;
            case Direction.Left:
                quaternion.eulerAngles = new Vector3(0, 90, 0);
                break;
            case Direction.Right:
                quaternion.eulerAngles = new Vector3(0, 270, 0);
                break;
            default:
                break;
        }

        rotationData.rotation = quaternion;
        rotationData.direction = direction;

        return rotationData;
    }

    
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
public class RotationData
{
    public Quaternion rotation;
    public Direction direction;

    public RotationData()
    {
        rotation = Quaternion.identity;
        direction = Direction.Down;
    }
    
}
public enum Direction
{
    Up,
    Down,
    Left,
    Right,
}