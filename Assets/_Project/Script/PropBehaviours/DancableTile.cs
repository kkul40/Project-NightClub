using System;
using Data;
using UnityEngine;

namespace PropBehaviours
{
    public class DancableTile : IPropUnit, IPropUpdate, IOccupieable
    {
        public NPC.NPC Owner { get; set; }
        public bool IsOccupied { get; set; }

        public void SetOccupied(NPC.NPC owner, bool isOccupied)
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