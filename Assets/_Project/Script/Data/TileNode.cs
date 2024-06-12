using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class TileNode
    {
        public Vector3 WorldPos;
        public TileNode Parent;
        public bool IsWalkable;

        public int GridX;
        public int GridY;
        
        public int GCost;
        public int HCost;
        
        public int FCost
        {
            get { return GCost + HCost; }
        }
        
        public TileNode(bool isWalkable, Vector3 position, int gridX, int gridY)
        {
            IsWalkable = isWalkable;
            WorldPos = position;
            GridX = gridX;
            GridY = gridY;
        }

    }
}