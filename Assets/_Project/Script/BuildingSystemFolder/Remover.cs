using ScriptableObjects;
using UnityEngine;

namespace BuildingSystemFolder
{
    public class Remover : MonoBehaviour, IBuild
    {
        [SerializeField] private BuildingSystem _buildingSystem;
        private Material defaultMaterial;
        private MeshRenderer selectedMeshRenderer;
        
        public void Setup<T>(T itemSo) where T : ItemSo
        {
        }

        public void BuildUpdate()
        {
            TryRemoving();
        }

        public void Exit()
        {
            if (selectedMeshRenderer != null)
            {
                selectedMeshRenderer.material = defaultMaterial;
                selectedMeshRenderer = null;
            }
            
            BuildingSystem.Instance.ResetPlacerAndRemover();
        }
        
        public void TryRemoving()
        {
            Vector3Int cellPos = BuildingSystem.Instance.GetMouseCellPosition();
            var placedObject = GetPlacedObjectFromTile(cellPos);

            SetMaterial(placedObject);

            if (InputSystem.Instance.LeftClickOnWorld)
            {
                if (placedObject != null)
                {
                    if (placedObject.transform.TryGetComponent(out IOccupieable occupieable))
                    {
                        if (occupieable.IsOccupied)
                        {
                            Debug.LogError("This Object Is Occupied Do Not Remove");
                        }
                        else
                        {
                            RemovePlacedObject(cellPos, placedObject);
                        }
                    }
                    else
                    {
                        RemovePlacedObject(cellPos, placedObject);
                    }
                }
            }
            
            if (InputSystem.Instance.Esc)
            {
                Exit();
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
                selectedMeshRenderer.material = _buildingSystem.yellowMaterial;
            }            
        }

        private GameObject GetPlacedObjectFromTile(Vector3Int cellPos)
        {
            if (GameData.Instance.placementDatas.TryGetValue(cellPos, out var placedObject))
            {
                return placedObject.Prefab;
            }
            return null;
        }

        protected virtual void RemovePlacedObject(Vector3Int cellPos, GameObject placedObject)
        {
            GameData.Instance.placementDatas.Remove(cellPos);
            Destroy(placedObject);
        }
    }
}