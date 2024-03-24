using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Developer : MonoBehaviour
{
   public static Developer Instance;
   private void Awake()
   {
      Instance = this;
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
