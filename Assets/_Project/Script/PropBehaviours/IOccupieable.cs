using NPCBehaviour;

namespace PropBehaviours
{
    public interface IOccupieable
    {
        public NPC Owner { get; }
        bool IsOccupied { get; }
        public void SetOccupied(NPC owner, bool isOccupied);
    }
}