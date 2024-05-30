using System;
using Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NPC.Activities
{
    public class WalkRandomActivity : Activity
    {
        private readonly float delay = 2;
        private float timer;
        public override bool isEnded { get; protected set; }
        public override bool isCanceled { get; protected set; }

        public override void StartActivity(NPC npc)
        {
            if (isCanceled) return;

            npc.SetNewDestination(GetRandomDestination());
            npc.SetAnimation(eNpcAnimation.Walk);
        }

        public override void UpdateActivity(NPC npc)
        {
            if (isCanceled) return;

            if (!npc._navMeshAgent.hasPath)
            {
                npc.SetAnimation(eNpcAnimation.Idle);
                timer += Time.deltaTime;
                if (timer > delay)
                {
                    npc.SetNewDestination(GetRandomDestination());
                    npc.SetAnimation(eNpcAnimation.Walk);
                    timer = 0;
                    isEnded = true;
                }
            }
        }

        public override void EndActivity(NPC npc)
        {
            if (isCanceled) return;
        }

        public Vector3 GetRandomDestination()
        {
            var loopCount = 0;

            var target = GameData.Instance.FloorMap[Random.Range(0, GameData.Instance.FloorMap.Count)];
            while (GameData.Instance.placementDataHandler.ContainsKey(GridHandler.Instance.GetWorldToCell(target),
                       ePlacementLayer.Floor))
            {
                target = GameData.Instance.FloorMap[Random.Range(0, GameData.Instance.FloorMap.Count)];
                loopCount++;
                if (loopCount >= 99)
                {
                    isCanceled = true;
                    break;
                }
            }

            return target;
        }
    }
}