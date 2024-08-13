using BuildingSystem;
using Data;
using New_NPC;
using UnityEngine;

namespace PropBehaviours
{
    public class Chair : IPropUnit, IOccupieable
    {
        [SerializeField] private Transform sitPosition;
        [SerializeField] private Transform frontPosition;

        public New_NPC.NPC Owner { get; private set; }

        [field: SerializeField] public bool IsOccupied { get; private set; }

        /// <summary>
        ///     Returns Sit Position
        /// </summary>
        /// <returns></returns>
        public void SetOccupied(NPC owner, bool isOccupied)
        {
            Owner = owner;
            IsOccupied = isOccupied;
        }

        public Transform GetFrontPosition() => frontPosition;

        public Vector3 GetSitPosition()
        {
            return sitPosition.position - new Vector3(0, 0.375f, 0);
            // 0.375f is the height of every chair
        }

        public override void Initialize(int ID, Vector3Int cellPosition, RotationData rotationData,
            ePlacementLayer placementLayer)
        {
            base.Initialize(ID, cellPosition, rotationData, placementLayer);
        }
    }
}