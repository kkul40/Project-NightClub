using ScriptableObjects;
using UnityEngine;

namespace BuildingSystemFolder
{
    public class NoneRotationMethod : IRotationMethod
    {
        public RotationData GetRotation(RotationData rotationData)
        {
            Debug.LogError("None Rotation Handler");
            return rotationData;
        }
    }
}