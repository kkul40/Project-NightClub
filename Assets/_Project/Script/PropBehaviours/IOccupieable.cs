namespace PropBehaviours
{
    public interface IOccupieable
    {
        public NPC.NPC Owner { get; }
        bool IsOccupied { get; }
        public void SetOccupied(NPC.NPC owner, bool isOccupied);
    }
}