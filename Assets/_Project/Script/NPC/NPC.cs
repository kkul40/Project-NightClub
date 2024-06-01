using System;
using DG.Tweening;
using New_NPC;
using New_NPC.Activities;
using NPC.Activities;
using PropBehaviours;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;
using WalkRandomActivity = New_NPC.Activities.WalkRandomActivity;

namespace NPC
{
    [SelectionBase]
    public class NPC : MonoBehaviour, IInteractable
    {
        /*
         * Play Animation {Idle, walk, sit, dance, argue, puke, drink}
         */
        private NPCAnimationControl _npcAnimationControl;
        private ActivityHandler _activityHandler;
        public NavMeshAgent _navMeshAgent { get; private set; }

        public void Init(NpcAnimationSo npcAnimationSo)
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
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

        public void SetNewDestination(Vector3 targetPos)
        {
            _navMeshAgent.SetDestination(targetPos);
        }

        public void SetRotation(Quaternion targetRotation)
        {
            transform.DORotate(targetRotation.eulerAngles, 0.5f);
        }

        public void SetAnimation(eNpcAnimation newAnimation)
        {
            _npcAnimationControl.PlayAnimation(newAnimation);
        }

        public NPCAnimationControl GetAnimationControl => _npcAnimationControl;
    }


    public enum eNpcAnimation
    {
        Idle,
        Walk,
        Sit,
        Dance
    }
}