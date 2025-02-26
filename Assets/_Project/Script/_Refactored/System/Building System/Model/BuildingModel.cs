using System;
using System.Collections.Generic;
using Data;
using Disco_ScriptableObject;
using RMC.Mini;
using RMC.Mini.Model;
using UnityEngine;

public class BuildingModel : BaseModel
{
    public List<StoreItemSO> StoreItems;

    //            instanceID   StoreID created-obj   Pos      Rot
    public Dictionary<int, Tuple<int, Transform, Vector3, Quaternion>> PlacedItems;
    
    public override void Initialize(IContext context)
    {
        base.Initialize(context);
        
        StoreItems = new List<StoreItemSO>();
        foreach (var item in DiscoData.Instance.AllInGameItems)
            StoreItems.Add(item.Value);

        PlacedItems = new Dictionary<int, Tuple<int, Transform, Vector3, Quaternion>>();
    }
}