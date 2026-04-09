using UnityEngine;
using UnityEngine.Events;

namespace SelfConscious
{
    public class EventListener<T> : MonoBehaviour
    {
        public Event<T> eventToListen;
        public UnityEvent<T> onEvent;


        public void OnEnable()
        {
            eventToListen.Register(this);
        }

        public void OnDisable()
        {
            eventToListen.Unregister(this);
        }

        public void Listen(T value)
        {
            onEvent?.Invoke(value);
        }

    }
}
