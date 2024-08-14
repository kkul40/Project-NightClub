using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace New_NPC
{
    public interface IPathFinder
    {
        Transform mTransform { get; }
        bool HasReachedDestination { get; }
        bool GoTargetDestination(Vector3 targetDestination, bool checkNodes = true, Action OnCompleteCallBack = null);
        void CancelDestination();

        List<Vector3> FoundPath { get; }

        void SetPositioning(Quaternion? newRotation = null, Vector3? newPosition = null, float? duration = null)
        {
            if (newRotation != null)
                mTransform.DORotate(newRotation.Value.eulerAngles, duration ?? 0.5f);

            if (newPosition != null)
                mTransform.DOMove((Vector3)newPosition, duration ?? 0.5f);
        }
    }
}