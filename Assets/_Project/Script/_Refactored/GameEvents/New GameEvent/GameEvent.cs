using System;
using UnityEngine;

namespace GameEvents
{
    public class GameEvent : MonoBehaviour
    {
        private static EventManager _eventManager = new();

        public void Initialize()
        {
            _eventManager = new EventManager();
        }

        public static void Subscribe<T>(Action<T> callback) => _eventManager.Subscribe(callback);
        public static void Unsubscribe<T>(Action<T> callback) => _eventManager.Unsubscribe(callback);
        public static void Trigger<T>(T eventData) => _eventManager.Trigger(eventData);
    }
}