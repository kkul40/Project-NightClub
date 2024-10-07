using System.Collections;
using Data;
using DefaultNamespace;
using DG.Tweening;
using PropBehaviours;
using UnityEngine;

namespace NPC_Stuff.Activities
{
    public class GetADrinkActivity : IActivity
    {
        //TODO Yeni BarSistemine Uyarla
        public bool CheckForPlacementOnTop { get; } = true;
        public bool IsEnded { get; private set; }

        private IBar _bar;

        public bool CanStartActivity(ActivityNeedsData and)
        {
            var _bars = and.GetAvaliablePropsByInterface<IBar>();
            if (_bars == null) return false;

            _bar = _bars[Random.Range(0, _bars.Count)];
            int iteration = 0;
            while (!_bar.HasDrinks)
            {
                _bar = _bars[Random.Range(0, _bars.Count)];

                if (Helper.IterateTo100(ref iteration)) return false;
            }

            if (!and.Npc.PathFinder.GoTargetDestination(_bar.CustomerWaitPosition.position)) return false;

            return true;
        }

        public void OnActivityStart(ActivityNeedsData and)
        {
            and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Walk);
            DOTween.instance.StartCoroutine(CoGetDrink(and));
        }

        public void OnActivityUpdate(ActivityNeedsData and)
        {
        }

        public void OnActivityEnd(ActivityNeedsData and)
        {
            and.Npc.PathFinder.CancelDestination();
            and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Idle);
        }


        private IEnumerator CoGetDrink(ActivityNeedsData and)
        {
            while (!and.Npc.PathFinder.HasReachedDestination)
            {
                if (!_bar.HasDrinks)
                {
                    IsEnded = true;
                    yield break;
                }

                yield return null;
            }

            yield return new WaitUntil(() => and.Npc.PathFinder.HasReachedDestination);

            and.Npc.AnimationController.PlayAnimation(eAnimationType.NPC_Idle);
            and.Npc.PathFinder.SetPositioning(_bar.CustomerWaitPosition.rotation);
            yield return new WaitForSeconds(1);
            _bar.GetDrink();

            yield return new WaitForSeconds(0.5f);

            IsEnded = true;

            yield return null;
        }
    }
}