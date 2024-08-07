using System;
using Data;
using UnityEngine;

namespace DefaultNamespace
{
    public class Initializer : MonoBehaviour
    {
        private void Awake()
        {
            SavingAndLoadingSystem.Instance.Initialize();
            DiscoData.Instance.Initialize();
            ActivitySystem.Instance.Initialize();
            MapGeneratorSystem.Instance.Initialize();
        }
    }
}