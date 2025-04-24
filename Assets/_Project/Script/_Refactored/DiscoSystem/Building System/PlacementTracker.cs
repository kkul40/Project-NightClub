using System.Collections.Generic;
using Data;
using Disco_ScriptableObject;
using DiscoSystem.Building_System.GameEvents;
using UnityEngine;

namespace DiscoSystem.Building_System
{
    public class PlacementTracker
    {
        private Stack<IRevertable> _revertables;

        public PlacementTracker() => _revertables = new Stack<IRevertable>();

        public void AddTrack(IRevertable revertable)
        {
            _revertables.Push(revertable);
        }

        public void Undo()
        {
            if (_revertables.Count == 0) return;

            var revertable = _revertables.Pop();
            revertable.Undo();
        }
    }

    public interface IRevertable
    {
        public void Undo();
    }
    
    public abstract class Revertable : IRevertable
    {
        public abstract void Undo();

        protected void UndoMoney(int storeItemID)
        {
            StoreItemSO item = DiscoData.Instance.FindAItemByID(storeItemID);
            GameEvent.Trigger(new Event_MoneyAdded(item.Price));
        }
    }

    public class PropUndo : Revertable
    {
        public int StoreItemId;
        public int InstanceId;

        public PropUndo(int storeItemId, int instanceId)
        {
            StoreItemId = storeItemId;
            InstanceId = instanceId;
        }

        public override void Undo()
        {
            GameEvent.Trigger(new Event_RemovePlacement(InstanceId));
            UndoMoney(StoreItemId);
        }
    }

    public class WallMaterialUndo : Revertable
    {
        public int PreviousMaterilID;
        public int CurrentMaterialID;
        public WallData WallData;

        public WallMaterialUndo(int previousMaterilID, int currentMaterialID, WallData wallData)
        {
            PreviousMaterilID = previousMaterilID;
            CurrentMaterialID = currentMaterialID;
            WallData = wallData;
        }

        public override void Undo()
        {
            var store = DiscoData.Instance.FindAItemByID(PreviousMaterilID);

            if (store is MaterialItemSo material)
            {
                WallData.AssignNewID(material);
                UndoMoney(CurrentMaterialID);
            }
            else
                Debug.LogError("Assigned Materials Is Not MaterialItemSo");
        }
    }
    
    public class FloorMaterialUndo : Revertable
    {
        public int PreviousMaterilID;
        public int CurrentMaterialID;
        public FloorData FloorData;

        public FloorMaterialUndo(int previousMaterilID, int currentMaterialID, FloorData floorData)
        {
            PreviousMaterilID = previousMaterilID;
            CurrentMaterialID = currentMaterialID;
            FloorData = floorData;

        }

        public override void Undo()
        {
            var store = DiscoData.Instance.FindAItemByID(PreviousMaterilID);
            
            if (store is MaterialItemSo material)
            {
                FloorData.AssignNewID(material);
                UndoMoney(CurrentMaterialID);
            }
            else
                Debug.LogError("Assigned Materials Is Not MaterialItemSo");
        }
    }
}