using System;
using Data;
using UnityEngine;

namespace DefaultNamespace
{
    public class Initializer : MonoBehaviour
    {
        private void Awake()
        {
            DiscoData.Instance.Initialize();
            MapGeneratorSystem.Instance.Initialize();
            ActivitySystem.Instance.Initialize();
            
            
            // Saving
            SavingAndLoadingSystem.Instance.Initialize();
        }
    }
}