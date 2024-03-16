using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;
    
    [SerializeField] private Camera mainCam;

    public LayerMask placableLayers;
    public PropSo propSo;
    public Transform tempPropTransform;
    private BoxCollider tempBoxCollider;
    private List<Material> tempMaterials;

    // Helper
    private Quaternion lastRotation;
    public Vector3 extractOffset;
    public Vector3 spawnPos;
    
    // Lerp Varriable
    public float duration = 0.5f;
    private float startTime = 0;
    private Vector3 snappedPos;
    private Vector3 lastPos;
    
    // Temp Varriable
    private Vector3 hitSnappedPos;
    private Transform hitTransform;
    private Quaternion hitRotation;
    private BoxCollider hitCollider;
    
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (propSo == null) return;
        
        var ray = mainCam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, propSo.GetPlacableLayer()))
        {
            var hitPoint = hitInfo.point;
            // Duvar ustunu gormezden gelsin diye
            if (hitPoint.y > 2)
                hitPoint.y = 2f;
            
            hitTransform = hitInfo.transform;
            hitSnappedPos = hitInfo.transform.position;
            hitRotation = hitInfo.transform.rotation;
            hitCollider = hitInfo.transform.gameObject.GetComponent<BoxCollider>();
            

            // Smooth Snapping
            if (tempPropTransform.position != hitSnappedPos)
            {
                if (startTime == 0)
                {
                    startTime = Time.time;
                }
                float fractionOfJourney = (Time.time - startTime) / duration; 
                var nextPos = Vector3.Lerp(tempPropTransform.position, hitSnappedPos, fractionOfJourney);
                tempPropTransform.position = nextPos;
            }
            else
            {
                startTime = 0;
            }
            
            if (CanBePlacable(hitTransform) && Input.GetMouseButtonDown(0))
            {
                PlaceObject(hitTransform, tempPropTransform.rotation);
                return;
            }
        }
        else
        {
            ChangeMaterialColor(Color.red);
        }

        if (propSo == null) return;
        
        switch (propSo.placableType)
        {
            case PlacableType.Floor:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    tempPropTransform.Rotate(Vector3.up, 90f);
                    lastRotation = tempPropTransform.rotation;
                }
                break;
            case PlacableType.Wall:
                tempPropTransform.rotation = hitRotation;
                break;
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelPlacing();
        }
    }
    
    private Vector3 SnapToGrid(Vector3 v, float snapValue)
    {
        float snapInv = 1/snapValue;

        Vector3 snapped = v;
        
        snapped.x = Mathf.RoundToInt(v.x * snapInv) / snapInv;
        snapped.y = Mathf.RoundToInt(v.y * snapInv) / snapInv;
        snapped.z = Mathf.RoundToInt(v.z * snapInv) / snapInv;

        return snapped;
    }

    private bool CanBePlacable(Transform checkPos)
    {
        var colliders = Physics.OverlapBox(checkPos.position, tempBoxCollider.bounds.extents - extractOffset / 2);
        
        foreach (var hit in colliders) // Floor objeleri duvara tirmaniyor
        {
            if (hit.gameObject == tempPropTransform.gameObject)
                continue;
        
            if (propSo.GetPlacableLayer() != 1 << hit.gameObject.layer)
            {
                ChangeMaterialColor(Color.yellow);
                return false;
            }
        }
        
        ChangeMaterialColor(Color.green);
        return true;
    }

    protected void ChangeMaterialColor(Color color)
    {
        if (tempMaterials.Count > 0)
        {
            if (tempMaterials[0].color == color) return;
        }
        else return;
        
        foreach (var material in tempMaterials)
        {
            material.color = color;
        }
    }


    private void PlaceObject(Transform placePos, Quaternion placeRotation)
    {
        Instantiate(propSo.Prefab, placePos.position, placeRotation);
        DestroyTempPrefab();
        propSo = null;
    }

    private void CancelPlacing()
    {
        DestroyTempPrefab();
        propSo = null;
    }

    public void StartBuilding(PropSo propSo)
    {
        CancelPlacing();
        this.propSo = propSo;
        tempPropTransform = Instantiate(this.propSo.Prefab, spawnPos, Quaternion.identity).transform;
        tempPropTransform.rotation = lastRotation;
        tempBoxCollider = tempPropTransform.GetComponent<BoxCollider>();
        tempMaterials = tempPropTransform.GetComponent<MeshRenderer>().materials.ToList();
    }

    private void DestroyTempPrefab()
    {
        if (tempPropTransform != null)
        {
            Destroy(tempPropTransform.gameObject);
            tempPropTransform = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (tempBoxCollider != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(tempBoxCollider.bounds.center, tempBoxCollider.bounds.size - extractOffset);
        }
    }
}
