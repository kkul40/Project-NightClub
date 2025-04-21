using System.Character.NPC.Activity;
using System.Character.NPC.Activity.Activities;
using System.Collections.Generic;
using Animancer;
using Data;
using DiscoSystem;
using PropBehaviours;
using UnityEngine;

namespace System.Character.NPC
{
    [SelectionBase]
    public class NPC : MonoBehaviour, IInteractable, IDoorTrigger
    {
        /*
         * Play Animation {Idle, walk, sit, dance, argue, puke, drink}
         */
        public eGenderType GenderType { get; private set; }
        public IAnimationController AnimationController { get; private set; }
        public IPathFinder PathFinder { get; private set; }
        public ActivityHandler _activityHandler { get; private set; }

        public void Initialize(NewAnimationSO npcAnimationSo, Animator animator, AnimancerComponent animancerComponent, Transform armatureTransform, DiscoData discoData)
        {
            PathFinder = new NpcPathFinder(transform, PathUserType.Customer);
            AnimationController = new NPCAnimationControl(animator, animancerComponent, npcAnimationSo, armatureTransform);
            _activityHandler = new ActivityHandler(this, discoData);
            
            _activityHandler.StartNewActivity(new WalkToEnteranceActivity());
        }

        private void Update()
        {
            if (_activityHandler == null) return;
            
            _activityHandler.UpdateActivity();
        }

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
            CameraControl.Instance.FollowTarget(transform);
        }

        public void OnDeselect()
        {
            CameraControl.Instance.ResetTarget();
        }

        private List<Vector3> path = new();

        private void OnDestroy()
        {
            if(_activityHandler != null)
                _activityHandler.ForceToEndActivity();
            
            if(PathFinder != null)
                PathFinder.CancelDestination();
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

        public bool TriggerDoor { get; set; }
    }
}