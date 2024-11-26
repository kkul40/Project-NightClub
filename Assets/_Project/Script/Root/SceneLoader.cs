using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Root
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        private bool isSceneLoading = false;

        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private Image loadingBar;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            loadingScreen.SetActive(false);
        }

        public void LoadScene(int sceneID)
        {
            if (isSceneLoading) return;
            
            StartCoroutine(CoSceneLoader(sceneID));
        }

        private IEnumerator CoSceneLoader(int sceneID)
        {
            isSceneLoading = true;

            loadingScreen.SetActive(true);
            loadingBar.fillAmount = 0;
            
            var sceneToLoad = SceneManager.LoadSceneAsync(sceneID);

            while (!sceneToLoad.isDone)
            {
                var sceneProgress = Mathf.Clamp01(sceneToLoad.progress / 0.99f);
                loadingBar.fillAmount = sceneProgress;
                yield return null;
            }
            
            yield return new WaitUntil( () => sceneToLoad.isDone);
            loadingBar.fillAmount = 1;
            loadingScreen.SetActive(false);
            
            isSceneLoading = false;
        }
    }
}