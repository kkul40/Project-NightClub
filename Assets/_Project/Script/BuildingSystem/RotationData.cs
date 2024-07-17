using Unity.Mathematics;
using UnityEngine;

namespace BuildingSystem
{
    public class RotationData
    {
        public Quaternion rotation;
        public Direction direction;

        public static readonly RotationData Down = new(new Vector3(0, 0, 0), Direction.Down);
        public static readonly RotationData Left = new(new Vector3(0, 90, 0), Direction.Left);
        public static readonly RotationData Up = new(new Vector3(0, 180, 0), Direction.Up);
        public static readonly RotationData Right = new(new Vector3(0, 270, 0), Direction.Right);
        public static RotationData Default => Down;


        public RotationData()
        {
            rotation = Quaternion.identity;
            direction = Direction.Down;
        }

        public RotationData(Vector3 eulerAngles, Direction direction)
        {
            rotation = Quaternion.identity;
            rotation.eulerAngles = eulerAngles;
            this.direction = direction;
        }

        public RotationData(Quaternion quaternion, Direction direction)
        {
            rotation = quaternion;
            this.direction = direction;
        }
    }
}