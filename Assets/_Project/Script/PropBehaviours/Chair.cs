using UnityEngine;

public class Chair : Prop, IOccupieable
{
    [SerializeField] private Transform sitPosition;
    [SerializeField] private Transform frontPosition;

    public NPC.NPC Owner { get; set; }

    [field: SerializeField] public bool IsOccupied { get; set; }

    /// <summary>
    ///     Returns Sit Position
    /// </summary>
    /// <returns></returns>
    public void GetItOccupied(NPC.NPC owner)
    {
        Owner = owner;
        IsOccupied = true;
    }

    public Vector3 GetFrontPosition()
    {
        return frontPosition.position;
    }

    public Vector3 GetSitPosition()
    {
        return sitPosition.position - new Vector3(0, 0.375f, 0);
        // 0.375f is the height of every chair
    }
}