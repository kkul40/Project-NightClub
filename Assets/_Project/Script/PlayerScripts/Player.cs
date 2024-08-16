using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using New_NPC;
using PropBehaviours;
using UnityEngine;
using UnityEngine.AI;

namespace PlayerScripts
{
    public class Player : MonoBehaviour, IDoorOpener
    {
        private NpcPathFinder _pathFinder;
        private IAnimationController _animationController;

        private void Awake()
        {
            _pathFinder = new NpcPathFinder(transform);
            var custom = GetComponent<PlayerCustomization>();
            var animation = custom.playerGenderIndex == 0 ? InitConfig.Instance.GetDefaultBoyNpcAnimation : InitConfig.Instance.GetDefaultGirlNpcAnimation;
            _animationController = new NPCAnimationControl(GetComponentInChildren<Animator>(), animation, transform.GetChild(0));
            
            _animationController.PlayAnimation(eAnimationType.Bartender_Idle);
        }

        private void Update()
        {
            Debug.Log(InputSystem.Instance.GetMouseMapPosition());
            Debug.Log(InputSystem.Instance.GetMouseMapPosition().WorldPosToCellPos(eGridType.PathFinderGrid));
            
            if (InputSystem.Instance.RightClickOnWorld)
            {
                _pathFinder.GoTargetDestination(InputSystem.Instance.GetMouseMapPosition(), SetIdleAnimation);
                _animationController.PlayAnimation(eAnimationType.NPC_Walk);
            }
        }

        private void SetIdleAnimation()
        {
            _animationController.PlayAnimation(eAnimationType.NPC_Idle);
        }
    }
}