using UnityEngine;

public class Chair : Prop, IOccupieable
{
    [SerializeField] private Transform sitPosition;
    
    public bool IsOccupied { get; set; } = false;
    
    /// <summary>
    /// Returns Sit Position
    /// </summary>
    /// <returns></returns>
    public Vector3 GetItOccupied()
    {
        return sitPosition.position - new Vector3(0, 0.375f, 0);
    }
}

public interface IOccupieable
{
    bool IsOccupied { get; set; }
    public Vector3 GetItOccupied();
}