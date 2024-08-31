using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using ScriptableObjects;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Rendering;

namespace PropBehaviours
{
    public class BarMediator : Singleton<BarMediator>
    {
        [SerializeField] private GameObject DrinkTablePrefab;

        [OdinSerialize] public SerializedDictionary<int, IBartender> _bartenders;
        [OdinSerialize] public SerializedDictionary<int, IBar> _bars;

        private void Start()
        {
            _bars = new SerializedDictionary<int, IBar>();
            _bartenders = new SerializedDictionary<int, IBartender>();
            GetBarAndBartender();
        }

        private void OnEnable()
        {
            PlacementDataHandler.OnPropUpdate += GetBarAndBartender;
            NPCSystem.OnBartenderCreated += GetBarAndBartender;
        }

        private void OnDisable()
        {
            PlacementDataHandler.OnPropUpdate -= GetBarAndBartender;
            NPCSystem.OnBartenderCreated -= GetBarAndBartender;
        }

        public void AddCommand(IBar source, IBartenderCommand command)
        {
            var avaliable = GetAvaliableBartender();
            if (avaliable == null)
            {
                Debug.Log("Could not found avaliable bartender");
                return;
            }

            command.InitCommand(source, avaliable);
            if (command.IsDoable()) avaliable.AddCommand(command);
        }

        public DrinkTable CreateDrinkTable(IBar bar)
        {
            var obj = Instantiate(DrinkTablePrefab, bar.CounterPlacePosition);
            obj.transform.position = bar.CounterPlacePosition.position;
            var drinkTable = obj.GetComponent<DrinkTable>();
            drinkTable.SetUpTable(bar.DrinkData);

            return drinkTable;
        }

        private IBartender GetAvaliableBartender()
        {
            foreach (var bartender in _bartenders.Values)
                if (!bartender.IsBusy)
                    return bartender;

            var workCount = 999;
            IBartender output = null;
            foreach (var bartender in _bartenders.Values)
                if (bartender.BartenderCommands.Count < workCount)
                {
                    workCount = bartender.BartenderCommands.Count;
                    output = bartender;
                }

            return output;
        }

        private void GetBarAndBartender()
        {
            _bars.Clear();
            _bartenders.Clear();

            var bars = DiscoData.Instance.placementDataHandler.GetPropsByType<IBar>();
            var bartenders = FindObjectsOfType<MonoBehaviour>().OfType<IBartender>().ToList();

            foreach (var bar in bars)
                _bars.Add(bar.InstanceID, bar);

            foreach (var bartender in bartenders)
                _bartenders.Add(bartender.InstanceID, bartender);
        }
    }
}