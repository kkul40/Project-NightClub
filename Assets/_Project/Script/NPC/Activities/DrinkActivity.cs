using Data;
using PropBehaviours;
using UnityEngine;

namespace NPC.Activities
{
    public class DrinkActivity : Activity
    {
        private readonly float waitTime = 1;
        private DrinkState _drinkState;

        private IBar iBar;

        private float timer;
        public override bool isEnded { get; protected set; }
        public override bool isCanceled { get; protected set; }

        public override void StartActivity(NPC npc)
        {
            var barList = GetMultiplePropsByType<Bar>(ePlacementLayer.Floor);

            if (barList.Count > 0)
                iBar = barList[Random.Range(0, barList.Count)];

            if (iBar != null)
            {
                if (iBar.HasDrinks)
                {
                    npc.SetAnimation(eNpcAnimation.Walk);
                    npc.SetNewDestination(iBar.WaitPosition.position);
                    iBar.DecreaseDrinkCount();
                    _drinkState = DrinkState.Walk;
                }
                else
                {
                    isCanceled = true;
                }
            }
            else
            {
                isCanceled = true;
            }
        }

        public override void UpdateActivity(NPC npc)
        {
            if (isCanceled) return;

            switch (_drinkState)
            {
                case DrinkState.Walk:
                    if (Vector3.Distance(npc.transform.position, iBar.WaitPosition.position) <= 0.1f)
                    {
                        npc.SetAnimation(eNpcAnimation.Idle);
                        npc.SetRotation(iBar.WaitPosition.rotation);
                        _drinkState = DrinkState.Wait;
                    }

                    break;
                case DrinkState.Wait:
                    timer += Time.deltaTime;
                    if (timer > waitTime)
                    {
                        timer = 0;
                        iBar.GetDrink();
                        isEnded = true;
                    }

                    break;
            }
        }

        public override void EndActivity(NPC npc)
        {
            if (isCanceled) return;

            npc.SetNewDestination(new WalkRandomActivity().GetRandomDestination());
        }

        private enum DrinkState
        {
            Walk,
            Wait
        }
    }
}