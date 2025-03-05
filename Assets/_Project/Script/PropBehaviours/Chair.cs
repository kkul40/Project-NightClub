using System.Character.NPC;
using UnityEngine;

namespace PropBehaviours
{
    public class Chair : IPropUnit, IOccupieable
    {
        [SerializeField] private Transform sitPosition;
        [SerializeField] private Transform frontPosition;

        public NPC Owner { get; private set; }
        public bool IsReservedToATable = false;

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

        public Transform GetFrontPosition()
        {
            return frontPosition;
        }

        public Vector3 GetSitPosition()
        {
            return sitPosition.position - new Vector3(0, 0.375f, 0);
            // 0.375f is the height of every chair
        }
    }
}