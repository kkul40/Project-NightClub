using BuildingSystem;
using UnityEngine;

namespace PropBehaviours
{
    public class DancableTile : Prop, IPropUpdate, IOccupieable, IInteractable
    {
        public Vector3 GetMiddlePos => CellPosition + new Vector3(0.5f, 0, 0.5f);

        public eInteraction Interaction { get; } = eInteraction.Interactable;

        public void OnFocus()
        {
        }

        public void OnOutFocus()
        {
        }

        public void OnClick()
        {
        }

        public NPC.NPC Owner { get; set; }
        public bool IsOccupied { get; set; }

        public void GetItOccupied(NPC.NPC owner)
        {
            Owner = owner;
            IsOccupied = true;
        }

        public void PropUpdate()
        {
            Debug.Log("Dancable Area Updated");
        }

        public override void Initialize(Vector3Int cellPosition, Direction direction)
        {
            base.Initialize(cellPosition, direction);
        }
    }
}