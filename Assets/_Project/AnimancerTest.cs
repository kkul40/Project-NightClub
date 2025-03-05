using System;
using System.Character.NPC;
using Animancer;
using ScriptableObjects;
using UnityEngine;

namespace DefaultNamespace
{
    public class AnimancerTest : MonoBehaviour
    {
        public NpcAnimationSo animations;
        public AnimancerComponent Component;

        private void Start()
        {
            Component.Play(animations.Walk[0]);
        }
    }
}