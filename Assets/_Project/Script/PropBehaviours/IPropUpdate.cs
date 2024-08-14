namespace PropBehaviours
{
    public interface IPropUpdate
    {
        public void OnPropPlaced();
        public void PropUpdate();
        public void OnPropRemoved();
    }
}