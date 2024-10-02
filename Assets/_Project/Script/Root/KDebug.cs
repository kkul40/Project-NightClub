using System;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Root
{
    public class KDebug : Singleton<KDebug>
    {
        public bool showPlacements = false;
        public bool showPathFinder = false;

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            if (showPlacements)
            {
                var keys = DiscoData.Instance.placementDataHandler.GetUsedKeys(ePlacementLayer.FloorProp);
                var rest = DiscoData.Instance.placementDataHandler.GetUsedKeys(ePlacementLayer.BaseSurface);
                foreach (var r in rest) keys.Add(r);

                foreach (var key in keys)
                {
                    Gizmos.color = Color.cyan;
                    var offset = new Vector3(0.5f, 0.5f, 0.5f);
                    Gizmos.DrawCube(key + offset, Vector3.one);
                }
            }

            if (showPathFinder)
                foreach (var node in DiscoData.Instance.MapData.GetPathFinderNodes())
                {
                    if (node.HasOccupied)
                        Gizmos.color = Color.blue;
                    else if(node.IsWall)
                        Gizmos.color = Color.black;
                    else if (node.IsWalkable)
                        Gizmos.color = Color.green;
                    else
                        Gizmos.color = Color.red;

                    Gizmos.DrawCube(node.WorldPos, new Vector3(0.8f, 0.8f, 0.8f) / ConstantVariables.PathFinderGridSize);
                }
        }

        public static void Print(string message)
        {
            // Debug.Log(message);
        }
    }
}