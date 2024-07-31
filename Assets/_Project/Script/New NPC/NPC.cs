using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using New_NPC.Activities;
using PropBehaviours;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using WalkRandomActivity = New_NPC.Activities.WalkRandomActivity;

namespace New_NPC
{
    [SelectionBase]
    public class NPC : MonoBehaviour, IInteractable
    {
        /*
         * Play Animation {Idle, walk, sit, dance, argue, puke, drink}
         */

        [SerializeField] private AudioClip _sinirClip;
        public IAnimationController animationController;
        public ActivityHandler _activityHandler { get; private set; }
        public IPathFinder PathFinder;

        public void Init(NpcAnimationSo npcAnimationSo)
        {
            PathFinder = new NpcPathFinder(transform);
            animationController = new NPCAnimationControl(GetComponentInChildren<Animator>(), npcAnimationSo,
                transform.GetChild(0));
            _activityHandler = new ActivityHandler(this);
        }

        private void Update()
        {
            _activityHandler.UpdateActivity();
        }

        public eInteraction Interaction { get; } = eInteraction.Customer;

        public void OnFocus()
        {
        }

        public void OnOutFocus()
        {
        }

        public void OnClick()
        {
            // TEST
            MusicSystem.Instance.PlaySoundEffect(_sinirClip);
        }

        private List<Vector3> path = new();

        private void OnDrawGizmosSelected()
        {
            if (path.Count > 1)
                for (var i = 1; i < path.Count; i++)
                    if (i == path.Count - 1)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawSphere(path[i], 0.5f);
                    }
                    else
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawLine(path[i - 1] + new Vector3(0, 0.5f, 0), path[i] + new Vector3(0, 0.5f, 0));
                    }
        }
    }


    public enum eAnimationType
    {
        NPC_Idle,
        NPC_Walk,
        NPC_Sit,
        NPC_Dance,
        Bartender_Idle,
        Bartender_Walk,
        Bartender_PrepareDrink
    }
}