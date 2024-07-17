namespace PropBehaviours
{
    public interface IOccupieable
    {
        public New_NPC.NPC Owner { get; }
        bool IsOccupied { get; }
        public void GetItOccupied(New_NPC.NPC owner);
    }
}