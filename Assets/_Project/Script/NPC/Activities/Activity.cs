// using System.Collections.Generic;
// using Data;
// using PropBehaviours;
// using UnityEngine;
//
// namespace Activities
// {
//     public abstract class Activity
//     {
//         public abstract bool isEnded { get; protected set; }
//         public abstract bool isCanceled { get; protected set; }
//
//         public abstract void StartActivity(New_NPC.NPC npc);
//         public abstract void UpdateActivity(New_NPC.NPC npc);
//         public abstract void EndActivity(New_NPC.NPC npc);
//
//         protected T GetAvaliablePropByType<T>(New_NPC.NPC npc, ePlacementLayer layer) where T : IPropUnit
//         {
//             if (DiscoData.Instance.placementDataHandler.GetPlacementDatas(layer).Count <= 0) return null;
//
//             float lastDistance = 9999;
//             T closestProp = null;
//             foreach (var prop in DiscoData.Instance.GetPropList)
//             {
//                 if (prop == null) continue;
//
//
//                 if (prop.transform.TryGetComponent(out IOccupieable occupieable))
//                     if (occupieable.IsOccupied)
//                         continue;
//
//                 if (prop is T propType)
//                 {
//                     var distance = Vector3.Distance(npc.transform.position, prop.CellPosition);
//                     if (distance < lastDistance)
//                     {
//                         closestProp = propType;
//                         lastDistance = distance;
//                     }
//                 }
//             }
//
//             if (closestProp == null)
//             {
//                 Debug.LogWarning(typeof(T) + " Turunde Prop Ogesi Bulunamadi!");
//                 return null;
//             }
//
//             return closestProp;
//         }
//
//         protected List<T> GetMultiplePropsByType<T>(ePlacementLayer layer) where T : IPropUnit
//         {
//             if (DiscoData.Instance.placementDataHandler.GetPlacementDatas(layer).Count <= 0) return new List<T>();
//
//             var propList = new List<T>();
//
//             float lastDistance = 9999;
//             foreach (var prop in DiscoData.Instance.GetPropList)
//             {
//                 if (prop == null) continue;
//
//                 if (prop.transform.TryGetComponent(out IOccupieable occupieable))
//                     if (occupieable.IsOccupied)
//                         continue;
//
//                 if (prop is T propType) propList.Add(propType);
//             }
//
//             if (propList.Count < 1)
//             {
//                 Debug.LogWarning(typeof(T) + " Turunde Prop Ogesi Bulunamadi!");
//                 return new List<T>();
//             }
//
//             return propList;
//         }
//     }
// }