using UnityEngine;
using System.Collections;

public static class InputManager
{
    public static bool GetMouseButton(int button)
    {
        if (UICamera.hoveredObject == null) {
            return Input.GetMouseButton(button);
        } else {
            // If click/drop on 2D UICamera, do not send the event to MainCamera.
            // 5 represents layer "UI"
            if (UICamera.hoveredObject.layer == 5) {
                return false;
            }
        }

        return false;
    }
}
