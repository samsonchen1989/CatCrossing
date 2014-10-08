using UnityEngine;
using System.Collections;

public class ItemBackpack : MonoBehaviour
{

    public int maxRows;
    public int maxColumns;
    public int padding;
    public int spacing;
    public GameObject itemSlotPrefab;

    // Use this for initialization
    void Start()
    {
        if (itemSlotPrefab == null) {
            Debug.LogError("Fail to get item slot prefabs.");
            return;
        }

        //just for ui test
        Inventory.Instance.AddStack(ItemPrefabsDefinition.StackClone(2, 3));
        Inventory.Instance.AddStack(ItemPrefabsDefinition.StackClone(0, 25));
        Inventory.Instance.AddStack(ItemPrefabsDefinition.StackClone(1, 10));

        this.transform.localPosition = new Vector3(-(spacing * (maxColumns / 2.0f)) + padding,
                                                   -(Screen.height / 2 - spacing * maxRows - padding),
                                                   0f);

        int count = 0;

        for (int i = 0; i < maxRows; i++) {
            for (int j = 0; j < maxColumns; j++) {
                // Item slot prefab and ItemSlot script are created here.
                GameObject itemSlot = NGUITools.AddChild(this.gameObject, itemSlotPrefab);
                itemSlot.transform.localPosition = new Vector3(padding + (j + 0.3f) * spacing, -(padding + (i + 0.3f) * spacing), 0f);

                ItemSlot slot = itemSlot.GetComponent<ItemSlot>();
                if (slot != null) {
                    slot.backPack = this;
                    slot.slotID = count;
                }

                count++;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
    
    }
}
