using System;
using BuildingSystem;
using Data;
using UnityEngine;

namespace PropBehaviours
{
    public class DancableTile : IPropUnit, IPropUpdate, IOccupieable
    {
        public Vector3 GetMiddlePos => CellPosition + new Vector3(0.5f, 0, 0.5f);

        public override void Initialize(int ID, Vector3Int cellPosition, RotationData rotationData,
            ePlacementLayer placementLayer)
        {
            base.Initialize(ID, cellPosition, rotationData, placementLayer);
        }

        public New_NPC.NPC Owner { get; set; }
        public bool IsOccupied { get; set; }

        public void GetItOccupied(New_NPC.NPC owner)
        {
            Owner = owner;
            IsOccupied = true;
        }

        public void PropUpdate()
        {
            Debug.Log("Dancable Area Updated");
        }
    }
}