using UnityEngine;

namespace ScriptableObjects
{
    public interface IOccupieable
    {
        public NPC Owner { get; set; }
        bool IsOccupied { get; set; }
        public Vector3 GetItOccupied(NPC owner);
    }
}