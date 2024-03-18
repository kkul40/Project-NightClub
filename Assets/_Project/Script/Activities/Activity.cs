using System;
using UnityEngine;

[Serializable]
public abstract class Activity
{
    public abstract bool isEnded { get; protected set; }
    public abstract bool isCanceled { get; protected set; }
    
    public abstract void StartActivity(NPC npc);
    public abstract void UpdateActivity(NPC npc);
    public abstract void EndActivity(NPC npc);

    protected Prop GetClosestPropByType(PropType propType, NPC npc)
    {
        if (GameData.Instance.placedProps.Count <= 0)
        {
            // Debug.LogWarning("Yerlestirilmis Prop Bulunamadi!");
            return null;
        }
        
        float lastDistance = 9999;
        Prop closestProp = null;
        foreach (var prop in GameData.Instance.placedProps)
        {
            if (prop.GetPropSo().PropType != propType) continue;

            var distance = Vector3.Distance(npc.transform.position, prop.GetPropPosition());
            if (distance < lastDistance)
            {
                closestProp = prop;
                lastDistance = distance;
            }
        }
        if (closestProp == null)
        {
            Debug.LogWarning(propType.ToString() + " Turunde Prop Ogesi Bulunamadi!");
            return null;
        }
        
        return closestProp;
    }
}