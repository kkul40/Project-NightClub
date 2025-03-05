using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace System.Character
{
    public interface IPathFinder
    {
        Transform mTransform { get; }
        bool HasReachedDestination { get; }
        Vector3 TargetPosition { get; }
        Vector3 CurrentPoistion => mTransform.position;
        bool CheckIfPathAvaliable(Vector3 targetDestination);
        bool GoTargetDestination(Vector3 targetDestination, Action OnCompleteCallBack = null);
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