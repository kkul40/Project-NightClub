using Data;
using UnityEngine;

namespace New_NPC.Activities
{
    public class WalkRandomActivity : IActivity
    {
        private readonly float delay = 2;
        private float timer;
        
        public bool IsEnded { get; private set; }
        
        public void StartActivity(ActivityNeedsData and)
        {
            and.Npc.SetNewDestination(GetRandomDestination(and));
            and.Npc.SetAnimation(eNpcAnimation.Walk);
        }

        public void UpdateActivity(ActivityNeedsData and)
        {
            if (and.Npc.hasReachedDestination)
            {
                and.Npc.SetAnimation(eNpcAnimation.Idle);
                timer += Time.deltaTime;
                if (timer > delay)
                {
                    and.Npc.SetNewDestination(GetRandomDestination(and));
                    and.Npc.SetAnimation(eNpcAnimation.Walk);
                    timer = 0;
                    IsEnded = true;
                }
            }
        }

        public void EndActivity(ActivityNeedsData and)
        {
        }
        
        public Vector3 GetRandomDestination(ActivityNeedsData and)
        {
            var loopCount = 0;

            var target = and.DiscoData.mapData.FloorMap[Random.Range(0, and.DiscoData.mapData.FloorMap.Count)];
            while (and.DiscoData.placementDataHandler.ContainsKey(and.GridHandler.GetWorldToCell(target),
                       ePlacementLayer.Floor))
            {
                target = DiscoData.Instance.mapData.FloorMap[Random.Range(0, DiscoData.Instance.mapData.FloorMap.Count)];
                loopCount++;
                if (loopCount >= 99)
                {
                    IsEnded = true;
                    break;
                }
            }

            return target;
        }
    }
}