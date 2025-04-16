using System.Collections.Generic;

namespace System.Building_System.GameEvents
{
    public class EventManager
    {
        private Dictionary<Type, Delegate> _events = new();

        public void Subscribe<T>(Action<T> callback)
        {
            Type eventType = typeof(T);
            if (!_events.ContainsKey(eventType))
                _events[eventType] = callback;
            else
                _events[eventType] = Delegate.Combine(_events[eventType], callback);
        }

        public void Unsubscribe<T>(Action<T> callback)
        {
            Type eventType = typeof(T);
            if (_events.ContainsKey(eventType))
            {
                _events[eventType] = Delegate.Remove(_events[eventType], callback);
                if (_events[eventType] == null)
                    _events.Remove(eventType);
            }
        }

        public void Trigger<T>(T eventData)
        {
            Type eventType = typeof(T);
            if (_events.ContainsKey(eventType))
            {
                (_events[eventType] as Action<T>)?.Invoke(eventData);
            }
        }
    }
}