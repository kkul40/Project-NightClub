using System;
using BuildingSystem;
using Sirenix.Serialization;
using UnityEngine;

namespace PropBehaviours
{
    [SelectionBase]
    public class IPropUnit : MonoBehaviour, IInteractable
    {
        // public NewPropPlacementDataHandler.NewPlacementData PropData { get; private set; }
        public int ID { get; private set; }

        [OdinSerialize] public Vector3Int CellPosition { get; private set; }
        public Vector3 WorldPos { get; private set; }
        public RotationData RotationData { get; private set; }
        public ePlacementLayer PlacementLayer { get; private set; }

        public virtual void Initialize(int ID, Vector3Int cellPosition, RotationData rotationData,
            ePlacementLayer placementLayer)
        {
            this.ID = ID;
            CellPosition = cellPosition;
            WorldPos = GridHandler.Instance.GetCellCenterWorld(cellPosition, eGridType.PlacementGrid);
            RotationData = rotationData;
            PlacementLayer = placementLayer;
        }

        public eInteraction Interaction { get; } = eInteraction.PropUnit;

        public virtual void OnFocus()
        {
        }

        public virtual void OnOutFocus()
        {
        }

        public virtual void OnClick()
        {
        }

        public IPropUnit Copy()
        {
            var output = new IPropUnit();
            output.ID = ID;
            output.CellPosition = CellPosition;

            return output;
        }
    }
}