using System;
using Data;
using Disco_Building;
using ExtensionMethods;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UI.GamePages;
using UnityEngine;

namespace PropBehaviours
{
    [SelectionBase]
    public class IPropUnit : MonoBehaviour, IInteractable
    {
        [ShowInInspector] public int ID { get; private set; }
        [ShowInInspector] public Vector3Int CellPosition { get; private set; }
        [ShowInInspector] public Vector3 WorldPos { get; private set; }
        [ShowInInspector] public RotationData RotationData { get; private set; }
        [ShowInInspector] public ePlacementLayer PlacementLayer { get; private set; }

        public virtual void Initialize(
            int ID, 
            Vector3Int cellPosition, 
            RotationData rotationData, 
            ePlacementLayer placementLayer)
        {
            this.ID = ID;
            CellPosition = cellPosition;
            WorldPos = cellPosition.CellCenterPosition(eGridType.PlacementGrid);
            RotationData = rotationData;
            PlacementLayer = placementLayer;
        }

        public bool IsInteractable { get; protected set; } = true;
        public eInteraction Interaction { get; } = eInteraction.PropUnit;

        public virtual void OnFocus()
        {
        }

        public virtual void OnOutFocus()
        {
        }

        public virtual void OnClick()
        {
            UIPageManager.Instance.RequestAPage(typeof(UIActionSelectionPage), this);
        }
    }
}