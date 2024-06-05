using System;
using Data;
using New_NPC;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Activities
{
    public class WalkRandomActivity : Activity
    {
        private readonly float delay = 2;
        private float timer;
        public override bool isEnded { get; protected set; }
        public override bool isCanceled { get; protected set; }

        public override void StartActivity(New_NPC.NPC npc)
        {
            if (isCanceled) return;

            npc.SetNewDestination(GetRandomDestination());
            npc.SetAnimation(eNpcAnimation.Walk);
        }

        public override void UpdateActivity(New_NPC.NPC npc)
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

        public override void EndActivity(New_NPC.NPC npc)
        {
            if (isCanceled) return;
        }

        public Vector3 GetRandomDestination()
        {
            var loopCount = 0;

            var target = DiscoData.Instance.FloorMap[Random.Range(0, DiscoData.Instance.FloorMap.Count)];
            while (DiscoData.Instance.placementDataHandler.ContainsKey(GridHandler.Instance.GetWorldToCell(target),
                       ePlacementLayer.Floor))
            {
                target = DiscoData.Instance.FloorMap[Random.Range(0, DiscoData.Instance.FloorMap.Count)];
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