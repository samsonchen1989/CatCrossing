using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

        foreach(Component listener in listeners[notificationName]) {
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

    // Clear all listeners
    public void ClearListeners()
    {
        // Remove all listeners
        listeners.Clear();
    }
}
