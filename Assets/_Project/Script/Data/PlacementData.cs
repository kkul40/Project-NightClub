using System;
using ScriptableObjects;
using UnityEngine;

namespace Data
{
    [Serializable]
    public struct PlacementData
    {
        public int ID;
        public PlacablePropSo ItemSo;
        public GameObject SceneObject;
        public Vector2Int ObjectSize;
        public Direction Direction;

        public PlacementData(PlacablePropSo itemSo, GameObject sceneObject,Vector2Int objectSize, Direction direction)
        {
            ID = Guid.NewGuid().GetHashCode();
            ItemSo = itemSo;
            SceneObject = sceneObject;
            ObjectSize = objectSize;
            Direction = direction;
        }
    }
}