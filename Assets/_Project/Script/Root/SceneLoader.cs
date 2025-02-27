using System;
using System.Collections;
using DG.Tweening;
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
        [SerializeField] private CanvasGroup canvasGroup;
        
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
            canvasGroup.alpha = 0;
            
            canvasGroup.DOFade(1, 0.2f).SetEase(Ease.OutCubic);
            yield return new WaitUntil(() => canvasGroup.alpha == 1);
            
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
            yield return new WaitForSeconds(0.2f);
            
            canvasGroup.DOFade(0, 0.2f).SetEase(Ease.OutCubic);
            yield return new WaitUntil(() => canvasGroup.alpha == 0);
            
            loadingBar.fillAmount = 1;
            LoadingText(loadingBar.fillAmount);
            loadingScreen.SetActive(false);
            canvasGroup.alpha = 0;
            
            isSceneLoading = false;
        }

        private void LoadingText(float progress)
        {
            loadingText.text = $"Loading {Math.Round(progress * 100, 1)}%";
        }
    }
}