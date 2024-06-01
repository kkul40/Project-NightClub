using System;
using Data;
using PropBehaviours;
using NPC;
using UnityEngine;

namespace New_NPC
{
    public class ActivityNeedsData
    {
        public NPC.NPC Npc;
        public GameData GameData;
        public GridHandler GridHandler;
        
        public T GetAvaliablePropByType<T>(ePlacementLayer layer) where T : Prop
        {
            if (GameData.Instance.placementDataHandler.GetPlacementData(layer).Count <= 0) return null;

            float lastDistance = 9999;
            T closestProp = null;
            foreach (var prop in GameData.Instance.GetPropList)
            {
                if (prop == null) continue;


                if (prop.transform.TryGetComponent(out IOccupieable occupieable))
                    if (occupieable.IsOccupied)
                        continue;

                if (prop is T propType)
                {
                    var distance = Vector3.Distance(Npc.transform.position, prop.GetPropCellPosition());
                    if (distance < lastDistance)
                    {
                        closestProp = propType;
                        lastDistance = distance;
                    }
                }
            }

            if (closestProp == null)
            {
                Debug.LogWarning(typeof(T) + " Turunde Prop Ogesi Bulunamadi!");
                return null;
            }

            return closestProp;
        }
    }
}