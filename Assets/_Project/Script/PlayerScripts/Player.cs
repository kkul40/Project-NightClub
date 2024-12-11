using System;
using Data;
using DiscoSystem;
using NPCBehaviour;
using NPCBehaviour.PathFinder;
using UI.GamePages;
using UnityEngine;

namespace PlayerScripts
{
    public class Player : MonoBehaviour, IDoorTrigger
    {
        private IAnimationController _animationController;
        private NpcPathFinder _pathFinder;

        private void Awake()
        {
            _pathFinder = new NpcPathFinder(transform);
            var custom = GetComponent<PlayerCustomization>();
            var animation = custom.playerGenderIndex == 0
                ? InitConfig.Instance.GetDefaultBoyNpcAnimation
                : InitConfig.Instance.GetDefaultGirlNpcAnimation;
            _animationController =
                new NPCAnimationControl(GetComponentInChildren<Animator>(), animation, transform.GetChild(0));

            _animationController.PlayAnimation(eAnimationType.Bartender_Idle);
        }

        private void Update()
        {
            // Debug.Log(InputSystem.Instance.GetMouseMapPosition());
            // Debug.Log(InputSystem.Instance.GetMouseMapPosition().WorldPosToCellPos(eGridType.PathFinderGrid));

            if (InputSystem.Instance.RightClickOnWorld && !UIPageManager.Instance.IsAnyUIToggled())
            {
                _pathFinder.GoTargetDestination(InputSystem.Instance.MousePosition, SetIdleAnimation);
                _animationController.PlayAnimation(eAnimationType.NPC_Walk);
            }
        }

        private void SetIdleAnimation()
        {
            _animationController.PlayAnimation(eAnimationType.NPC_Idle);
        }

        public bool TriggerDoor { get; set; } = true;
    }
}