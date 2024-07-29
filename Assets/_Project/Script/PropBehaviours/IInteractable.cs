namespace PropBehaviours
{
    public interface IInteractable
    {
        public eInteraction Interaction { get; }
        public void OnFocus();
        public void OnOutFocus();
        public void OnClick();
    }
}