using System;
using BuildingSystem;
using Data;
using UnityEngine;

namespace PropBehaviours
{
    public class DancableTile : IPropUnit, IPropUpdate, IOccupieable
    {
        public New_NPC.NPC Owner { get; set; }
        public bool IsOccupied { get; set; }

        public void GetItOccupied(New_NPC.NPC owner)
        {
            Owner = owner;
            IsOccupied = true;
        }

        public void PropUpdate()
        {
            // Debug.Log("Dancable Area Updated");
        }
    }
}