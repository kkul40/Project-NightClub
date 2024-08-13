using New_NPC;

namespace PropBehaviours
{
    public interface IOccupieable
    {
        public New_NPC.NPC Owner { get; }
        bool IsOccupied { get; }
        public void SetOccupied(New_NPC.NPC owner, bool isOccupied);
    }
}