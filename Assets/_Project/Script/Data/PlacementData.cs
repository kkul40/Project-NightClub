using System;
using ScriptableObjects;
using UnityEngine;

[Serializable]
public class PlacementData
{
    public PlacablePropSo ItemSo;
    public GameObject SceneObject;

    public PlacementData(PlacablePropSo itemSo, GameObject sceneObject)
    {
        ItemSo = itemSo;
        SceneObject = sceneObject;
    }
}