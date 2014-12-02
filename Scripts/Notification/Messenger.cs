using System;
using System.Collections.Generic;

// A messenger for events that have no parameters
public static class Messenger
{
    private static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

    public static void AddListener(string eventType, Callback handler)
    {
        // Obtain a lock on the event table to keep this thread-safe
        lock (eventTable) {
            // Create an entry for this event type if it not exist
            if (!eventTable.ContainsKey(eventType)) {
                eventTable.Add(eventType, null);
            }

            eventTable[eventType] = (Callback)eventTable[eventType] + handler;
        }
    }

    public static void RemoveListener(string eventType, Callback handler)
    {
        lock (eventTable) {
            if (eventTable.ContainsKey(eventType)) {
                eventTable[eventType] = (Callback)eventTable[eventType] - handler;

                // If there's nothing left then remove the event type from event table
                if (eventTable[eventType] == null) {
                    eventTable.Remove(eventType);
                }
            }
        }
    }

    public static void Invoke(string eventType)
    {
        Delegate d;
        // Invoke the delegate only if the event type is in the dictionary
        if (eventTable.TryGetValue(eventType, out d)) {
            // Take a local copy to prevent a race condition if another thread
            // were to unsubscribe from this event
            Callback callback = (Callback)d;

            // Invoke the delegate if it's not null
            if (callback != null) {
                callback();
            }
        }
    }
}

// A messenger for events that have one paremeter of type T
public static class Messenger<T>
{
    private static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

    public static void AddListener(string eventType, Callback<T> handler)
    {
        lock (eventTable) {
            // Create an entry for this event type if it doesn't already exist
            if (!eventTable.ContainsKey(eventType)) {
                eventTable.Add(eventType, null);
            }

            eventTable[eventType] = (Callback<T>)eventTable[eventType] + handler;
        }
    }

    public static void RemoveListener(string eventType, Callback<T> handler)
    {
        lock (eventTable) {
            if (eventTable.ContainsKey(eventType)) {
                eventTable[eventType] = (Callback<T>)eventTable[eventType] - handler;

                // If there's nothing left then remove the event type from event table
                if (eventTable[eventType] == null) {
                    eventTable.Remove(eventType);
                }
            }
        }
    }

    public static void Invoke(string eventType, T arg1)
    {
        Delegate d;
        // Invoke the delegate only if the event type is in the dictionary
        if (eventTable.TryGetValue(eventType, out d)) {
            // Take a local copy to prevent a race condition if another thread
            // were to unsubscribe from this event.
            Callback<T> callback = (Callback<T>)d;

            // Invoke the delegate if it's not null
            if (callback != null) {
                callback(arg1);
            }
        }
    }
}

public static class Messenger<T, U>
{
    private static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

    public static void AddListener(string eventType, Callback<T, U> handler)
    {
        // Obtain a lock on the event table to keep this threadsafe
        lock (eventTable) {
            if (!eventTable.ContainsKey(eventType)) {
                eventTable.Add(eventType, null);
            }

            eventTable[eventType] = (Callback<T, U>)eventTable[eventType] + handler;
        }
    }

    public static void RemoveListener(string eventType, Callback<T, U> handler)
    {
        lock (eventTable) {
            if (eventTable.ContainsKey(eventType)) {
                eventTable[eventType] = (Callback<T, U>)eventTable[eventType] - handler;

                if (eventTable[eventType] == null) {
                    eventTable.Remove(eventType);
                }
            }
        }
    }

    public static void Invoke(string eventType, T arg1, U arg2)
    {
        Delegate d;
        // Invoke the delegate only if the event type is in the dictionary
        if (eventTable.TryGetValue(eventType, out d)) {
            Callback<T, U> callback = (Callback<T, U>)d;

            // Invoke the delegate if it's not null
            if (callback != null) {
                callback(arg1, arg2);
            }
        }
    }
}
