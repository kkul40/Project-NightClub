using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Activity
{
    public abstract bool isEnded { get; protected set; }
    public abstract bool isCanceled { get; protected set; }
    
    public abstract void StartActivity(NPC npc);
    public abstract void UpdateActivity(NPC npc);
    public abstract void EndActivity(NPC npc);

    protected T GetAvaliablePropByType<T>(NPC npc) where T : Prop
    {
        if (GameData.Instance.placedProps.Count <= 0)
        {
            // Debug.LogWarning("Yerlestirilmis Prop Bulunamadi!");
            return null;
        }

        float lastDistance = 9999;
        T closestProp = null;
        foreach (var prop in GameData.Instance.placedProps)
        {
            if (prop.transform.TryGetComponent(out IOccupieable occupieable))
            {
                if(occupieable.IsOccupied) continue;
            }
            
            if (prop is T propType)
            {
                var distance = Vector3.Distance(npc.transform.position, prop.GetPropPosition());
                if (distance < lastDistance)
                {
                    closestProp = propType;
                    lastDistance = distance;
                }
            }
        }

        if (closestProp == null)
        {
            Debug.LogWarning( typeof(T)+ " Turunde Prop Ogesi Bulunamadi!");
            return null;
        }

        return closestProp;
    }
}