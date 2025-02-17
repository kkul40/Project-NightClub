using System.Collections.Generic;
using Data;
using Disco_Building;
using Disco_ScriptableObject;
using DiscoSystem;
using PropBehaviours;
using UnityEngine;

public class IFloorPlacementTool : ITool
{
    private GameObject _tempObject;

    private PlacementItemSO _placementItem;
    private Vector3 mouseDeltaStart;
    private List<MeshRenderer> _tempMeshRenderer;

    private Transform hitSurface;
    private bool isSnappedToSurface;
    private Vector3 LastPosition;
    private Quaternion LastRotation;
    
    // Box Collider Values
    // private Collider _collider;
    private Vector3 colliderExtend;
    private Vector3 colliderSize;

    private Collider[] Colliders;


    public bool isFinished { get; }

    public void OnStart(ToolHelper TH)
    {
        Debug.Log("Floor Placemenet Tool Started");
        _placementItem = TH.SelectedStoreItem as PlacementItemSO;

        _tempObject = Object.Instantiate(_placementItem.Prefab, TH.LastPosition, TH.LastRotation);
        _tempObject.transform.SetParent(null);

        CalculateBounds();
        _tempMeshRenderer = TH.MaterialColorChanger.ReturnMeshRendererList(_tempObject);
    }
    
    public bool OnValidate(ToolHelper TH)
    {
        // Check For Wall
        // Check For Other Object
        // Check For In Boundries
        // Check For Surface
        
        // Boundry Check
        Vector3 center = GetCenterOfBounds();
        Vector3 size = colliderSize * 0.98f;
        Vector3 TopCenter = center + new Vector3(0f, size.y / 2, 0f);
        Vector3 BottomCenter = center - new Vector3(0f, size.y / 2, 0f);
        if (TopCenter.y > 3 + 0.01f || BottomCenter.y < -0.01)
        {
            return false;
        }

        if (isSnappedToSurface)
        {
            if (!CheckIfPlacedOnSurface()) return false;
        }
        
        // Collision Check
        var colliders = Physics.OverlapBox(center,colliderExtend * 0.98f, LastRotation);
        for (int i = 0; i < colliders.Length; i++)
        {
            var hitObject = colliders[i];

            var hitUnit = hitObject.GetComponentInParent<IPropUnit>();
            if(hitUnit == null || hitUnit.transform == _tempObject.transform) continue;
            
            if (hitObject.transform.parent.TryGetComponent(out Wall wall))
                return false;

            IPropUnit propUnit = hitObject.GetComponentInParent<IPropUnit>();
           
            if (propUnit != null)
            {
                if (propUnit.PlacementLayer == _placementItem.PlacementLayer)
                    return false;

                if (propUnit.PlacementLayer == ePlacementLayer.WallProp && _placementItem.PlacementLayer == ePlacementLayer.FloorProp)
                    return false;

                if (propUnit.PlacementLayer == ePlacementLayer.FloorProp && _placementItem.PlacementLayer == ePlacementLayer.WallProp)
                    return false;
            }
        }
        return true;
    }
    
    public void OnUpdate(ToolHelper TH)
    {
        FloorRotation(TH);
        FloorPositioning(TH);

        if (_placementItem.canPlaceOntoOtherObjects)
        {
            hitSurface = TH.InputSystem.GetHitTransformWithLayer(ToolHelper.SurfaceLayerID);

            if (hitSurface != null)
                SnapToSurfaceGrid(hitSurface.GetComponent<Collider>().bounds);

            isSnappedToSurface = hitSurface != null;
        }
         
        if(_tempObject != null)
        {
            // Apply To Object
            _tempObject.transform.position = LastPosition;
            // _tempObject.transform.position = LastPosition - (Camera.main.transform.forward * 0.02f);
            _tempObject.transform.rotation = LastRotation;
        }
        
        TH.MaterialColorChanger.SetMaterialsColorByValidity(_tempMeshRenderer, OnValidate(TH));
    }
    
    public void OnPlace(ToolHelper TH)
    {
        GameObject obj;
        
        obj = Object.Instantiate(_placementItem.Prefab, LastPosition, LastRotation);

        // Setting Parent Object
        if (isSnappedToSurface)
        {
             Transform snapHolder = hitSurface.transform.parent.Find("Snapped Object Holder");

            if (snapHolder == null)
            {
                GameObject newHolder = new GameObject();
                newHolder.transform.SetParent(hitSurface.parent);

                newHolder.name = "Snapped Object Holder";
                snapHolder = newHolder.transform;
            }
            
            obj.transform.SetParent(snapHolder);
        }
        else
        {
            obj.transform.SetParent(SceneGameObjectHandler.Instance.GetHolderByLayer(_placementItem.PlacementLayer));
        }

        IPropUnit unit;
        if (obj.TryGetComponent(out IPropUnit propUnit))
            unit = propUnit;
        else
            unit = obj.AddComponent<IPropUnit>();
        

        TH.LastPosition = LastPosition;
        TH.LastRotation = LastRotation;
    }

