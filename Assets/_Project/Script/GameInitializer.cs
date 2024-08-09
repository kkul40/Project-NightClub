using System;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class GameInitializer : MonoBehaviour
    {
        private void Awake()
        {
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 0:
                    InitializeMainMenu();
                    break;
                case 1:
                    InitializeGame();
                    break;
            }
            
            InitializeCommon();
        }

        private void InitializeMainMenu() // 0
        {
        }

        private void InitializeGame() // 1
        {
            DiscoData.Instance.Initialize();
            ActivitySystem.Instance.Initialize();
            MapGeneratorSystem.Instance.Initialize();
        }

        private void InitializeCommon()
        {
            SavingAndLoadingSystem.Instance.Initialize();
        }
    }
}