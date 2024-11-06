using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Root
{
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
}