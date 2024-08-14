using System;
using System.Collections.Generic;
using BuildingSystem;
using Data;
using UnityEngine;

namespace PropBehaviours
{
    public class Table : IPropUnit, IPropUpdate
    {
        public List<Chair> Chairs;

        public void OnPropPlaced()
        {
            Chairs = new List<Chair>();
            foreach (var prop in DiscoData.Instance.GetPropList)
            {
                if (Chairs.Count > 3) break;

                if (prop is Chair chair)
                {
                    var distance = chair.CellPosition - CellPosition;
                    if (distance.magnitude <= 1 && IsChairFacingToThisTable(chair))
                    {
                        chair.IsReservedToATable = true;
                        Chairs.Add(chair);
                    }
                }
            }
        }

        public void PropUpdate()
        {
            OnPropRemoved();

            OnPropPlaced();
        }

        public void OnPropRemoved()
        {
            Debug.Log("OnRemoved Calisti");
            foreach (var chair in Chairs)
                chair.IsReservedToATable = false;

            Chairs.Clear();
        }

        private bool IsChairFacingToThisTable(Chair chair)
        {
            switch (chair.RotationData.direction)
            {
                case Direction.Down:
                    if (CellPosition.z - chair.CellPosition.z == 1)
                        return true;
                    break;
                case Direction.Up:
                    if (CellPosition.z - chair.CellPosition.z == -1)
                        return true;
                    break;
                case Direction.Left:
                    if (CellPosition.x - chair.CellPosition.x == 1)
                        return true;
                    break;
                case Direction.Right:
                    if (CellPosition.x - chair.CellPosition.x == -1)
                        return true;
                    break;
            }

            return false;
        }
    }
}