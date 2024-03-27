using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Developer : Singleton<Developer>
{
   private void Awake()
   {
#if UNITY_ANDROID
      Application.targetFrameRate = 120;
#endif
   }

   public void NextScene()
   {
      SceneManager.LoadScene(1);
   }

   public void PreviousScene()
   {
      SceneManager.LoadScene(0);
   }
}