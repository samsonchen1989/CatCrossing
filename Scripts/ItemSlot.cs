using UnityEngine;
using System.Collections;

public class ItemSlot : MonoBehaviour
{

    #region Public gameObject to drop to in Inspector

    public UISprite background;
    public UISprite icon;
    public UILabel countLabel;

    #endregion

    public ItemBackpack backPack;
    public int slotID;

    Inventory inventory;
    ItemStack slotItemStack;
    
    void Awake()
    {
        inventory = Inventory.Instance;
        if (inventory == null) {
            Debug.LogError("Fail to get inventory instance.");
            return;
        }
        
        inventory.InventoryChanged += OnInventoryChanged;
    }

    // Use this for initialization
    void Start()
    {
        slotItemStack = inventory.inventory [slotID];
        if (slotItemStack != null && icon != null && countLabel != null) {
            icon.enabled = true;
            icon.spriteName = slotItemStack.item.itemName;
            icon.MakePixelPerfect();

            countLabel.enabled = true;
            countLabel.text = slotItemStack.num.ToString();
        }
    }
	
    void OnClick()
    {
        if (inventory.swapItemStack == null) {
            if (slotItemStack != null && slotItemStack.num > 0) {
                inventory.swapItemStack = slotItemStack;
                inventory.swapSlotId = slotID;
                inventory.Remove(slotID);
                ItemCursor.UpdateSprite();
            }
        } else {
            inventory.ReplaceSwapItemStack(slotID);
            ItemCursor.UpdateSprite();
        }
    }
    
    void OnTooltip(bool show)
    {
        if (!show) {
            UITooltip.ShowText(null);
            return;
        }

        if (slotItemStack != null && slotItemStack.num > 0) {
            UITooltip.ShowText(slotItemStack.item.itemDesc);
        }
    }
    
    // If inventory changed, check and update current slotItemStack,
    // no need to update every frame.
    void OnInventoryChanged()
    {
        slotItemStack = inventory.inventory [slotID];
        if (slotItemStack != null && slotItemStack.num > 0 && icon != null && countLabel != null) {
            icon.enabled = true;
            icon.spriteName = slotItemStack.item.itemName;
            icon.MakePixelPerfect();
            
            countLabel.enabled = true;
            countLabel.text = slotItemStack.num.ToString();
        }
        
        if (slotItemStack == null) {
            icon.enabled = false;
            countLabel.enabled = false;
        }
    }
}
