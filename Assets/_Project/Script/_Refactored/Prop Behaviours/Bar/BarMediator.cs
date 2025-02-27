using Data;
using DiscoSystem;
using GameEvents;
using PropBehaviours;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Rendering;

namespace Prop_Behaviours.Bar
{
    // TODO Bartender Manager Class
    // TODO Add ReAssignment Logic
    // TODO Use This Script To Manage All Your Bartenders. Give them states(orders), have them clean stuff.
    public class BarMediator : Singleton<BarMediator>, ISaveLoad
    {
        [SerializeField] public GameObject DrinkTablePrefab;

        [OdinSerialize] public SerializedDictionary<int, IBartender> _bartenders;
        [OdinSerialize] public SerializedDictionary<int, IBar> _bars;

        private void Start()
        {
            _bars = new SerializedDictionary<int, IBar>();
            _bartenders = new SerializedDictionary<int, IBartender>();
        }

        private void OnEnable()
        {
            KEvent_Employee.OnBartenderHired += AssignBartender;
            KEvent_Employee.OnBartenderKicked += DeassignBartender;
            KEvent_Building.OnPropPlaced += TryAssigningBar;
            KEvent_Building.OnPropRemoved += TryDeassigningBar;
        }

        private void OnDisable()
        {
            KEvent_Employee.OnBartenderHired -= AssignBartender;
            KEvent_Employee.OnBartenderKicked -= DeassignBartender;
            KEvent_Building.OnPropPlaced -= TryAssigningBar;
            KEvent_Building.OnPropRemoved -= TryDeassigningBar;
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

        private void AssignBartender(IBartender bartender)
        {
            _bartenders.Add(bartender.InstanceID, bartender);
        }
     
        private void DeassignBartender(IBartender bartender)
        {
            _bartenders.Remove(bartender.InstanceID);
        }

        private void TryAssigningBar(IPropUnit unit)
        {
            if(unit is IBar bar)
                _bars.Add(bar.InstanceID, bar);
        }
        
        private void TryDeassigningBar(IPropUnit unit)
        {
            if(unit is IBar bar)
                _bars.Remove(bar.InstanceID);
        }

        #region SavingAndLoading...

        public SavePriority Priority { get; } = SavePriority.Default;

        public void LoadData(GameData gameData)
        {
        }

        public void SaveData(ref GameData gameData)
        {
        }

        #endregion
    }
}