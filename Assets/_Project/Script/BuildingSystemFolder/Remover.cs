using UnityEngine;

namespace _Project.Script.NewSystem
{
    public class Remover : MonoBehaviour, IRemover
    {
        [SerializeField] private InputSystem inputSystem;
        [SerializeField] private Grid grid;
        [SerializeField] private Material yellowRemover;

        private Material defaultMaterial;
        private MeshRenderer selectedMeshRenderer;
        
        
        public void StartRemoving()
        {
            
        }
        
        
        public void TryRemoving()
        {
            Vector3Int cellPos = BuildingSystem.Instance.GetMouseCellPosition(inputSystem, grid);
            var placedObject = GetPlacedObjectFromTile(cellPos);

            SetMaterial(placedObject);

            if (Input.GetMouseButtonDown(0))
            {
                if (placedObject != null && placedObject.transform.TryGetComponent(out IOccupieable occupieable))
                {
                    if (occupieable.IsOccupied)
                    {
                        Debug.LogError("This Object Is Occupied Do Not Remove");
                        return;
                    }
                    RemovePlacedObject(cellPos, placedObject);
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopRemoving();
            }
        }

        protected virtual void SetMaterial(GameObject placedObject)
        {
            // Hiraeth Yardimlariyla
            if (selectedMeshRenderer != null)
            {
                selectedMeshRenderer.material = defaultMaterial;
                selectedMeshRenderer = null;
            }

            if (placedObject != null)
            {
                selectedMeshRenderer = placedObject.GetComponent<MeshRenderer>();
                defaultMaterial = selectedMeshRenderer.material;
                selectedMeshRenderer.material = yellowRemover;
            }            
        }

        private GameObject GetPlacedObjectFromTile(Vector3Int cellPos)
        {
            if (GameData.Instance.placedObjects.TryGetValue(cellPos, out var placedObject))
            {
                return placedObject.Prefab;
            }
            return null;
        }

        protected virtual void RemovePlacedObject(Vector3Int cellPos, GameObject placedObject)
        {
            GameData.Instance.placedObjects.Remove(cellPos);
            Destroy(placedObject);
            StopRemoving();
        }
        
        public void StopRemoving()
        {
            BuildingSystem.Instance.ResetPlacerAndRemover();
        }
    }
}