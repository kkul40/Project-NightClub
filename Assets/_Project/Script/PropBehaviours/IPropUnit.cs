using Data;
using Disco_Building;
using GameEvents;
using Sirenix.OdinInspector;
using UI.GamePages;
using UI.PopUp;
using UnityEngine;

namespace PropBehaviours
{
    [SelectionBase]
    public class IPropUnit : MonoBehaviour, IInteractable
    {
        [ShowInInspector] public bool IsInitialized;
        [ShowInInspector] public int ID { get; private set; }
        [ShowInInspector] public Vector3Int CellPosition { get; private set; }
        [ShowInInspector] public Vector3 WorldPos { get; private set; }
        [ShowInInspector] public RotationData RotationData { get; private set; }
        [ShowInInspector] public ePlacementLayer PlacementLayer { get; private set; }
        
        // Events
       
        public virtual void Initialize(
            int ID, 
            Vector3Int cellPosition, 
            RotationData rotationData, 
            ePlacementLayer placementLayer)
        {
            IsInitialized = true;
            this.ID = ID;
            CellPosition = cellPosition;
            // WorldPos = cellPosition.CellCenterPosition(eGridType.PlacementGrid);
            WorldPos = transform.position;
            RotationData = rotationData;
            PlacementLayer = placementLayer;
        }

        public GameObject mGameobject => gameObject;
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
            UIPageManager.Instance.ShowActionSelectionPage(this);
        }

        public void OnDeselect()
        {
            // TODO Close The Page
        }

        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            transform.rotation = rotation;
            transform.position = position;

            WorldPos = position;
        }
    }
}