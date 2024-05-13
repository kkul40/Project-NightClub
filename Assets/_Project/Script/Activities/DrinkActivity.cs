using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Activities
{
    public class DrinkActivity : Activity
    {
        public override bool isEnded { get; protected set; }
        public override bool isCanceled { get; protected set; }

        private IBar iBar = null;

        private DrinkState _drinkState;

        private float timer;
        private float waitTime = 1;

        public override void StartActivity(NPC npc)
        {
            var barList = GetMultiplePropsByType<Bar>();

            if(barList.Count > 0)
                iBar = barList[Random.Range(0, barList.Count)] as IBar;

            if (iBar != null)
            {
                if (iBar.HasDrinks)
                {
                    npc.ChangeState(eNpcAnimation.Walk);
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
                        npc.ChangeState(eNpcAnimation.Idle);
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

        enum DrinkState
        {
            Walk,
            Wait
        }
    }
}