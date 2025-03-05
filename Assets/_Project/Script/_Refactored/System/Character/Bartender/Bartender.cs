using System.Character.Bartender.Command;
using System.Character.NPC;
using System.Collections.Generic;
using Animancer;
using Data;
using Prop_Behaviours.Bar;
using PropBehaviours;
using UI.GamePages;
using UI.PopUp;
using UnityEngine;

namespace System.Character.Bartender
{
    [SelectionBase]
    public class Bartender : MonoBehaviour, IBartender, IInteractable, IDoorTrigger
    {
        public int InstanceID => GetInstanceID();
        public BarMediator BarMediator { get; private set; }
        public IPathFinder PathFinder { get; private set; }
        public bool IsBusy { get; set; }
        public Transform mTransform { get; private set; }
        public Queue<IBartenderCommand> BartenderCommands { get; } = new();
        public IBartenderCommand CurrentCommand { get; private set; }
        public IAnimationController AnimationController { get; private set; }

        private bool isStarted = false;

        public void Init(Animator animator, AnimancerComponent animancer, Transform armature)
        {
            PathFinder = new BartenderPathFinder(transform);
            mTransform = transform;
            BarMediator = BarMediator.Instance;
            AnimationController = new BartenderAnimationControl(animator, animancer, InitConfig.Instance.GetDefaultBartenderAnimation, armature);

            mGameobject = gameObject;
        }

        private void Update()
        {
            if(BartenderCommands.Count > 0)
                UpdateCommand();
        }

        public void UpdateCommand()
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

        public void AddCommand(IBartenderCommand command)
        {
            BartenderCommands.Enqueue(command);
        }

        public void RemoveCommand()
        {
            if (BartenderCommands.Count > 1)
            {
                BartenderCommands.Dequeue();
            }
        }

        public GameObject mGameobject { get; private set; }
        public bool IsInteractable { get; } = true;
        public bool IsAnimatable { get; } = false;
        public eInteraction Interaction { get; } = eInteraction.Customer;

        public void OnFocus()
        {
        }

        public void OnOutFocus()
        {
        }

        public void OnClick()
        {
            UIPageManager.Instance.RequestAPage(typeof(UIActionSelectionPage), this);
        }

        public bool TriggerDoor { get; set; } = true;
    }
}