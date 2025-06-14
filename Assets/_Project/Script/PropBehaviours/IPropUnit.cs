using _Initializer;
using Data;
using Sirenix.OdinInspector;
using UI.GamePages;
using UnityEngine;

namespace PropBehaviours
{
    [SelectionBase]
    public class IPropUnit : MonoBehaviour, IInteractable
    {
        [ShowInInspector] public bool IsInitialized;
        [ShowInInspector] public int ID { get; private set; }
        [ShowInInspector] public Vector3 WorldPos => transform.position;
        [ShowInInspector] public ePlacementLayer PlacementLayer { get; private set; }
        
        // Events
       
        public virtual void Initialize(
            int ID, 
            ePlacementLayer placementLayer)
        {
            IsInitialized = true;
            this.ID = ID;
            PlacementLayer = placementLayer;
            mGameobject = gameObject;
        }

        public GameObject mGameobject { get; protected set; }
        public bool IsInteractable { get; protected set; } = true;
        public bool hasInteractionAnimation { get; } = true;
        public eInteraction Interaction { get; } = eInteraction.PropUnit;

        public virtual void OnFocus()
        {
        }

        public virtual void OnOutFocus()
        {
        }

        public virtual void OnClick()
        {
            ServiceLocator.Get<UIPageManager>().ShowActionSelectionPage(this);
        }

        public void OnDeselect()
        {
            // TODO Close The Page
        }
        
        public virtual void OnRelocated()
        {
        }
    }
}