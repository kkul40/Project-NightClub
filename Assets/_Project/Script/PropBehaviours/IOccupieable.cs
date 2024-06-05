namespace PropBehaviours
{
    public interface IOccupieable
    {
        public New_NPC.NPC Owner { get; set; }
        bool IsOccupied { get; set; }
        public void GetItOccupied(New_NPC.NPC owner);
    }
}