using System;
using _Initializer;
using Data;
using DiscoSystem;
using DiscoSystem.Building_System.GameEvents;
using DiscoSystem.Character.Bartender;
using DiscoSystem.Character.Bartender.Command;
using PropBehaviours;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Rendering;

namespace Prop_Behaviours.Bar
{
    // TODO Bartender Manager Class
    // TODO Add ReAssignment Logic
    // TODO Use This Script To Manage All Your Bartenders. Give them states(orders), have them clean stuff.
    public class BarMediator : MonoBehaviour
    {
        [SerializeField] public GameObject DrinkTablePrefab;

        [OdinSerialize] public SerializedDictionary<int, IBartender> _bartenders;
        [OdinSerialize] public SerializedDictionary<int, IBar> _bars;

        private void Awake()
        {
            ServiceLocator.Register(this);
        }

        private void Start()
        {
            _bars = new SerializedDictionary<int, IBar>();
            _bartenders = new SerializedDictionary<int, IBartender>();
            
            GameEvent.Subscribe<Event_BartenderHired>(AssignBartender);
            GameEvent.Subscribe<Event_BartenderKicked>(DeassignBartender);
            
            GameEvent.Subscribe<Event_PropPlaced>(TryAssigningBar);
            GameEvent.Subscribe<Event_PropRemoved>(TryDeassigningBar);
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

        private void AssignBartender(Event_BartenderHired bartenderEvent)
        {
            _bartenders.Add(bartenderEvent.Bartender.InstanceID, bartenderEvent.Bartender);
        }
     
        private void DeassignBartender(Event_BartenderKicked bartenderEvent)
        {
            _bartenders.Remove(bartenderEvent.Bartender.InstanceID);
        }

        private void TryAssigningBar(Event_PropPlaced placedEvent)
        {
            if(placedEvent.PropUnit is IBar bar)
                _bars.Add(bar.InstanceID, bar);
        }
        
        private void TryDeassigningBar(Event_PropRemoved removedEvent)
        {
            if(removedEvent.PropUnit is IBar bar)
                _bars.Remove(bar.InstanceID);
        }
    }
}