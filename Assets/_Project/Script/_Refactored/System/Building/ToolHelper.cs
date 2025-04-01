using System.Building.Controller;
using Data;
using Disco_ScriptableObject;
using UnityEngine;

namespace System.Building
{
    public enum PurchaseTypes
    {
        None,
        Buy,
        Free,
        Unique,
    }
    
    public class ToolHelper
    {
        // Constant Variables
        public const int FloorLayerID = 7;
        public const int WallLayerID = 8;
        public const int GroundLayerID = 9;
        public const int SurfaceLayerID = 11;
    
        public const float HitCollisionLeniency = 0.98f;
    
        // Instance Variables
        public BuildingController BuildingController;
        public InputSystem InputSystem;
        public DiscoData DiscoData;
        public MaterialColorChanger MaterialColorChanger;
        public FXCreatorSystem FXCreatorSystem;
    
        // Static Variables
        public StoreItemSO SelectedStoreItem;
        public PurchaseTypes PurchaseMode;
        // Keep In Position
        public bool KeepInStartPosition;
        
        // Relocate Variables
        // public bool IsRelocating;
        // public IPropUnit SelectedPropItem;

    
        // Dynamic Variables
        private Collider[] Colliders;
        public Vector3 colliderSize;
        public Vector3 colliderExtend;
        public Quaternion LastRotation;
        public Vector3 LastPosition;
        public Vector3 StartMousePos;

    
        public ToolHelper(BuildingController controller, InputSystem inputSystem, DiscoData discoData,
            MaterialColorChanger materialColorChanger, FXCreatorSystem fxCreatorSystem)
        {
            BuildingController = controller;
            InputSystem = inputSystem;
            DiscoData = discoData;
            MaterialColorChanger = materialColorChanger;
            FXCreatorSystem = fxCreatorSystem;
        }

        #region Position Rotation Functions
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
    
        public void SnapToSurfaceGrid(ToolHelper TH, Transform hitSurface)
        {
            Bounds surfaceBounds = hitSurface.GetComponent<Collider>().bounds;
        
            float gridSize = 0.25f;
            Vector3 surfaceCenter = surfaceBounds.center;
            Vector3 localPosition = InputSystem.Instance.GetMousePositionOnLayer(ToolHelper.SurfaceLayerID);
        
            Debug.Log(localPosition);

            Vector3 offsetFromCenter = localPosition - surfaceCenter;

            Vector3 snappedOffset = new Vector3(
                Mathf.Round(offsetFromCenter.x / gridSize) * gridSize,
                Mathf.Round(offsetFromCenter.y / gridSize) * gridSize,
                Mathf.Round(offsetFromCenter.z / gridSize) * gridSize
            );

            Vector3 snappedPosition = surfaceCenter + snappedOffset;
            TH.LastPosition = snappedPosition;

            if (InputSystem.FreePlacementKey) // Free Placement
            {
                TH.LastPosition = InputSystem.Instance.MousePosition;
            }
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
    
        #endregion

        #region Helper Functions

        public bool IsPositioningLocked()
        {
            if (KeepInStartPosition)
            {
                if (Vector3.Distance(StartMousePos, InputSystem.MousePosition) < 0.2f)
                    return true;
                
                KeepInStartPosition = false;
            }

            return false;
        }

        public WallData GetClosestWall()
        {
            Vector3 mousePos = InputSystem.GetMousePositionOnLayer(ToolHelper.GroundLayerID);
            float maxDistance = float.MaxValue;

            WallData output = null;

            foreach (var wall in DiscoData.Instance.MapData.WallDatas)
            {
                float dis = Vector3.Distance(mousePos, wall.assignedWall.transform.position);
                if (dis < maxDistance)
                {
                    maxDistance = dis;
                    output = wall;
                }
            }

            return output;
        }

        #endregion

        #region Collider Functions

        public void CalculateBounds(Collider[] colliders)
        {
            Colliders = colliders;
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
    
        public Vector3 GetCenterOfBounds()
        {
            Bounds combinedBounds = Colliders[0].bounds;

            foreach (var col in Colliders)
                combinedBounds.Encapsulate(col.bounds);

            return combinedBounds.center;
        }
    
        public Vector3[] GetRotatedFloorCorners(Quaternion rotation)
        {
            Vector3 size = colliderSize * HitCollisionLeniency;
            Vector3[] localCorners = new Vector3[]
            {
                new Vector3(-size.x / 2, -size.y / 2, -size.z / 2), // Bottom Front Left
                new Vector3(size.x / 2, -size.y / 2, -size.z / 2),  // Bottom Front Right
                new Vector3(-size.x / 2, -size.y / 2, size.z / 2),  // Bottom Back Left
                new Vector3(size.x / 2, -size.y / 2, size.z / 2)   // Bottom Back Right
            };

            for (int i = 0; i < localCorners.Length; i++)
            {
                localCorners[i] = rotation * localCorners[i] + GetCenterOfBounds();
            }

            return localCorners;
        }
    
        #endregion

        #region  Validation Functions

        public bool MouseInBoundryCheck()
        {
            Vector3 position = InputSystem.GetMousePositionOnLayer(GroundLayerID);
            if (position.x < 0 || position.z < 0) return false;
            if (position.x > DiscoData.MapData.CurrentMapSize.x || position.z > DiscoData.MapData.CurrentMapSize.y) return false;
        
            return true;
        }

        public bool HeightCheck()
        {
            Vector3 center = GetCenterOfBounds();
            Vector3 size = colliderSize * HitCollisionLeniency;
            Vector3 TopCenter = center + new Vector3(0f, size.y / 2, 0f);
            Vector3 BottomCenter = center - new Vector3(0f, size.y / 2, 0f);
            // Height Boundry
            if (TopCenter.y > 3 + 0.01f || BottomCenter.y < -0.01)
                return false;

            return true;
        }
    
        public bool MapBoundryCheck()
        {
            foreach (var vector in GetRotatedFloorCorners(LastRotation))
            {
                if (vector.x < 0 || vector.z < 0) return false;
                if (vector.x > DiscoData.MapData.CurrentMapSize.x || vector.z > DiscoData.MapData.CurrentMapSize.y) return false;
            }
            return true;
        }

        #endregion
    }
}