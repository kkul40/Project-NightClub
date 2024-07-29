﻿using Data;
using UnityEngine;

namespace New_NPC.Activities
{
    public class WalkRandomActivity : IActivity
    {
        private readonly float delay = 2;
        private float timer;

        public bool IsEnded { get; private set; }

        public bool CanStartActivity(ActivityNeedsData and)
        {
            return true;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            and.Npc.PathFinder.GoTargetDestination(GetRandomDestination(and));
            and.Npc.animationController.PlayAnimation(eAnimationType.NPC_Walk);
        }

        public void OnActivityUpdate(ActivityNeedsData and)
        {
            if (and.Npc.PathFinder.HasReachedDestination)
            {
                and.Npc.animationController.PlayAnimation(eAnimationType.NPC_Idle);
                timer += Time.deltaTime;
                if (timer > delay)
                {
                    timer = 0;
                    IsEnded = true;
                }
            }
        }

        public void OnActivityEnd(ActivityNeedsData and)
        {
        }

        public Vector3 GetRandomDestination(ActivityNeedsData and)
        {
            var loopCount = 0;

            var target = and.DiscoData.MapData.PathFinderNodes[
                Random.Range(0, DiscoData.Instance.MapData.CurrentMapSize.x),
                Random.Range(0, DiscoData.Instance.MapData.CurrentMapSize.y)];

            if (target == null) return and.Npc.transform.position;

            while (!target.IsWalkable)
            {
                target = and.DiscoData.MapData.PathFinderNodes[
                    Random.Range(0, DiscoData.Instance.MapData.CurrentMapSize.x),
                    Random.Range(0, DiscoData.Instance.MapData.CurrentMapSize.y)];

                loopCount++;
                if (loopCount > 100)
                {
                    IsEnded = true;
                    break;
                }
            }

            return target.WorldPos;
        }
    }
}