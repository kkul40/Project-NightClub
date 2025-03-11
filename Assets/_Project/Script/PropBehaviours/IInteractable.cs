using UnityEngine;

namespace PropBehaviours
{
    public interface IInteractable
    {
        public GameObject mGameobject { get; }
        public bool IsInteractable { get; }
        public bool hasInteractionAnimation { get; }
        public eInteraction Interaction { get; }
        public void OnFocus();
        public void OnOutFocus();
        public void OnClick();
        public void OnDeselect();
    }
}