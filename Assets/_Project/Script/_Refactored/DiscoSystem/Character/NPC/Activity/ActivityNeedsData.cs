using System.Collections.Generic;
using Data;
using PropBehaviours;
using UnityEngine;

namespace DiscoSystem.Character.NPC.Activity
{
    public class ActivityNeedsData
    {
        public NPC Npc;
        public DiscoData DiscoData;

        public ActivityNeedsData(NPC npc, DiscoData discoData)
        {
            this.Npc = npc;
            this.DiscoData = discoData;
        }

        public T GetAvaliablePropByType<T>() where T : IPropUnit
        {
            if (DiscoData.Instance.PlacedItems.Count <= 0) return null;

            float lastDistance = 9999;
            T closestProp = null;
            foreach (var prop in DiscoData.GetPropList())
            {
                if (prop == null) continue;

                if (prop.transform.TryGetComponent(out IOccupieable occupieable))
                    if (occupieable.IsOccupied)
                        continue;

                if (prop is T propType)
                {
                    var distance = Vector3.Distance(Npc.transform.position, prop.WorldPos);
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

        public List<T> GetAvaliablePropsByType<T>() where T : IPropUnit
        {
            if (DiscoData.Instance.PlacedItems.Count <= 0) return null;

            var avaliableProps = new List<T>();
            foreach (var prop in DiscoData.GetPropList())
            {
                if (prop.transform.TryGetComponent(out IOccupieable occupieable))
                    if (occupieable.IsOccupied)
                        continue;

                if (prop is T propType) avaliableProps.Add(propType);
            }

            if (avaliableProps.Count < 1)
            {
                Debug.LogWarning(typeof(T) + " Turunde Prop Ogesi Bulunamadi!");
                return null;
            }

            return avaliableProps;
        }

        public List<T> GetAvaliablePropsByInterface<T>()
        {
            var bars = new List<T>();
            foreach (var prop in DiscoData.GetPropList())
                if (prop is T propType)
                    bars.Add(propType);

            if (bars.Count < 1)
            {
                Debug.LogWarning(typeof(T) + " Turunde Prop Ogesi Bulunamadi!");
                return null;
            }

            return bars;
        }
    }
}