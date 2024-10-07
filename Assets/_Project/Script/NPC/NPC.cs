using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NPC_Stuff.Activities;
using PropBehaviours;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using WalkRandomActivity = NPC_Stuff.Activities.WalkRandomActivity;

namespace NPC_Stuff
{
    public interface IUnit
    {
        public eGenderType GenderType { get; }
        public IAnimationController AnimationController { get; }
        public IPathFinder PathFinder { get; }
    }
    
    
    [SelectionBase]
    public class NPC : MonoBehaviour, IUnit, IInteractable, IDoorOpener
    {
        /*
         * Play Animation {Idle, walk, sit, dance, argue, puke, drink}
         */

        public eGenderType GenderType { get; private set; }
        public IAnimationController AnimationController { get; private set; }
        public IPathFinder PathFinder { get; private set; }
        public ActivityHandler _activityHandler { get; private set; }

        public void Init(NpcAnimationSo npcAnimationSo)
        {
            PathFinder = new NpcPathFinder(transform);
            AnimationController = new NPCAnimationControl(GetComponentInChildren<Animator>(), npcAnimationSo, transform.GetChild(0));
            _activityHandler = new ActivityHandler(this);
        }

        private void Update()
        {
            _activityHandler.UpdateActivity();
        }

        public bool IsInteractable { get; }
        public eInteraction Interaction { get; } = eInteraction.Customer;

        public void OnFocus()
        {
        }

        public void OnOutFocus()
        {
        }

        public void OnClick()
        {
        }

        private List<Vector3> path = new();

        private void OnDestroy()
        {
            _activityHandler.ForceToEndActivity();
        }

        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying) return;
            if (PathFinder.FoundPath == null) return;
            
            path = PathFinder.FoundPath;
            if (path.Count > 1)
                for (var i = 1; i < path.Count; i++)
                    if (i == path.Count - 1)
                    {
                        Gizmos.color = Color.magenta;
                        Gizmos.DrawSphere(path[i], 0.15f);
                    }
                    else
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawLine(path[i - 1] + new Vector3(0, 0.25f, 0), path[i] + new Vector3(0, 0.25f, 0));
                    }
        }
    }


    public enum eAnimationType
    {
        NPC_Idle,
        NPC_Walk,
        NPC_Sit,
        NPC_Dance,
        NPC_Lean,
        Bartender_Idle,
        Bartender_Walk,
        Bartender_PrepareDrink,
        Bartender_CleanUpTable
    }
}