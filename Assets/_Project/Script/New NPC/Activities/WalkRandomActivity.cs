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
            and.Npc.pathFinder.GoToDestination(GetRandomDestination(and));
            and.Npc.SetAnimation(eNpcAnimation.Walk);
        }

        public void UpdateActivity(ActivityNeedsData and)
        {
            if (and.Npc.pathFinder.hasReachedDestination)
            {
                and.Npc.SetAnimation(eNpcAnimation.Idle);
                timer += Time.deltaTime;
                if (timer > delay)
                {
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

            var target = and.DiscoData.MapData.FloorGridDatas[
                Random.Range(0, DiscoData.Instance.MapData.CurrentMapSize.x),
                Random.Range(0, DiscoData.Instance.MapData.CurrentMapSize.y)];

            // if (target == null)
            // {
            //     return and.Npc.transform.position;
            // }

            while (target.assignedObjectID != -1)
            {
                target = and.DiscoData.MapData.FloorGridDatas[
                    Random.Range(0, DiscoData.Instance.MapData.CurrentMapSize.x),
                    Random.Range(0, DiscoData.Instance.MapData.CurrentMapSize.y)];

                loopCount++;
                if (loopCount >= 99)
                {
                    IsEnded = true;
                    break;
                }
            }

            return target.CellPosition;
        }
    }
}