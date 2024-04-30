using System;
using ScriptableObjects;
using UnityEngine;

[Serializable]
public class PlacementData
{
    public PlacablePropSo ItemSo;
    public GameObject SceneObject;
    public Direction Direction;

    public PlacementData(PlacablePropSo itemSo, GameObject sceneObject, Direction direction)
    {
        ItemSo = itemSo;
        SceneObject = sceneObject;
        Direction = direction;
    }
}