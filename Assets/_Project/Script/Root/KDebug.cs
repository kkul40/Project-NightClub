using System;
using System.Linq;
using Data;
using UnityEngine;

namespace Root
{
    public class KDebug : MonoBehaviour
    {
        public bool showPlacements = false;
        
        private void OnDrawGizmosSelected()
        {
            if (showPlacements)
            {
                var keys = DiscoData.Instance.placementDataHandler.GetUsedKeys(ePlacementLayer.Floor);
                var rest = DiscoData.Instance.placementDataHandler.GetUsedKeys(ePlacementLayer.Surface);


                foreach (var r in rest)
                {
                    keys.Add(r);
                }

                foreach (var key in keys)
                {
                    Gizmos.color = Color.green;
                    var offset = new Vector3(0.5f, 0.5f, 0.5f);
                    Gizmos.DrawCube(key + offset, Vector3.one);
                }
            }
        }
    }
}