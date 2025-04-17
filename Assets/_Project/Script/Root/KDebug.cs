using System;
using Data;
using DiscoSystem;
using UnityEngine;

namespace Root
{
    public class KDebug : Singleton<KDebug>
    {
        public GameObject TestCube;
        public bool showPlacements = false;
        public bool showPathFinder = false;
        public bool showAvaliableLeanPosition = false;

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            // if (showPlacements)
            // {
            //     var keys = DiscoData.Instance.placementDataHandler.GetUsedKeys(ePlacementLayer.FloorProp);
            //     var rest = DiscoData.Instance.placementDataHandler.GetUsedKeys(ePlacementLayer.BaseSurface);
            //     foreach (var r in rest) keys.Add(r);
            //
            //     foreach (var key in keys)
            //     {
            //         Gizmos.color = Color.cyan;
            //         var offset = new Vector3(0.5f, 0.5f, 0.5f);
            //         Gizmos.DrawCube(key + offset, Vector3.one);
            //     }
            // }

            if (showPathFinder)
                foreach (var node in DiscoData.Instance.MapData.Path.GetPaths())
                {
                    if (node.HasOccupied)
                        Gizmos.color = Color.blue;
                    else if(node.IsWall)
                        Gizmos.color = Color.black;
                    else if (node.OnlyEmployee)
                        Gizmos.color = Color.cyan;
                    else if (node.IsWalkable)
                        Gizmos.color = Color.green;
                    else
                        Gizmos.color = Color.red;

                    Gizmos.DrawCube(node.WorldPos, DefaultSize / ConstantVariables.PathFinderGridSize);
                }

            // if (showAvaliableLeanPosition)
            // {
            //     foreach (var node in MapGeneratorSystem.Instance.MapData.Path.GetAvaliableWallPaths)
            //     {
            //         Gizmos.color = Color.magenta;
            //         Gizmos.DrawCube(node.WorldPos, DefaultSize / ConstantVariables.PathFinderGridSize);
            //     }
            // }
        }

        public Vector3 DefaultSize => new Vector3(0.8f, 0.8f, 0.8f);

        public static void Print(string message)
        {
            // Debug.Log(message);
        }
    }
}