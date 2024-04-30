using BuildingSystemFolder;
using UnityEngine;

namespace Activities
{
    public class WalkRandomActivity : Activity
    {
        public override bool isEnded { get; protected set; }
        public override bool isCanceled { get; protected set; }

        private float delay = 2;
        private float timer;
    
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
        
        private Vector3 GetRandomDestination()
        {
            int loopCount = 0;
        
            var target = GameData.Instance.FloorMap[Random.Range(0, GameData.Instance.FloorMap.Count)];
            while (GameData.Instance.PlacementHandler.ContainsKey(BuildingSystem.Instance.GetWorldToCell(target)))
            {
                target = GameData.Instance.FloorMap[Random.Range(0, GameData.Instance.FloorMap.Count)];
                loopCount++;
                if (loopCount >= 99)
                {
                    break;
                }
            }

            return target;
        }
    }
}