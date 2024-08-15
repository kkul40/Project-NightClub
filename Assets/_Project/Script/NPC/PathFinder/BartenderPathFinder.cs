using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace New_NPC
{
    public class BartenderPathFinder : IPathFinder
    {
        public bool HasReachedDestination { get; private set; }
        private Vector3 target;
        public Transform mTransform { get; }

        public BartenderPathFinder(Transform mTransform)
        {
            this.mTransform = mTransform;
        }

        public bool GoTargetDestination(Vector3 targetDestination, Action OnCompleteCallBack = null)
        {
            target = targetDestination;
            HasReachedDestination = false;
            SetRotationToTarget(target);
            DOTween.instance.StartCoroutine(CoWalkPosition());
            return true;
        }

        public void CancelDestination()
        {
            DOTween.instance.StopAllCoroutines();
        }

        public List<Vector3> FoundPath { get; }

        private void SetRotationToTarget(Vector3 lookatTarget)
        {
            var direction = lookatTarget - mTransform.position;
            direction.Normalize();
            var lookRotation = Quaternion.LookRotation(direction);
            mTransform.DORotate(lookRotation.eulerAngles, 0.5f);
        }

        private IEnumerator CoWalkPosition()
        {
            while (Vector3.Distance(mTransform.position, target) > 0.01f)
            {
                mTransform.position = Vector3.MoveTowards(mTransform.position, target, Time.deltaTime * 1.5f);
                yield return null;
            }

            HasReachedDestination = true;
        }
    }
}