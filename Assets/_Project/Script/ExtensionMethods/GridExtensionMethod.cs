using System;
using Data;
using UnityEngine;

namespace ExtensionMethods
{
    public static class GridExtensionMethod
    {
        /// <summary>
        /// Finds The PathFinder Index By PlacementPos
        ///
        /// Which you can use to Set PathFinderNode Accuratly
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector2Int PlacementPosToPathFinderIndex(this Vector3Int vector)
        {
            return new Vector2Int(vector.x, vector.z) * ConstantVariables.PathFinderGridSize;
        }

        /// <summary>
        /// Converts a world position to grid cell position.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="cellGridType"></param>
        /// <returns></returns>
        public static Vector3Int WorldPosToCellPos(this Vector3 vector, eGridType cellGridType)
        {
            switch (cellGridType)
            {
                case eGridType.PlacementGrid:
                    return new Vector3Int((short)vector.x, (short)vector.y, (short)vector.z); // 1 Birim
                case eGridType.PathFinderGrid:
                    var x = vector.x / (1f / ConstantVariables.PathFinderGridSize);
                    var y = vector.y / (1f / ConstantVariables.PathFinderGridSize);
                    var z = vector.z / (1f / ConstantVariables.PathFinderGridSize);
                    return new Vector3Int((short)x, (short)y, (short)z);
            }

            Debug.LogError("Conversion Returned Null when trying : " + cellGridType.ToString());
            return -Vector3Int.one;
        }

        /// <summary>
        /// Gets the Centrol Position of a grid cellposition while keeping height unchanged
        /// </summary>
        /// <param name="vectorInt"></param>
        /// <param name="cellGridType"></param>
        /// <returns></returns>
        public static Vector3 CellCenterPosition(this Vector3Int vector, eGridType cellGridType)
        {
            Vector3 vector3 = new Vector3(vector.x, vector.y, vector.z);
            return CellCenterPosition(vector3, cellGridType);
        }

        /// <summary>
        /// Gets the Centrol Position of a grid cellposition while keeping height unchanged
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="cellGridType"></param>
        /// <returns></returns>
        public static Vector3 CellCenterPosition(this Vector3 vector, eGridType cellGridType)
        {
            switch (cellGridType)
            {
                case eGridType.PlacementGrid:
                    float centerOffset1x1 = 0.5f;
                    return new Vector3(vector.x + centerOffset1x1, vector.y, vector.z + centerOffset1x1);
                case eGridType.PathFinderGrid:
                    float centerOffset4x4 = 1f / ConstantVariables.PathFinderGridSize;
                    return new Vector3(vector.x * centerOffset4x4, 0, vector.z * centerOffset4x4);
            }

            Debug.LogError("Returned Null : " + cellGridType.ToString());
            return -Vector3.one;
        }
    }
}