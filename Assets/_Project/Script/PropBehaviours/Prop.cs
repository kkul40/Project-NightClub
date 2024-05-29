using System;
using BuildingSystem;
using UnityEngine;

namespace PropBehaviours
{
    [Serializable]
    public class Prop : MonoBehaviour
    {
        public Vector3Int CellPosition;
        public Direction direction;

        public virtual void Initialize(Vector3Int cellPosition, Direction direction)
        {
            CellPosition = cellPosition;
            this.direction = direction;
        }

        public Vector3Int GetPropCellPosition()
        {
            return CellPosition;
        }

        public Quaternion GetPropRotation()
        {
            return transform.rotation;
        }
    }

    public struct PropData
    {
        public int ID { get; }
        public Vector3Int cellPosition { get; }
        public RotationData rotationData { get; }

        public PropData(int ID, Vector3Int cellPosition, RotationData rotationData)
        {
            this.ID = ID;
            this.cellPosition = cellPosition;
            this.rotationData = rotationData;
        }
    }
}