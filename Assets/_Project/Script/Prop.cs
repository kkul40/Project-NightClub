using ScriptableObjects;
using UnityEngine;

public class Prop : MonoBehaviour, IRemovable
{
    private PlacablePropSo _placablePropSo;
    public Vector3Int propPosition;
    public Direction direction;

    public virtual void Initialize(PlacablePropSo placablePropSo, Vector3Int propPos, Direction direction)
    {
        this._placablePropSo = placablePropSo;
        this.propPosition = propPos;
        this.direction = direction;
    }

    public Vector3 GetPropPosition() => propPosition;
    public Quaternion GetPropRotation() => transform.rotation;
    public PlacablePropSo GetPropSo() => _placablePropSo;
    
    public void Remove()
    {
        throw new System.NotImplementedException();
    }
}

public enum Direction
{
    Up,
    Right,
    Down,
    Left,
}