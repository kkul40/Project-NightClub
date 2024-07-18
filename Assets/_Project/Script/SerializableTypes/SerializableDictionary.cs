using System;
using System.Collections.Generic;
using UnityEngine;

namespace SerializableTypes
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> keys = new();
        [SerializeField] private List<TValue> values = new();

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach (var pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();

            if (keys.Count != values.Count)
                Debug.LogError("Tried to deserialize a SerializableDictionary, but the amount of keys (" + keys.Count +
                               ") does not match the number of values (" + values.Count +
                               ") which indicates that something went wrong");

            for (var i = 0; i < keys.Count; i++) 
                Add(keys[i], values[i]);
        }
    }
    
    [Serializable]
    public class SerializableTuple<T1, T2, T3> : ISerializationCallbackReceiver
    {
        [SerializeField] public T1 Item1;
        [SerializeField] public T2 Item2;
        [SerializeField] public T3 Item3;
        
        private Tuple<T1, T2, T3> tuple;
        
        public SerializableTuple(T1 item1, T2 item2, T3 item3)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;

            tuple = new Tuple<T1, T2, T3>(item1, item2, item3);
        }

        public void OnBeforeSerialize()
        {
            Item1 = tuple.Item1;
            Item2 = tuple.Item2;
            Item3 = tuple.Item3;

            tuple = new Tuple<T1, T2, T3>(Item1,Item2,Item3);
        }

        public void OnAfterDeserialize()
        {
            tuple = new Tuple<T1, T2, T3>(Item1, Item2, Item3);
        }
    }
}