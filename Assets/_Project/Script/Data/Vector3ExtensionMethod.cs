using System.Collections;
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

        public static Vector3 SubtractVector(this Vector3 vector, Vector3? vectorToSubtract = null)
        {
            return vector - (vectorToSubtract ?? Vector3.zero);
        }
    }
}