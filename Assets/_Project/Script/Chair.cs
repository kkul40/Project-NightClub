using ScriptableObjects;
using UnityEngine;

public class Chair : Prop, IOccupieable
{
    [SerializeField] private Transform sitPosition;

    public NPC Owner { get; set; }
    public bool IsOccupied { get; set; } = false;
    
    /// <summary>
    /// Returns Sit Position
    /// </summary>
    /// <returns></returns>
    public Vector3 GetItOccupied(NPC owner)
    {
        Owner = owner;
        return sitPosition.position - new Vector3(0, 0.375f, 0);
    }
}