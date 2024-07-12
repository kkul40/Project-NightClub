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
        private NPCAnimationControl _npcAnimationControl;
        private ActivityHandler _activityHandler;
        public NpcPathFinder pathFinder;
        public GameObject Prefab;
        
        public void Init(NpcAnimationSo npcAnimationSo)
        {
            // _navMeshAgent = GetComponent<NavMeshAgent>();
            pathFinder = new NpcPathFinder(transform);
            _npcAnimationControl = new NPCAnimationControl(GetComponentInChildren<Animator>(), npcAnimationSo, transform.GetChild(0));
            _activityHandler = new ActivityHandler(this);
            _activityHandler.StartActivity(new WalkRandomActivity());
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
        }

        private List<Vector3> path;

        public void SetAnimation(eNpcAnimation newAnimation)
        {
            _npcAnimationControl.PlayAnimation(newAnimation);
        }
        public NPCAnimationControl GetAnimationControl => _npcAnimationControl;
        
        // void OnDrawGizmosSelected()
        // {
        //     if (path.Count > 1)
        //     {
        //         for (int i = 1; i < path.Count; i++)
        //         {
        //             if (i == path.Count - 1)
        //             {
        //                 Gizmos.color = Color.green;
        //                 Gizmos.DrawSphere(path[i], 0.5f);
        //             }
        //             else
        //             {
        //                 Gizmos.color = Color.blue;
        //                 Gizmos.DrawLine(path[i - 1] + new Vector3(0,0.5f,0), path[i] + new Vector3(0,0.5f,0));
        //             }
        //         }
        //     }
        // }
    }


    public enum eNpcAnimation
    {
        Idle,
        Walk,
        Sit,
        Dance
    }
}