using UnityEngine;
using System.Collections;

public class InteractiveItem : MonoBehaviour
{
    const float MaxInteractDis = 5.0f;

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        if (MouseRaycastManager.Instance.hitObjectType == HitObjectType.InteractiveItem) {
            GameObject hitObject = MouseRaycastManager.Instance.hitObject;
            if (hitObject == null) {
                return;
            }

            InteractiveItemInfo itemInfo = hitObject.GetComponent<InteractiveItemInfo>(); 
            if (itemInfo == null) {
                return;
            }

            if (InputManager.GetMouseButtonUp(1)) {
                if (MouseRaycastManager.Instance.hitDistance <= MaxInteractDis) {
                    ItemStack leftStack = Inventory.Instance.AddStack(itemInfo.GetItemStack());
                    if (leftStack == null) {
                        // All item stack added to inventory
                        itemInfo.ReduceInteractiveCount();
                    }
                } else {
                    ScreenTipManager.Instance.DisplayTipMessage("Too far away.");
                }
            }
        }
    }
}
