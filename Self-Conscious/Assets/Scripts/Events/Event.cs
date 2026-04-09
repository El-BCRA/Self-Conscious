using System.Collections.Generic;
using UnityEngine;

namespace SelfConscious
{
    public abstract class Event<T> : ScriptableObject
    {
        public T testingValue;
        private List<EventListener<T>> listeners;

        public void Register(EventListener<T> listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void Unregister(EventListener<T> listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }

        public void Invoke(T value)
        {
            foreach (EventListener<T> listener in listeners)
            {
                listener.Listen(value);
            }
        }
    }
}
