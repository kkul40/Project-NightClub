using System;
using UnityEngine;

namespace Data
{
    public class KDebugger : MonoBehaviour
    {
        [SerializeField] private bool showTileDataWalkable;


        private void OnDrawGizmos()
        {
            if (showTileDataWalkable && MapGeneratorSystem.Instance.MapData != null)
            {
                foreach (var node in MapGeneratorSystem.Instance.MapData.GetPathFinderNode())
                {
                    if (node.IsWalkable)
                        Gizmos.color = Color.green;
                    else
                        Gizmos.color = Color.red;
                    
                    Gizmos.DrawCube(node.WorldPos.Add(y:0.005f), new Vector3(1,0.1f, 1));
                }
            }
        }
    }
}