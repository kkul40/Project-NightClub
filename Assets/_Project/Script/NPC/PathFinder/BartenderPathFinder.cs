using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace NPC_Stuff
{
    public class BartenderPathFinder : IPathFinder
    {
        public bool HasReachedDestination { get; private set; }
        public Vector3 TargetPosition { get; private set; }
        public Transform mTransform { get; }

        public BartenderPathFinder(Transform mTransform)
        {
            this.mTransform = mTransform;
            TargetPosition = -Vector3.one;
        }

        public bool GoTargetDestination(Vector3 targetDestination, Action OnCompleteCallBack = null)
        {
            TargetPosition = targetDestination;
            HasReachedDestination = false;
            SetRotationToTarget(TargetPosition);
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
            while (Vector3.Distance(mTransform.position, TargetPosition) > 0.01f)
            {
                mTransform.position = Vector3.MoveTowards(mTransform.position, TargetPosition, Time.deltaTime * 1.5f);
                yield return null;
            }

            HasReachedDestination = true;
        }
    }
}