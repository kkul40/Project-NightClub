using System;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.MainMenu
{
    public class ContinueButtonBase : UIButtonBase
    {
        protected override void Start()
        {
            button.interactable = SavingAndLoadingSystem.Instance.HasBeenSavedBefore();
        }

        public override void OnClick()
        {
            SceneManager.LoadScene(1);
        }
    }
}