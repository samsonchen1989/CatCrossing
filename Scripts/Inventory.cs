using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public delegate void InventoryChangedHandler();

public class Inventory : MonoBehaviour {

    #region Sigleton Code

    private static Inventory instance;
    public static Inventory Instance
    {
        get {
            if (instance == null) {
                Debug.LogError("Fail to find Inventory Instance.");
            }

            return instance;
        }
    }

    #endregion
    
    //Inventory changed event
    public event InventoryChangedHandler InventoryChanged = delegate {};

    public int inventorySize = 15;
    public int InventorySize {
        get {
            return inventorySize;
        }
    }

    [HideInInspector]
    public List<ItemStack> inventory;
    [HideInInspector]
    public ItemStack swapItemStack;
    [HideInInspector]
    public int swapSlotId;
    
    public ReadOnlyCollection<ItemStack> readonlyInventory;
    public ReadOnlyCollection<ItemStack> InventoryStack {
        get {
            return readonlyInventory;
        }
    }

    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("Only one instance of Inventory is allowed.");
        }

        inventory = new List<ItemStack>(inventorySize);
        while (inventory.Count < inventorySize) {
            inventory.Add(null);
        }

        readonlyInventory = inventory.AsReadOnly();
        
        swapItemStack = null;
        swapSlotId = -1;
    }
    
    public ItemStack AddStack(ItemStack stack)
    {
        if (stack.num < 1 || stack.item == null) {
            Debug.LogError("Trying to add empty stack to inventory.");
            return null;
        }

        // First run, add to the same item stack in inventory
        foreach(ItemStack st in inventory) {
            if (st != null && st.item.itemID == stack.item.itemID) {
                while (st.num < st.item.itemMaxStack && stack.num > 0) {
                    stack.num--;
                    st.num++;
                }
            }
        }

        // Add to existed item stack complete, return null(nothing left)
        if (stack.num == 0) {
            InventoryChanged();
            return null;
        }

        int stackSplitNum = stack.num / (stack.item.itemMaxStack);;

        for (int i = 0; i < stackSplitNum; i++) {
            for (int j = 0; j < inventory.Count && (stack.num > stack.item.itemMaxStack); j++) {
                if (inventory[j] == null) {
                    inventory[j] = ItemPrefabsDefinition.StackClone(stack.item.itemID, stack.item.itemMaxStack);
                    stack.num -= stack.item.itemMaxStack;
                }
            }
        }

        // add item left to the first empty inventory slot
        for (int i = 0; i < inventory.Count; i++) {
            if (inventory[i] == null) {
                inventory[i] = stack;
                InventoryChanged();
                return null;
            }
        }

        InventoryChanged();
        //return what's left
        return stack;
    }

    public ItemStack RemoveOne(int pos)
    {
        ItemStack stack = inventory[pos];
        if (stack == null) {
            return null;        
        }

        stack.num--; //take one
        ItemStack newSt = new ItemStack() { num = 1, item = stack.item };
        if (stack.num == 0) {
            inventory[pos] = null;
        }

        InventoryChanged();
        return newSt;
    }

    public ItemStack Remove(int pos)
    {
        ItemStack stack = inventory[pos];
        inventory[pos] = null;
        InventoryChanged();
        return stack;
    }

    // This function is used after completed quest is reported to npc,
    // therefore we do not need to check whether inventory has sufficient
    // item to remove.
    public bool Remove(ItemStack stack) {
        int leftNum = stack.num;
        for (int i = 0; i < inventory.Count; i++) {
            if (inventory[i].item.itemID != stack.item.itemID) {
                continue;
            }

            if (inventory[i].num > leftNum) {
                inventory[i].num -= leftNum;
                leftNum = 0;
            } else {
                leftNum -= inventory[i].num;
                Remove(i);
            }

            if (leftNum == 0) {
                InventoryChanged();
                return true;
            }
        }

        return false;
    }

    // Same function as above
    public bool Remove(int itemID, int count) {
        int leftNum = count;
        for (int i = 0; i < inventory.Count; i++) {
            if (inventory[i].item.itemID != itemID) {
                continue;
            }
            
            if (inventory[i].num > leftNum) {
                inventory[i].num -= leftNum;
                leftNum = 0;
            } else {
                leftNum -= inventory[i].num;
                Remove(i);
            }
            
            if (leftNum == 0) {
                InventoryChanged();
                return true;
            }
        }
        
        return false;
    }

    public void UseConsumable(Item item, int slot)
    {
        RemoveOne(slot);

        switch(item.itemID) {
        case 0:
            print ("Use consumble:" + item.itemName);
            break;
        case 1:
            print ("Use consumble:" + item.itemName);
            break;
        case 2:
            print ("Use consumble:" + item.itemName);
            break;
        default:
            break;
        }
    }

    public void ReplaceSwapItemStack(int targetPos)
    {
        if (swapSlotId < 0 || swapSlotId >= inventorySize || targetPos < 0 || targetPos >= inventorySize) {
            Debug.Log("Position out of inventory range, swap fail.");
            return;
        }

        if (swapItemStack != null && swapItemStack.num > 0) {
            inventory[swapSlotId] = inventory[targetPos];
            inventory[targetPos] = swapItemStack;
            swapItemStack = null;
            swapSlotId = -1;
        }
        
        InventoryChanged();
    }

    public int GetItemCount(int itemID)
    {
        if (itemID < 0) {
            Debug.Log("Invalid itemID to get count.");
            return -1;
        }

        int num = 0;

        foreach(ItemStack stack in inventory) {
            if (stack == null) {
                continue;
            }

            if (stack.item.itemID == itemID) {
                num += stack.num;
            }
        }

        return num;
    }

    public bool InventoryContains(int id)
    {
        //TODO
        return false;
    }

    private void SaveInventory()
    {
        //TODO
    }

    private void LoadInventory()
    {
        //TODO
    }
}
