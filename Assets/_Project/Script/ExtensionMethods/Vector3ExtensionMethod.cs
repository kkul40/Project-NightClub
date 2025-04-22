
using UnityEngine;

namespace ExtensionMethods
{
    public static class Vector3ExtensionMethod
    {
        public static Vector3 ToFloat(this Vector3Int vector)
        {
            return new Vector3(vector.x, vector.y, vector.z);
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

        public static Vector3Int Add(this Vector3Int vector, int? x = null, int? y = null, int? z = null)
        {
            Vector3 output = vector.ToFloat().Add(x, y, z);
            return new Vector3Int((int)output.x, (int)output.y, (int)output.z);
        }
        
        public static Vector3Int FlattenToGround(this Vector3Int vector)
        {
            return new Vector3Int(vector.x, 0, vector.z);
        }

        public static Vector3 SetValue(this Vector3 vector, int? x = null, int? y = null, int? z = null)
        {
            vector.x = x ?? vector.x;
            vector.y = y ?? vector.y;
            vector.z = z ?? vector.z;

            return vector;
        }
    }
}