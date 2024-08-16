using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public static class PlacementExtensionMethod
    {
        public static List<Vector3Int> GetNearByKeys(this Vector3Int vector)
        {
            List<Vector3Int> keys = new List<Vector3Int>();
            
            keys.Add(vector + new Vector3Int(-1,0,1));
            keys.Add(vector + new Vector3Int(0,0,1));
            keys.Add(vector + new Vector3Int(1,0,1));
            
            keys.Add(vector + new Vector3Int(-1,0,0));
            keys.Add(vector + new Vector3Int(1,0,0));
            
            keys.Add(vector + new Vector3Int(-1,0,-1));
            keys.Add(vector + new Vector3Int(0,0,-1));
            keys.Add(vector + new Vector3Int(1,0,-1));
            
            return keys;
        }
    }
}