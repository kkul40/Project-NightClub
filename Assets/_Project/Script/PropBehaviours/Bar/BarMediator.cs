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

        private List<IBar> _bars;
        private List<NewBartender> _bartenders;

        // Commendlari Bartendera Tasi ve bu sekilde tum bartender gorev bitince yeni goreve gecebilirler
        private List<IBartenderCommand> _barCommands = new();

        private void Start()
        {
            GetBarAndBartender();
        }

        private void OnEnable()
        {
            PlacementDataHandler.OnPropAdded += GetBarAndBartender;
            NPCSystem.OnBartenderCreated += GetBarAndBartender;
        }

        private void OnDisable()
        {
            PlacementDataHandler.OnPropAdded -= GetBarAndBartender;
            NPCSystem.OnBartenderCreated -= GetBarAndBartender;
        }

        private void Update()
        {
            for (var i = _barCommands.Count - 1; i >= 0; i--)
                if (_barCommands[i].UpdateCommand(this))
                    _barCommands.RemoveAt(i);
        }

        public void CreateDrinkTable(IBar source, Drink drink)
        {
            var d = Instantiate(DrinkTablePrefab, source.CounterPlacePosition);
            d.transform.position = source.CounterPlacePosition.position;
            var drinkTable = d.GetComponent<DrinkTable>();
            drinkTable.SetUpTable(drink);
        }

        public void AddCommandToExecute(IBar source, IBartenderCommand command)
        {
            var avaliableBartender = GetAvaliableBartender();
            if (avaliableBartender == null) return;

            command.InitCommand(source, avaliableBartender);
            if (command.IsDoable()) _barCommands.Add(command);
        }

        public void AddCommandToExecute(NewBartender source, IBartenderCommand command)
        {
            command.InitCommand(_bars[0], source);
            if (command.IsDoable()) _barCommands.Add(command);
        }

        private NewBartender GetAvaliableBartender()
        {
            foreach (var bartender in _bartenders)
                if (!bartender.IsBusy)
                    return bartender;
            return null;
        }

        private void GetBarAndBartender()
        {
            _bars = FindObjectsOfType<MonoBehaviour>().OfType<IBar>().ToList();
            _bartenders = FindObjectsOfType<MonoBehaviour>().OfType<NewBartender>().ToList();
        }
    }
}