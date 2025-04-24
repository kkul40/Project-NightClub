using System;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using ExtensionMethods;
using UnityEngine;

namespace DiscoSystem.Character
{
    public enum PathUserType
    {
        Player,
        Employee,
        Customer,
    }
    public interface IPathFinder
    {
        public Transform FinderTransform { get; }
        public Vector3 TargetPosition { get; }
        public Vector3 CurrentPosition => FinderTransform.position.CellCenterPosition(eGridType.PathFinderGrid);
        PathUserType PathUserType { get; }
        public bool HasReachedDestination { get; }
        public List<Vector3> GetWayPoints { get; }
        public PathFinderNode GetDestinationNode();
        public bool IsPathAvaliable(Vector3 destination);
        public bool GoToDestination(Vector3 destination, Action onCompleteCallBack = null);
        public bool GoToRandomDestination();
        public void Cancel();
        public void ErrorHelp(Action onCompleteCallBack = null);
        void SetPositioning(Quaternion? rotation = null, Vector3? position = null, float? duration = null)
        {
            if (rotation != null)
                FinderTransform.DORotate(rotation.Value.eulerAngles, duration ?? 0.5f);

            if (position != null)
                FinderTransform.DOMove((Vector3)position, duration ?? 0.5f);
        }
    }
}