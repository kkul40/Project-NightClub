using UnityEngine;

public static class DirectionHelper
{
    public static Quaternion RotateClockWise(Quaternion currentQuaternion)
    {
        var eulerAngle = currentQuaternion.eulerAngles;

        Quaternion quaternion = Quaternion.identity;

        if (eulerAngle == new Vector3(0, 0, 0))
        {
            quaternion.eulerAngles = new Vector3(0, 90, 0);
            return quaternion;
        }
        else if (eulerAngle == new Vector3(0, 90, 0))
        {
            quaternion.eulerAngles = new Vector3(0, 180, 0);
            return quaternion;
        }
        else if (eulerAngle == new Vector3(0, 180, 0))
        {
            quaternion.eulerAngles = new Vector3(0, 270, 0);
            return quaternion;
        }
        else
        {
            return quaternion;
        }
    }
    
    public static Quaternion RotateCounterClockWise(Quaternion currentQuaternion)
    {
        var eulerAngle = currentQuaternion.eulerAngles;

        Quaternion quaternion = Quaternion.identity;

        if (eulerAngle == new Vector3(0, 0, 0))
        {
            quaternion.eulerAngles = new Vector3(0, 270, 0);
            return quaternion;
        }
        else if (eulerAngle == new Vector3(0, 270, 0))
        {
            quaternion.eulerAngles = new Vector3(0, 180, 0);
            return quaternion;
        }
        else if (eulerAngle == new Vector3(0, 180, 0))
        {
            quaternion.eulerAngles = new Vector3(0, 90, 0);
            return quaternion;
        }
        else
        {
            return quaternion;
        }
    }
    
}