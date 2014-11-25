using UnityEngine;
using System.Collections;

public static class InputManager
{
    // 5 is UI Layer
    const int UILayer = 5;

    public static bool GetMouseButton(int button)
    {
        if (UICamera.hoveredObject == null) {
            return Input.GetMouseButton(button);
        } else {
            // If click/drop on 2D UICamera, do not send the event to MainCamera.
            // 5 represents layer "UI"
            if (UICamera.hoveredObject.layer == UILayer) {
                return false;
            }
        }

        return false;
    }

    public static bool GetMouseButtonUp(int button)
    {
        if (UICamera.hoveredObject == null) {
            return Input.GetMouseButtonUp(button);
        } else {
            // If click/drop on 2D UICamera, do not send the event to MainCamera.
            // 5 represents layer "UI"
            if (UICamera.hoveredObject.layer == UILayer) {
                return false;
            }
        }
        
        return false;
    }
}
