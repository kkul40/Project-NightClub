using UnityEngine;

public class Prop : MonoBehaviour
{
    private PlacablePropSo _placablePropSo;
    public Vector3 propPosition;

    public void Initialize(PlacablePropSo placablePropSo, Vector3 propPos)
    {
        this._placablePropSo = placablePropSo;
        this.propPosition = propPos;
        GameData.Instance.AddProp(this);
    }

    public Vector3 GetPropPosition() => propPosition;
    public Quaternion GetPropRotation() => transform.rotation;
    public PlacablePropSo GetPropSo() => _placablePropSo;
}