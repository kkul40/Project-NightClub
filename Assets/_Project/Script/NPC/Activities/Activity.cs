using System.Collections.Generic;
using Data;
using PropBehaviours;
using UnityEngine;

namespace NPC.Activities
{
    public abstract class Activity
    {
        public abstract bool isEnded { get; protected set; }
        public abstract bool isCanceled { get; protected set; }

        public abstract void StartActivity(NPC npc);
        public abstract void UpdateActivity(NPC npc);
        public abstract void EndActivity(NPC npc);

        protected T GetAvaliablePropByType<T>(NPC npc, ePlacementLayer layer) where T : Prop
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
                    var distance = Vector3.Distance(npc.transform.position, prop.GetPropCellPosition());
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

        protected List<T> GetMultiplePropsByType<T>(ePlacementLayer layer) where T : Prop
        {
            if (GameData.Instance.placementDataHandler.GetPlacementData(layer).Count <= 0) return new List<T>();

            var propList = new List<T>();

            float lastDistance = 9999;
            foreach (var prop in GameData.Instance.GetPropList)
            {
                if (prop == null) continue;

                if (prop.transform.TryGetComponent(out IOccupieable occupieable))
                    if (occupieable.IsOccupied)
                        continue;

                if (prop is T propType) propList.Add(propType);
            }

            if (propList.Count < 1)
            {
                Debug.LogWarning(typeof(T) + " Turunde Prop Ogesi Bulunamadi!");
                return new List<T>();
            }

            return propList;
        }
    }
}