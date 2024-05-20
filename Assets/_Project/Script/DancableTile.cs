using ScriptableObjects;
using UnityEngine;

public class DancableTile : Prop, IPropUpdate, IOccupieable, IInteractable
{
    public override void Initialize(PlacablePropSo placablePropSo, Vector3Int cellPosition, Direction direction)
    {
        base.Initialize(placablePropSo, cellPosition, direction);
    }

    public void PropUpdate()
    {
        Debug.Log("Dancable Area Updated");
    }

    public Vector3 GetMiddlePos => CellPosition + new Vector3(0.5f, 0, 0.5f);

    public NPC Owner { get; set; }
    public bool IsOccupied { get; set; }
    public void GetItOccupied(NPC owner)
    {
        Owner = owner;
        IsOccupied = true;
    }

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
}