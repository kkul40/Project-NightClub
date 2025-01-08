using System;
using System.Collections;
using DiscoSystem;
using TMPro;
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
        [SerializeField] private TextMeshProUGUI loadingText;
        
        
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
            LoadingText(loadingBar.fillAmount);
            
            var sceneToLoad = SceneManager.LoadSceneAsync(sceneID);

            while (!sceneToLoad.isDone)
            {
                var sceneProgress = Mathf.Clamp01(sceneToLoad.progress / 0.99f);
                loadingBar.fillAmount = sceneProgress;
                LoadingText(loadingBar.fillAmount);
                yield return null;
            }
            
            yield return new WaitUntil( () => sceneToLoad.isDone);
            loadingBar.fillAmount = 1;
            LoadingText(loadingBar.fillAmount);
            loadingScreen.SetActive(false);
            
            isSceneLoading = false;
        }

        private void LoadingText(float progress)
        {
            loadingText.text = $"Loading {Math.Round(progress * 100, 1)}%";
        }
    }
}