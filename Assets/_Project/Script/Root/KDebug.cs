using System;
using System.Linq;
using Data;
using UnityEngine;

namespace Root
{
    public class KDebug : MonoBehaviour
    {
        public bool showPlacements = false;

        public bool showPaths = false;

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
                    Gizmos.color = Color.green;
                    var offset = new Vector3(0.5f, 0.5f, 0.5f);
                    Gizmos.DrawCube(key + offset, Vector3.one);
                }
            }

            if (showPaths)
                foreach (var node in DiscoData.Instance.MapData.GetNewPathFinderNote())
                {
                    if (node.IsMarked)
                        Gizmos.color = Color.blue;
                    else if (node.IsWalkable)
                        Gizmos.color = Color.green;
                    else
                        Gizmos.color = Color.red;

                    Gizmos.DrawCube(node.WorldPos, new Vector3(0.2f, 0.2f, 0.2f));
                }
        }

        public static void Print(string message)
        {
            // Debug.Log(message);
        }
    }
}