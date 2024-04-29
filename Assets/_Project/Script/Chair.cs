using ScriptableObjects;
using UnityEngine;

public class Chair : Prop, IOccupieable
{
    [SerializeField] private Transform sitPosition;
    [SerializeField] private Transform frontPosition;

    public NPC Owner { get; set; }
    
    [field : SerializeField]
    public bool IsOccupied { get; set; } = false;
    
    /// <summary>
    /// Returns Sit Position
    /// </summary>
    /// <returns></returns>
    public Vector3 GetItOccupied(NPC owner)
    {
        Owner = owner;
        return sitPosition.position - new Vector3(0, 0.375f, 0); // 0.375f is the height of every chair
    }
    public Vector3 GetFrontPosition() => frontPosition.position;
    public Vector3 GetSitPosition() => sitPosition.position;
}