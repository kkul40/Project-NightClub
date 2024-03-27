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

            npc.SetRandomTarget();
            npc.ChangeState(NpcState.Walk);
        }

        public override void UpdateActivity(NPC npc)
        {
            if (isCanceled) return;

            if (!npc._navMeshAgent.hasPath)
            {
                npc.ChangeState(NpcState.Idle);
                timer += Time.deltaTime;
                if (timer > delay)
                {
                    npc.SetRandomTarget();
                    npc.ChangeState(NpcState.Walk);
                    timer = 0;
                    isEnded = true;
                }
            }
        }

        public override void EndActivity(NPC npc)
        {
            if (isCanceled) return;
        }
    }
}