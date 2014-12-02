using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Obsolete
// Use Messenger using C# delegate instead of this one, SendMessage may cause performance
// problem if not use properly.
public class Notification : MonoBehaviour
{
    // Internal reference to all listener for notifications
    private Dictionary<string, List<Component>> listeners = new Dictionary<string, List<Component>>();

    // Add a listener for a specific nofication to its listeners list
    public void AddListener(Component listener, string notificationName)
    {
        if (!listeners.ContainsKey(notificationName)) {
            listeners.Add(notificationName, new List<Component>());
        }

        listeners[notificationName].Add(listener);
    }

    // Post a notification to all its listeners
    public void PostNotification(Component sender, string notificationName)
    {
        if (!listeners.ContainsKey(notificationName)) {
            return;
        }

        foreach (Component listener in listeners[notificationName]) {
            listener.SendMessage(notificationName, sender, SendMessageOptions.DontRequireReceiver);
        }
    }

    // Remove a listener for a notification
    public void RemoveListener(Component sender, string notification)
    {
        if (!listeners.ContainsKey(notification)) {
            return;
        }

        for (int i = 0; i < listeners[notification].Count - 1; i++) {
            // Check instance ID, if matched, remove it
            if (listeners[notification][i].GetInstanceID() == sender.GetInstanceID()) {
                listeners[notification].RemoveAt(i);
            }
        }
    }

    // Clear listener list
    public void RemoveRedundancies()
    {
        // Create new dictionary
        Dictionary<string, List<Component>> tmpListeners = new Dictionary<string, List<Component>>();

        // Cycle through all dictionary entries
        foreach (KeyValuePair<string, List<Component>> Item in listeners) {
            // Cycle through all listeners objects in list, remove null objects
            for (int i = 0; i < Item.Value.Count; i++) {
                if (Item.Value[i] == null) {
                    Item.Value.RemoveAt(i);
                }
            }

            // If items remain in list for this notification, then add this to tmp dictionary
            if (Item.Value.Count > 0) {
                tmpListeners.Add(Item.Key, Item.Value);
            }
        }

        listeners = tmpListeners;
    }

    // Clear all listeners
    public void ClearListeners()
    {
        // Remove all listeners
        listeners.Clear();
    }
}
