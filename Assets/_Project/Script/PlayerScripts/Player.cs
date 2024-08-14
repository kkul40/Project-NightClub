using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using New_NPC;
using PropBehaviours;
using UnityEngine;
using UnityEngine.AI;

namespace PlayerScripts
{
    public class Player : MonoBehaviour, IDoorOpener
    {
        private NpcPathFinder _pathFinder;

        private void Awake()
        {
            _pathFinder = new NpcPathFinder(transform);
        }

        private void Update()
        {
            if (InputSystem.Instance.RightClickOnWorld)
            {
                var lastHitFloor = InputSystem.Instance.GetHitTransformWithLayer(ConstantVariables.FloorLayerID);
                if (lastHitFloor != null)
                    _pathFinder.GoTargetDestination(GridHandler.Instance.GetWorldToCell(lastHitFloor.position, eGridType.PathFinderGrid));
            }
        }
    }
}