using UnityEngine;

namespace BuildingSystemFolder
{
    public interface IRotater
    {
        public RotationData RotationData { get; set; }
        public void TryRotating(GameObject gameObject);

        public void SaveData(RotationData rotationData)
        {
            RotationData = rotationData;
        }
    }
}