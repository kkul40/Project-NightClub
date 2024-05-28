using UnityEngine;

namespace BuildingSystemFolder
{
    public class RotationData
    {
        public Quaternion rotation;
        public Direction direction;

        public RotationData()
        {
            rotation = Quaternion.identity;
            direction = Direction.Down;
        }
        public RotationData(Quaternion quaternion, Direction direction)
        {
            this.rotation = quaternion;
            this.direction = direction;
        }
    }
}