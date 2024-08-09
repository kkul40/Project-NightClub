using System;
using System.Collections.Generic;
using Data;
using New_NPC;
using UnityEngine;

namespace PropBehaviours
{
    public class TestBartender : MonoBehaviour, IBartender, IInteractable
    {
        public int InstanceID => GetInstanceID();
        public BarMediator BarMediator { get; private set; }
        public IPathFinder PathFinder { get; private set; }
        public bool IsBusy { get; set; }
        public Transform mTransform { get; private set; }
        public Queue<IBartenderCommand> BartenderCommands { get; } = new Queue<IBartenderCommand>();
        public IBartenderCommand CurrentCommand { get; private set; }
        public IAnimationController AnimationController { get; private set; }

        private bool isStarted = false;

        private void Start()
        {
            PathFinder = new BartenderPathFinder(transform);
            mTransform = this.transform;
            BarMediator = new BarMediator();
            AnimationController = new BartenderAnimationControl(GetComponentInChildren<Animator>(), InitConfig.Instance.GetDefaultBartenderAnimation, transform.GetChild(0));
        }

        private void Update()
        {
            UpdateCommand();
        }

        public void UpdateCommand()
        {
            if (BartenderCommands.Count != 0)
            {
                if (!isStarted)
                {
                    BartenderCommands.Peek().SetThingsBeforeStart();
                    isStarted = true;
                }
                
                if (!BartenderCommands.Peek().HasFinish)
                {
                    BartenderCommands.Peek().UpdateCommand(BarMediator);
                }
                else
                {
                    BartenderCommands.Dequeue();
                    isStarted = false;
                }
            }
        }
    
        public void AddCommand(IBartenderCommand command)
        {
            BartenderCommands.Enqueue(command);
        }
    
        public void RemoveCommand()
        {
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
    }
}