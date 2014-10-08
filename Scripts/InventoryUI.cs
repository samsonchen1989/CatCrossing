using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour {
	public int slotsX = 15;
	public int slotsY;
	public int slotsRemain;
	public GUISkin skin;

	private bool showInventory = true;
	private bool showToolTip = false;
	private string toolTip = "";

	private bool draggingItem;
	private ItemStack draggedItemStack;
	private int prevIndex;

	Inventory inv;
	ItemStack currentStack;

	// Use this for initialization
	void Start()
	{
		inv = Inventory.Instance;
		if (inv == null) {
			Debug.LogError("Fail to get Inventory's single instance.");
		}

		if (slotsX >= inv.InventorySize) {
			slotsY = 1;
			slotsRemain = 0;
		} else {
			slotsY = inv.InventorySize / slotsX;
			slotsRemain = inv.InventorySize % slotsX;
		}

		// just for UI test
        /*
		ItemStack rest = inv.AddStack(ItemPrefabsDefinition.StackClone(2, 3));
		inv.AddStack(ItemPrefabsDefinition.StackClone(0, 25));
		inv.AddStack(ItemPrefabsDefinition.StackClone(1, 10));
        if (rest != null) Debug.Log("Couldn't fit " + rest.num + " x " + rest.item.itemName);
        */
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetButtonDown("Inventory")) {
			showInventory = !showInventory;
		}
	}

	void OnGUI()
	{
		GUI.skin = skin;
		toolTip = "";
		if (showInventory) {
			DrawInventory();

			if (showToolTip) {
				DrawTooltip();
			}
		}

		if (draggingItem) {
			DrawTextureAndCount(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 64, 64),
			                    draggedItemStack);
		}
	}

	void DrawInventory()
	{
		Event currentEvent = Event.current;
		int i = 0;
		for (int y = 0; y < slotsY; y++) {
			for (int x = 0; x < slotsX; x++) {
				Rect slotRect = new Rect(Screen.width / 2 - slotsX / 2 * 70 + x * 70,
				                         0.85f * Screen.height + y * 70, 64, 64);

				currentStack = inv.inventory[i];
				GUI.Box(slotRect, "", skin.GetStyle("Slot"));
				if (currentStack != null) {
					// draw item icon and stack count
					DrawTextureAndCount(slotRect, currentStack);

					// item drag and drop
					if (slotRect.Contains(currentEvent.mousePosition)) {
						// start drag
						if (currentEvent.button == 0 && currentEvent.type == EventType.mouseDown && !draggingItem) {
							draggingItem = true;
							prevIndex = i;
							draggedItemStack = currentStack;
							inv.inventory[i] = null;
						}

						// release drag
						if (currentEvent.isMouse && currentEvent.type == EventType.mouseUp && draggingItem) {
							inv.inventory[prevIndex] = currentStack;
							inv.inventory[i] = draggedItemStack;
							draggingItem = false;
							draggedItemStack = null;
						}

						// draw item tootip
						if (!draggingItem) {
							toolTip = CreateTooltip(currentStack.item);
							showToolTip = true;
						}

						// click mouse right button to use item
						if (currentEvent.isMouse && currentEvent.type == EventType.mouseDown
						    && currentEvent.button == 1
						    && !draggingItem)
						{
							if (currentStack.item.itemType == Item.ItemType.Consumable) {
								inv.UseConsumable(currentStack.item, i);
							}
						}
					}
				} else {
					// release drag on empty inventory slot
					if (slotRect.Contains(currentEvent.mousePosition)) {
						if (currentEvent.isMouse && currentEvent.type == EventType.mouseDown && draggingItem) {
							inv.inventory[i] = draggedItemStack;
							draggingItem = false;
							draggedItemStack = null;
						}
					}
				}

				if (toolTip == "") {
					showToolTip = false;
				}

				i++;
			}
		}
	}

	private void DrawTextureAndCount(Rect rect, ItemStack stack)
	{
		GUI.DrawTexture(rect, stack.item.itemIcon);
		GUI.Label(new Rect(rect.x + 50, rect.y + 45, 30, 30), stack.num.ToString());
	}

	private void DrawTooltip()
	{
		float dynamicHeight = skin.box.CalcHeight(new GUIContent(toolTip), 200);
		GUI.Box(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y - 100, 200, dynamicHeight),
		        toolTip, skin.GetStyle("Box"));
	}

	private string CreateTooltip(Item item)
	{
		toolTip = "";
		// we can change text color depending on item rare degree
		toolTip = "<color=#ffffff>" + item.itemName + "</color>\n\n<color=#ffffff>" + item.itemDesc + "</color>";
		return toolTip;
	}
}