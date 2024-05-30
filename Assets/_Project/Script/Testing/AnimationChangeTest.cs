using System;
using UnityEngine;

namespace Testing
{
    public class AnimationChangeTest : MonoBehaviour
    {
        private Animator _animator;
        public AnimatorOverrideController _overrideController;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _animator.runtimeAnimatorController = _overrideController;
        }
        
    }
}