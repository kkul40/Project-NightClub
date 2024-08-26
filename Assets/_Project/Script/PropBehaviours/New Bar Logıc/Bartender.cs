using System;
using System.Collections.Generic;
using Data;
using New_NPC;
using UnityEngine;

namespace PropBehaviours.New_Bar_Logıc
{
    public class Bartender : MonoBehaviour
    {
        public IPathFinder PathFinder;
        public IAnimationController AnimationController;

        private void Start()
        {
            PathFinder = new BartenderPathFinder(transform);
            
            AnimationController = new BartenderAnimationControl(GetComponentInChildren<Animator>(),
                InitConfig.Instance.GetDefaultBartenderAnimation, transform.GetChild(0));
        }
    }
}