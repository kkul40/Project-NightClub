using System;
using _Initializer;
using Data;
using DiscoSystem;
using UnityEngine;

namespace Root
{
    public class KDebug : MonoBehaviour
    {
        public GameObject TestCube;
        public bool showPlacements = false;
        public bool showPathFinder = false;
        public bool showEmployeeFinder = false;
        public bool showAvaliableLeanPosition = false;
        public bool showActivityNodes = false;

        private void Awake()
        {
            ServiceLocator.Register(this);
        }

#if UNITY_EDITOR
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

            if (showPathFinder && DiscoData.Instance != null)
                foreach (var node in DiscoData.Instance.MapData.Path.GetPaths())
                {
                    if (node.IsWalkable)
                        Gizmos.color = Color.green;
                    else
                        Gizmos.color = Color.red;
                    
                    if(showActivityNodes && node.OnlyActivity)
                        Gizmos.color = Color.yellow;
                    
                    if (showEmployeeFinder && node.OnlyEmployee)
                        Gizmos.color = Color.cyan;
                    
                    if (node.HasOccupied)
                        Gizmos.color = Color.blue;
                    if(node.IsWall)
                        Gizmos.color = Color.black;

                    Gizmos.DrawCube(node.WorldPos, DefaultSize / ConstantVariables.PathFinderGridSize);
                }
            
            
            if (showAvaliableLeanPosition)
            {
                foreach (var node in DiscoData.Instance.MapData.Path.GetAvaliableWallPaths)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawCube(node.WorldPos, DefaultSize / ConstantVariables.PathFinderGridSize);
                }
            }
        }
#endif

        public Vector3 DefaultSize => new Vector3(0.8f, 0.8f, 0.8f);

        public static void Print(string message)
        {
            // Debug.Log(message);
        }
    }
}