    public void OnStop(ToolHelper TH)
    {
        if (_tempObject != null)
        {
            Object.Destroy(_tempObject.gameObject);
        }
    }
    
    public void OnCancel(ToolHelper TH)
    {
    }
    
    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    ///

    private void CalculateBounds()
    {
        Colliders = _tempObject.transform.GetComponentsInChildren<Collider>();

        if (Colliders.Length == 0)
        {
            Debug.LogWarning("No Colliders found in the object.");
            return;
        }

        Bounds combinedBounds = Colliders[0].bounds;

        foreach (var col in Colliders)
        {
            if (col.gameObject.layer == ToolHelper.SurfaceLayerID) continue;
            
            combinedBounds.Encapsulate(col.bounds);
        }

        colliderSize = combinedBounds.size;
        colliderExtend = combinedBounds.extents;
    }

    private Vector3 GetCenterOfBounds()
    {
        Bounds combinedBounds = Colliders[0].bounds;

        foreach (var col in Colliders)
            combinedBounds.Encapsulate(col.bounds);

        return combinedBounds.center;
    }

    private void FloorPositioning(ToolHelper TH)
    {
        LastPosition = TH.SnapToGrid(TH.InputSystem.MousePosition, _placementItem.GridSizes);
        LastPosition.y = 0;

        if (Input.GetKey(KeyCode.LeftAlt)) // Free Placement
        {
            LastPosition = InputSystem.Instance.MousePosition;
        }
    }


    private void FloorRotation(ToolHelper TH)
    {
        if (Input.GetKeyDown(KeyCode.Q))
            LastRotation = TH.SnappyRotate(_tempObject.transform.rotation, 1);
        else if(Input.GetKeyDown(KeyCode.E))
            LastRotation = TH.SnappyRotate(_tempObject.transform.rotation, -1);
    }

    public void SnapToSurfaceGrid(Bounds surfaceBounds)
    {
        float gridSize = 0.25f;
        Vector3 surfaceCenter = surfaceBounds.center;
        // Get Surface Position From Input 
        // KInputSystem.MousePosition(BuildinSystemConstants.surfaceLayer);
        Vector3 localPosition = InputSystem.Instance.MousePosition;
        
        Debug.Log(localPosition);

        Vector3 offsetFromCenter = localPosition - surfaceCenter;

        Vector3 snappedOffset = new Vector3(
            Mathf.Round(offsetFromCenter.x / gridSize) * gridSize,
            Mathf.Round(offsetFromCenter.y / gridSize) * gridSize,
            Mathf.Round(offsetFromCenter.z / gridSize) * gridSize
        );

        Vector3 snappedPosition = surfaceCenter + snappedOffset;
        LastPosition = snappedPosition;

        if (Input.GetKey(KeyCode.LeftAlt)) // Free Placement
        {
            LastPosition = InputSystem.Instance.MousePosition;
        }
    }

    private bool CheckIfPlacedOnSurface()
    {
        foreach (var vector3 in GetRotatedFloorCorners(GetCenterOfBounds(), colliderSize, LastRotation))
        {
            Debug.Log(vector3);
            Ray ray = new Ray(vector3, Vector3.down);
            // TODO + Check If It Hits a surface of a different object.
            if (Physics.CheckSphere(vector3, 0.1f, 1 << ToolHelper.SurfaceLayerID))
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.green);
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.red);
                return false;
            }
        }
        return true;
    }
    
    private Vector3[] GetRotatedFloorCorners(Vector3 center, Vector3 size, Quaternion rotation)
    {
        // Calculate local positions of the floor corners relative to the center
        Vector3[] localCorners = new Vector3[]
        {
            new Vector3(-size.x / 2, -size.y / 2, -size.z / 2), // Bottom Front Left
            new Vector3(size.x / 2, -size.y / 2, -size.z / 2),  // Bottom Front Right
            new Vector3(-size.x / 2, -size.y / 2, size.z / 2),  // Bottom Back Left
            new Vector3(size.x / 2, -size.y / 2, size.z / 2)   // Bottom Back Right
        };

        // Apply rotation and translation to get world-space positions
        for (int i = 0; i < localCorners.Length; i++)
        {
            localCorners[i] = rotation * localCorners[i] + center;
        }

        return localCorners;
    }
}