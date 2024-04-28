using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace StartMenu
{
    public class UI_CharacterCreationPage : UI_Page
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform cameraTransform;
        
        public override void Show(ePushAnimation ePushAnimation)
        {
            base.Show(ePushAnimation);

            cameraTransform.DOMoveX(playerTransform.position.x, 1);
        }

        public override void Hide(ePushAnimation ePushAnimation)
        {
            base.Hide(ePushAnimation);
            cameraTransform.DOMoveX(0, 0.5f);
        }
    }
}