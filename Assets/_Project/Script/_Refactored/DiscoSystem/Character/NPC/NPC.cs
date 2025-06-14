using System.Collections.Generic;
using _Initializer;
using Animancer;
using DiscoSystem.CameraSystem;
using DiscoSystem.Character.NPC.Activity;
using DiscoSystem.NewPathFinder;
using PropBehaviours;
using UnityEngine;

namespace DiscoSystem.Character.NPC
{
    [SelectionBase]
    public class NPC : MonoBehaviour, IInteractable, IDoorTrigger
    {
        /*
         * Play Animation {Idle, walk, sit, dance, argue, puke, drink}
         */
        public eGenderType GenderType { get; private set; }
        public IAnimationController AnimationController { get; private set; }
        // public IPathFinder PathFinder { get; private set; }
        
        public PathFindingAgent PathAgent { get; private set; }
        public ActivityHandler ActivityHandler { get; private set; }
        
#if UNITY_EDITOR
        public string debugState;
#endif

        public void Initialize(NewAnimationSO npcAnimationSo, Animator animator, AnimancerComponent animancerComponent, Transform armatureTransform)
        {
            PathAgent = new PathFindingAgent(transform, PathUserType.Customer);
            AnimationController = new NPCAnimationControl(animator, animancerComponent, npcAnimationSo, armatureTransform);
            ActivityHandler = new ActivityHandler(this);
        }

        // Carried To NPCSystem
        // private void Update()
        // {
        //     if (ActivityHandler == null) return;
        //     
        //     ActivityHandler.UpdateActivity();
        //     debugState = ActivityHandler.GetCurrentActivity.GetType().Name;
        // }

        public GameObject mGameobject { get; }
        public bool IsInteractable { get; } = true;
        public bool hasInteractionAnimation { get; } = false;
        public eInteraction Interaction { get; private set; } = eInteraction.Customer;

        public void OnFocus()
        {
            // TODO Show some attributes or details for npc.
        }

        public void OnOutFocus()
        {
        }

        public void OnClick()
        {
            ServiceLocator.Get<CameraControl>().FollowTarget(transform);
        }

        public void OnDeselect()
        {
            ServiceLocator.Get<CameraControl>().ResetTarget();
        }

        private List<Vector3> path = new();

        private void OnDestroy()
        {
            if (ActivityHandler != null)
            {
                ActivityHandler.ForceToEndActivity();
                ActivityHandler.isDead = true;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying) return;
            
            path = PathAgent.GetPath();
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

        public bool TriggerDoor { get; set; }
    }
}