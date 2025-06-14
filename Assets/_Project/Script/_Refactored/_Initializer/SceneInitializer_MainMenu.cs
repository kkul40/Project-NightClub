using UnityEngine;

namespace _Initializer
{
    public class SceneInitializer_MainMenu : MonoBehaviour
    {
        private void Awake()
        {
            ServiceLocator.Clear();
        }
    }

    
}