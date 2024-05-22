using System;
using ScriptableObjects;
using UnityEngine;

[Serializable]
public class Prop : MonoBehaviour, IProp
{
    public Vector3Int CellPosition;
    public Direction direction;
    private IPlaceableItemData _placableItemDataSo;

    public virtual void Initialize(IPlaceableItemData placableItemDataSo, Vector3Int cellPosition, Direction direction)
    {
        _placableItemDataSo = placableItemDataSo;
        CellPosition = cellPosition;
        this.direction = direction;
    }

    public Vector3Int GetPropCellPosition()
    {
        return CellPosition;
    }

    public Quaternion GetPropRotation()
    {
        return transform.rotation;
    }

    public IPlaceableItemData GetPropSo()
    {
        return _placableItemDataSo;
    }
}

public interface IProp
{
}

public struct PropData
{
    public int ID { get; }
    public Vector3Int cellPosition { get; }
    public RotationData rotationData { get; }

    public PropData(int ID, Vector3Int cellPosition, RotationData rotationData)
    {
        this.ID = ID;
        this.cellPosition = cellPosition;
        this.rotationData = rotationData;
    }
}