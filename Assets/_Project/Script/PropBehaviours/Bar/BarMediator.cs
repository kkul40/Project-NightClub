using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using ScriptableObjects;
using UnityEngine;

namespace PropBehaviours
{
    public class BarMediator : Singleton<BarMediator>
    {
        [SerializeField] private GameObject DrinkTablePrefab;

        private Dictionary<int, IBartender> _bartenders;
        private Dictionary<int, IBar> _bars;

        private void Start()
        {
            GetBarAndBartender();
        }

        private void OnEnable()
        {
            PlacementDataHandler.OnPropAdded += GetBarAndBartender;
            PlacementDataHandler.OnPropRemoved += GetBarAndBartender;
            NPCSystem.OnBartenderCreated += GetBarAndBartender;
        }

        private void OnDisable()
        {
            PlacementDataHandler.OnPropAdded -= GetBarAndBartender;
            PlacementDataHandler.OnPropRemoved -= GetBarAndBartender;
            NPCSystem.OnBartenderCreated -= GetBarAndBartender;
        }

        private IBartender GetAvaliableBartender()
        {
            foreach (var bartender in _bartenders.Values)
                if (!bartender.IsBusy)
                    return bartender;
            
            return null;
        }

        private void GetBarAndBartender()
        {
            var bars = FindObjectsOfType<MonoBehaviour>().OfType<IBar>().ToList();
            var bartenders = FindObjectsOfType<MonoBehaviour>().OfType<IBartender>().ToList();

            foreach (var bar in bars)
                _bars.Add(bar.ID, bar);

            foreach (var bartender in bartenders)
                _bartenders.Add(bartender.ID, bartender);
        }
    }
}