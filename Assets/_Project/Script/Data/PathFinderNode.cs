using System;
using System.Character.NPC;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class PathFinderNode
    {
        public Vector3 WorldPos = -Vector3.one;
        public PathFinderNode Parent;
        public bool IsWalkable = false;
        public bool OnlyEmployee = false; // Only Employee Can Walk
        public bool IsWall = false;
        public bool IsPlacable = true;

        public bool HasOccupied { get; private set; }
        public bool ChangeOccupition(NPC source, bool hasOccuipied) => HasOccupied = hasOccuipied;

        public bool GetIsWalkable
        {
            get
            {
                if (!IsWalkable || IsWall) return false;
                return true;
            }
        }

        // Testing
        public ePathNodeType PathNodeType;

        public int GridX = -1;
        public int GridY = -1;

        public int GCost = 0;
        public int HCost = 0;

        public int FCost => GCost + HCost;

        public PathFinderNode()
        {
            HasOccupied = false;
        }

        public PathFinderNode Init()
        {
            return new PathFinderNode();
        }

        public PathFinderNode Copy()
        {
            var output = new PathFinderNode();

            output.WorldPos = WorldPos;
            output.Parent = Parent;
            output.IsWalkable = IsWalkable;
            output.OnlyEmployee = OnlyEmployee;
            output.GridX = GridX;
            output.GridY = GridY;
            output.HasOccupied = HasOccupied;

            return output;
        }
    }

    public enum ePathNodeType
    {
        Null,
        Walkable,
        Waitable
    }
}