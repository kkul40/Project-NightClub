using System;
using Data;
using ScriptableObjects;
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

        public override void StartActivity(NPC npc)
        {
            if (isCanceled) return;

            npc.SetNewDestination(GetRandomDestination());
            npc.ChangeState(eNpcAnimation.Walk);
        }

        public override void UpdateActivity(NPC npc)
        {
            if (isCanceled) return;

            if (!npc._navMeshAgent.hasPath)
            {
                npc.ChangeState(eNpcAnimation.Idle);
                timer += Time.deltaTime;
                if (timer > delay)
                {
                    npc.SetNewDestination(GetRandomDestination());
                    npc.ChangeState(eNpcAnimation.Walk);
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
            while (GameData.Instance.PlacementHandler.ContainsKey(GridHandler.Instance.GetWorldToCell(target),
                       PlacementMethodType.Placement))
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