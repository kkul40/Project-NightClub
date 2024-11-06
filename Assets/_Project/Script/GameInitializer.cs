using System;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    private void Start()
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

        InitializeCommon(this);
    }

    private void InitializeMainMenu() // 0
    {
    }

    private void InitializeGame() // 1
    {
        DiscoData.Instance.Initialize(this);
        ActivitySystem.Instance.Initialize(this);
        MapGeneratorSystem.Instance.Initialize(this);
    }

    private void InitializeCommon(GameInitializer gameInitializer)
    {
        SavingAndLoadingSystem.Instance.Initialize(gameInitializer);
    }
}