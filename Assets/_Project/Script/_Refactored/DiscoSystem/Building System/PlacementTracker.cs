using System.Collections.Generic;
using Data;
using Disco_ScriptableObject;
using DiscoSystem.Building_System.GameEvents;
using ExtensionMethods;
using PropBehaviours;
using UnityEngine;
using UnityEngine.ProBuilder;

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
            GameEvent.Trigger(new Event_Sfx(SoundFXType.UIBack));
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
        public Vector3Int CellPos;

        public WallMaterialUndo(int previousMaterilID, int currentMaterialID, Vector3Int cellPos)
        {
            PreviousMaterilID = previousMaterilID;
            CurrentMaterialID = currentMaterialID;
            CellPos = cellPos;
        }

        public override void Undo()
        {
            var store = DiscoData.Instance.FindAItemByID(PreviousMaterilID);

            if (store is MaterialItemSo material)
            {
                DiscoData.Instance.MapData.GetWallDataByCellPos(CellPos).AssignNewID(material);
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

    public class MapSizeUndo : Revertable
    {
        public int ID;
        public Vector2Int ExtendSize;

        public MapSizeUndo(int id, Vector2Int extendSize)
        {
            ID = id;
            ExtendSize = extendSize;
        }

        // TODO : Implement This later
        public override void Undo()
        {
            DiscoData.Instance.MapData.RevertMapSize(ExtendSize.x, ExtendSize.y);
            UndoMoney(ID);
        }
    }

    public class WallDoorUndo : Revertable
    {
        public bool PreviousIsWallOnX;
        public int PreviousWallIndex;
        public int PreviousMaterialID;

        public WallDoorUndo(bool previousIsWallOnX, int previousWallIndex, int previousMaterialID)
        {
            PreviousIsWallOnX = previousIsWallOnX;
            PreviousWallIndex = previousWallIndex;
            PreviousMaterialID = previousMaterialID;
        }

        public override void Undo()
        {
            // Remove Current Door And Add Wall In Place
            var wallDoorData = _mapData.WallDatas.Find(x => x.assignedWall is WallDoor);
            
            _mapData.RemoveWallData(wallDoorData.CellPosition);

            bool isOnX = wallDoorData.CellPosition.x > wallDoorData.CellPosition.z;
            int index = Mathf.Max(wallDoorData.CellPosition.x, wallDoorData.CellPosition.z);
            if (isOnX)
                MapGeneratorSystem.Instance.InstantiateXWall(index);
            else
                MapGeneratorSystem.Instance.InstantiateYWall(index);

            _mapData.GetWallDataByCellPos(wallDoorData.CellPosition).AssignNewID(GetMaterial(wallDoorData.assignedMaterialID));

            // Remove Wall That was door before And Add Door
            Vector3Int cellPos = new Vector3Int(PreviousWallIndex, 0, 0);
            index = Mathf.Max(cellPos.x, cellPos.z);
            if (!PreviousIsWallOnX) cellPos = new Vector3Int(0, 0, PreviousWallIndex);

            _mapData.RemoveWallData(cellPos);

            GameObject door;
            if (PreviousIsWallOnX)
                door = MapGeneratorSystem.Instance.InstantiateXWallDoor(index);
            else
                door = MapGeneratorSystem.Instance.InstantiateYWallDoor(index);

            var doorData =_mapData.AddNewWallData(cellPos, door);
            doorData.AssignNewID(GetMaterial(PreviousMaterialID));
            
            
            DiscoData.Instance.MapData.ChangeDoorPosition(PreviousWallIndex, PreviousIsWallOnX);
        }

        private MapData _mapData => DiscoData.Instance.MapData;
        
        private MaterialItemSo GetMaterial(int index) => DiscoData.Instance.FindAItemByID(index) as MaterialItemSo;
    }
}