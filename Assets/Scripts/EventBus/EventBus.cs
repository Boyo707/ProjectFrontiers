using UnityEngine;

public class EventBus<T> where T : Event {
    public static event System.Action<T> OnEvent;

    public static void Publish(T e) {
        if (OnEvent != null) {
            OnEvent?.Invoke(e);
        }
    }
}