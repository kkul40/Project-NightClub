using System.Collections.Generic;
using DiscoSystem.Character.Bartender.Command;
using DiscoSystem.Character.NPC;
using Prop_Behaviours.Bar;
using PropBehaviours;
using UnityEngine;

namespace DiscoSystem.Character.Bartender
{
    public interface IBartender : IID
    {
        BarMediator BarMediator { get; }
        IPathFinder PathFinder { get; }
        bool IsBusy { get; set; }
        Transform mTransform { get; }
        Queue<IBartenderCommand> BartenderCommands { get; }
        IBartenderCommand CurrentCommand { get; }
        IAnimationController AnimationController { get; }
        void UpdateCommand();
        void AddCommand(IBartenderCommand command);
        void RemoveCommand();
    }
}