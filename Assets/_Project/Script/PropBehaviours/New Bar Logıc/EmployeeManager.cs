using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Rendering;

namespace PropBehaviours.New_Bar_Logıc
{
    public class EmployeeManager : Singleton<EmployeeManager>
    {
        [OdinSerialize] public List<IBartender> _bartenders = new List<IBartender>();
        [OdinSerialize] public List<IBar> _bars = new List<IBar>();

        private void Start()
        {
            _bars = new List<IBar>();
            _bartenders = new List<IBartender>();
            UpdateBartenders();
        }

        private void OnEnable()
        {
            PlacementDataHandler.OnPropUpdate += UpdateProps;
            NPCSystem.OnBartenderCreated += UpdateBartenders;
        }

        private void OnDisable()
        {
            PlacementDataHandler.OnPropUpdate -= UpdateProps;
            NPCSystem.OnBartenderCreated -= UpdateBartenders;
        }
        
        private void UpdateBartenders()
        {
            _bartenders.Clear();
            _bartenders = FindObjectsOfType<MonoBehaviour>().OfType<IBartender>().ToList();
        }

        private void UpdateProps()
        {
            _bars.Clear();
            foreach (var propUnit in DiscoData.Instance.placementDataHandler.GetPropList)
            {
                if (propUnit is IBar ibar)
                {
                    _bars.Add(ibar);
                }
            }
        }
    }
}