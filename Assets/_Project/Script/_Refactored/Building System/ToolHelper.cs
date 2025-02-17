using Data;
using Disco_Building;
using Disco_ScriptableObject;
using DiscoSystem;
using UnityEngine;

public class ToolHelper
{
    public const int GroundLayerID = 10;
    public const int SurfaceLayerID = 10;
    
    //
    public InputSystem InputSystem;
    public DiscoData DiscoData;
    public MaterialColorChanger MaterialColorChanger;
    public FXCreator FXCreator;
    
    //
    public StoreItemSO SelectedStoreItem;
    public Vector3Int CellPosition;
    
    //
    public Quaternion LastRotation;
    public Vector3 LastPosition;
    
    public ToolHelper(InputSystem inputSystem, DiscoData discoData,
        MaterialColorChanger materialColorChanger, FXCreator fxCreator)
    {
        InputSystem = inputSystem;
        DiscoData = discoData;
        MaterialColorChanger = materialColorChanger;
        FXCreator = fxCreator;
    }
    
    public Vector3 SnapToGrid(Vector3 position, GridSizes gridSizes)
    {
        float size = gridSizes switch
        {
            GridSizes.FullGrid => 1,
            GridSizes.HalfGrid => 0.5f,
        };
            
        float x = Mathf.Round(position.x / size) * size;
        float y = Mathf.Round(position.y / size) * size;
        float z = Mathf.Round(position.z / size) * size;

        return new Vector3(x, y, z);
    }

    public Quaternion SnappyRotate(Quaternion currentQuaternion, int rotateDirection)
    {
        Quaternion quaternion = currentQuaternion;
            
        float yValue = quaternion.eulerAngles.y;
            
        int i = Mathf.RoundToInt(currentQuaternion.eulerAngles.y / 45) + rotateDirection;

        yValue = i * 45;
            
        quaternion.eulerAngles = new Vector3(0, yValue % 360, 0);
            
        return quaternion;
    }

    public Quaternion FreeRotate(Quaternion currentQuaternion, float rotateDirection)
    {
        Quaternion quaternion = currentQuaternion;
        float angle = currentQuaternion.eulerAngles.y;

        angle += rotateDirection * 10;
        quaternion.eulerAngles = new Vector3(0, angle, 0);
            
        return quaternion;
    }
}