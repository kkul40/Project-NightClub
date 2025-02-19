using DiscoSystem;
using UnityEngine;

namespace DefaultNamespace._Refactored
{
    public class SceneInitializer_Game : MonoBehaviour
    {
        [SerializeField] private BuildingSystem _buildingSystem;
        [SerializeField] private GridSystem _gridSystem;
        private void Awake()
        {
            if (_buildingSystem == null)
            {
                Debug.LogError("Building System Is Null");
                return;
            }
            
            if (_gridSystem== null)
            {
                Debug.LogError("Building System Is Null");
                return;
            }
            
            
            
            
            
            
            
            
            
            
            
            _gridSystem.Initialize();
            _buildingSystem.Initialize();
        }
    }
}