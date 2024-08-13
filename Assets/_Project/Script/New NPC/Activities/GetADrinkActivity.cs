using System.Collections;
using Data;
using DG.Tweening;
using PropBehaviours;
using UnityEngine;

namespace New_NPC.Activities
{
    public class GetADrinkActivity : IActivity
    {
        //TODO Yeni BarSistemine Uyarla
        public bool IsEnded { get; private set; }
        
        private IBar _bar;
        
        public bool CanStartActivity(ActivityNeedsData and)
        {
            return true;
        }
        
        public void OnActivityStart(ActivityNeedsData and)
        {
            var _bars = and.GetAvaliablePropsByInterface<IBar>();
            
            if (_bars.Count == 0)
            {
                IsEnded = true;
                return;
            }
        
            _bar = _bars[Random.Range(0, _bars.Count)];
        
            if (!_bar.HasDrinks)
            {
                IsEnded = true;
                return;
            }
        
            if(!and.Npc.PathFinder.GoTargetDestination(_bar.CustomerWaitPosition.position))
            {
                IsEnded = true;
                return;
            }
            
            and.Npc.animationController.PlayAnimation(eAnimationType.NPC_Walk);
            DOTween.instance.StartCoroutine(CoGetDrink(and));
        }
        
        public void OnActivityUpdate(ActivityNeedsData and)
        {
        }
        
        public void OnActivityEnd(ActivityNeedsData and)
        {
            and.Npc.PathFinder.CancelDestination();
            and.Npc.animationController.PlayAnimation(eAnimationType.NPC_Idle);
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
        
            and.Npc.animationController.PlayAnimation(eAnimationType.NPC_Idle);
            and.Npc.PathFinder.SetPositioning(_bar.CustomerWaitPosition.rotation);
            yield return new WaitForSeconds(1);
            _bar.GetDrink();
        
            yield return new WaitForSeconds(0.5f);
        
            IsEnded = true;
        
            yield return null;
        }
    }
}