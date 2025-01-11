using UnityEngine;

namespace PropBehaviours
{
    public interface IInteractable
    {
        public GameObject mGameobject { get; }
        public bool IsInteractable { get; }
        public bool IsAnimatable { get; }
        public eInteraction Interaction { get; }
        public void OnFocus();
        public void OnOutFocus();
        public void OnClick();
    }
}