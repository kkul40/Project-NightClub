using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using New_NPC;
using UnityEngine;

namespace PropBehaviours
{
    public interface IBartender : IID
    {
        BarMediator BarMediator { get; }
        IPathFinder PathFinder { get; }
        bool IsBusy { get; }
        Stack<IBartenderCommand> BartenderCommands { get; }
        IBartenderCommand CurrentCommand { get; }
        void UpdateCommand();
        void AddCommand();
        void RemoveCommand();
    }
    
    public class TestBartender : MonoBehaviour, IBartender
    {
        public BarMediator BarMediator { get; } = BarMediator.Instance;
        public IPathFinder PathFinder { get; } = new BartenderPathFinder();
        public bool IsBusy { get; } = false;
        public Stack<IBartenderCommand> BartenderCommands { get; } = new Stack<IBartenderCommand>();
        public IBartenderCommand CurrentCommand { get; private set; }
    
        private void Start()
        {
            // CurrentCommand = new WallToEntranceCommand();
        }
    
        public void UpdateCommand()
        {
            if (!BartenderCommands.Peek().HasFinish)
            {
                BartenderCommands.Peek().UpdateCommand(BarMediator);
                return;
            }
    
            if (BartenderCommands.Count > 0)
                BartenderCommands.Pop();
        }
    
        public void AddCommand()
        {
            throw new NotImplementedException();
        }
    
        public void RemoveCommand()
        {
            throw new NotImplementedException();
        }
    }

    public class NewBartender : MonoBehaviour, IInteractable
    {
        private BarMediator _barMediator;

        private List<IBartenderCommand> bartenderCommands;

        public IAnimationController AnimationController;

        public eInteraction Interaction { get; }

        public bool IsBusy = false;

        private void Start()
        {
            _barMediator = BarMediator.Instance;
            AnimationController = new BartenderAnimationControl(GetComponentInChildren<Animator>(), InitConfig.Instance.GetDefaultBartenderAnimation, transform.GetChild(0));
            bartenderCommands = new List<IBartenderCommand>();
        }

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