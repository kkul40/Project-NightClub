using System;
using BuildingSystem;
using Data;
using NPC_Stuff;
using UnityEngine;

namespace PropBehaviours
{
    public class DancableTile : IPropUnit, IPropUpdate, IOccupieable
    {
        public NPC Owner { get; set; }
        public bool IsOccupied { get; set; }

        public void SetOccupied(NPC owner, bool isOccupied)
        {
            Owner = owner;
            IsOccupied = true;
        }

        public void OnPropPlaced()
        {
        }

        public void PropUpdate()
        {
        }

        public void OnPropRemoved()
        {
        }
    }
}