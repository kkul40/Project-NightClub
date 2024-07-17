using System;
using System.Collections;
using System.Collections.Generic;
using Testing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] private Transform loadingScreen;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(CoSceneLoader(1));
    }

    public void LoadScene(int sceneID)
    {
        StartCoroutine(CoSceneLoader(sceneID));
    }

    private IEnumerator CoSceneLoader(int sceneID)
    {
        loadingScreen.gameObject.SetActive(false);
        yield break;
    }
}