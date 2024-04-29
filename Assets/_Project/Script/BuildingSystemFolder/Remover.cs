using ScriptableObjects;
using UnityEngine;

namespace BuildingSystemFolder
{
    public class Remover : IBuilder
    {
        [SerializeField] private BuildingSystem _buildingSystem => BuildingSystem.Instance;
        private Material defaultMaterial;
        private MeshRenderer selectedMeshRenderer;
        
        public void Setup(PlacablePropSo placablePropSo)
        {
            // Do Nothing...
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
            var placedObject = GameData.Instance.GetPlacedObject(cellPos);

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
                            RemovePlacedObject(cellPos);
                        }
                    }
                    else
                    {
                        RemovePlacedObject(cellPos);
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

        protected virtual void RemovePlacedObject(Vector3Int cellPos)
        {
            GameData.Instance.RemovePlacementData(cellPos);
        }
    }
}