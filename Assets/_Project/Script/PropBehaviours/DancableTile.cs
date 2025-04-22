using System;
using Data;
using DiscoSystem.Character.NPC;
using UnityEngine;

namespace PropBehaviours
{
    public class DancableTile : IPropUnit, IOccupieable
    {
        public NPC Owner { get; set; }
        public bool IsOccupied { get; set; }

        public void SetOccupied(NPC owner, bool isOccupied)
        {
            Owner = owner;
            IsOccupied = true;
        }
    }
}