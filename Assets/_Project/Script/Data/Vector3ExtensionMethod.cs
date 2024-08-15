using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Data
{
    public static class Vector3ExtensionMethod
    {
        public static Vector3 BuildingOffset(this Vector3 vector, ePlacementLayer placementLayer)
        {
            switch (placementLayer)
            {
                case ePlacementLayer.WallProp:
                    vector = Vector3.zero;
                    break;
                case ePlacementLayer.BaseSurface:
                case ePlacementLayer.FloorProp:
                    vector = new Vector3(0, -0.5f, 0);
                    break;
            }

            return vector;
        }

        public static Vector3 Add(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(vector.x + (x ?? 0), vector.y + (y ?? 0), vector.z + (z ?? 0));
        }

        public static Vector3 AddVector(this Vector3 vector, Vector3? vectorToAdd = null)
        {
            return vector + (vectorToAdd ?? Vector3.zero);
        }

        public static Vector3 SubVector(this Vector3 vector, Vector3? vectorToSubtract = null)
        {
            return vector - (vectorToSubtract ?? Vector3.zero);
        }
        
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