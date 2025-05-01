using System.Collections.Generic;
using DiscoSystem;
using UnityEngine;

namespace Prop_Behaviours
{
    public class UpdatableHandler : Singleton<UpdatableHandler>
    {
        public List<IUpdateable> updates = new List<IUpdateable>();

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