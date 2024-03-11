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
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, placableLayers))
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
                PlaceObject(hitTransform.position, tempPropTransform.rotation);
                return;
            }
        }
        else
        {
            // Out of Map Logic
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
        Debug.Log(checkPos.position);
        
        
        var colliders = Physics.OverlapBox(checkPos.position, tempBoxCollider.bounds.extents - extractOffset / 2);
        
        foreach (var hit in colliders) // Floor objeleri duvara tirmaniyor
        {
            if (hit.gameObject == tempPropTransform.gameObject)
                continue;
        
            if (propSo.GetPlacableLayer != 1 << hit.gameObject.layer)
            {
                Debug.LogWarning("Nooo");
                return false;
            }
        }
        
        Debug.LogWarning("Yess");
        return true;
    }
    
    
    private void PlaceObject(Vector3 placePos, Quaternion placeRotation)
    {
        Instantiate(propSo.Prefab, placePos, placeRotation);
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
