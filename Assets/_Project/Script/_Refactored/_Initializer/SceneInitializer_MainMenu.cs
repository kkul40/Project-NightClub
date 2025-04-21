using SaveAndLoad;
using UnityEngine;

namespace _Initializer
{
    public class SceneInitializer_MainMenu : MonoBehaviour
    {
        [SerializeField] private SavingAndLoadingSystem _savingAndLoadingSystem;
        
        private void Start()
        {
            _savingAndLoadingSystem.Initialize();
            
            Debug.developerConsoleEnabled = true;
            Debug.developerConsoleVisible = true;
            Debug.LogError("Console Is Active");
        }
    }
}