using System.Collections.Generic;
using NPC;
using NPC.PathFinder;
using UnityEngine;

namespace PropBehaviours
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