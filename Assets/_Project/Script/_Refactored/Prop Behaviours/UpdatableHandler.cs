using System;
using System.Collections.Generic;
using _Initializer;
using UnityEngine;

namespace Prop_Behaviours
{
    public class UpdatableHandler : MonoBehaviour
    {
        public List<IUpdateable> updates = new List<IUpdateable>();

        private void Awake()
        {
            ServiceLocator.Register(this);
        }

        private void Update()
        {
            foreach (var update in updates)
            {
                update.TickUpdate(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            foreach (var update in updates)
            {
                update.TickFixedUpdate(Time.deltaTime);
            }
        }

        public void RegisterUpdate(IUpdateable updateable)
        {
            updates.Add(updateable);
        }
    }
}