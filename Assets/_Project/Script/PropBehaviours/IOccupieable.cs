namespace PropBehaviours
{
    public interface IOccupieable
    {
        public NPC.NPC Owner { get; set; }
        bool IsOccupied { get; set; }
        public void GetItOccupied(NPC.NPC owner);
    }
}