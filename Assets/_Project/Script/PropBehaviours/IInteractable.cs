namespace PropBehaviours
{
    public interface IInteractable
    {
        public bool IsInteractable { get; }
        public eInteraction Interaction { get; }
        public void OnFocus();
        public void OnOutFocus();
        public void OnClick();
    }
}