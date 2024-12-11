using System;
using Data;
using DiscoSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    }

    private void InitializeMainMenu() // 0
    {
        SavingAndLoadingSystem.Instance.Initialize(this);
    }

    private void InitializeGame() // 1
    {
        DiscoData.Instance.Initialize(this);
        ActivitySystem.Instance.Initialize(this);
        MusicPlayer.Instance.Initialize(this);
        MapGeneratorSystem.Instance.Initialize(this);
        UpdatableHandler.Instance.Initialize(this);
        SavingAndLoadingSystem.Instance.Initialize(this);
    }
}