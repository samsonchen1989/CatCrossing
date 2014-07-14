using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
	public int slotsX;
	public int slotsY;
	public GUISkin skin;

	public List<Item> inventory = new List<Item>();
	public List<Item> slots = new List<Item>();
	private ItemDatabase database;

	private bool showInventory = true;
	private bool showToolTip = false;
	private string toolTip = "";

	private bool draggingItem;
	private Item draggedItem;
	private int prevIndex;

	// Use this for initialization
	void Start ()
	{
		database = GameObject.FindGameObjectWithTag("Item Database").GetComponent<ItemDatabase>();
		if (!database) {
			Debug.LogError("Fail to find item database.");
			return;
		}

		for (int i= 0; i < slotsX * slotsY; i++) {
			slots.Add(new Item());
			inventory.Add(new Item());
		}

		AddItem(0);
		AddItem(1);
		AddItem(2);
		AddItem(0);
		AddItem(2);
		AddItem(2);
		AddItem(2);
	}
	
	// Update is called once per frame
	void Update ()
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
			drawTextureAndCount(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 64, 64),
			                    draggedItem);
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

				slots[i] = inventory[i];
				GUI.Box(slotRect, "", skin.GetStyle("Slot"));
				if (slots[i].itemName != null) {
					drawTextureAndCount(slotRect, slots[i]);
					if (slotRect.Contains(currentEvent.mousePosition)) {
						if (currentEvent.button == 0 && currentEvent.type == EventType.mouseDown && !draggingItem) {
							draggingItem = true;
							prevIndex = i;
							draggedItem = slots[i];
							inventory[i] = new Item();
						}

						if (currentEvent.isMouse && currentEvent.type == EventType.mouseUp && draggingItem) {
							inventory[prevIndex] = inventory[i];
							inventory[i] = draggedItem;
							draggingItem = false;
							draggedItem = null;
						}

						if (!draggingItem) {
							toolTip = CreateTooltip(slots[i]);
							showToolTip = true;
						}

						if (currentEvent.isMouse && currentEvent.type == EventType.mouseDown
						    && currentEvent.button == 1
						    && !draggingItem)
						{
							if (slots[i].itemType == Item.ItemType.Consumable) {
								UseConsumable(slots[i], i);
							}
						}
					}
				} else {
					if (slotRect.Contains(currentEvent.mousePosition)) {
						if (currentEvent.isMouse && currentEvent.type == EventType.mouseDown && draggingItem) {
							inventory[i] = draggedItem;
							draggingItem = false;
							draggedItem = null;
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

	// Add item:
	// 1. If not in itemDatabase, just return error.
	// 2. If current inventory has item with given id and count is not over maxCount, just add count by 1.
	// 3. Otherwise, find a new empty inventory slot to put the item.
	public bool AddItem(int id)
	{
		Item resultItem = database.items.Find(
			delegate(Item item) {
				return item.itemID == id;
			}
		);

		if (resultItem == null) {
			Debug.Log("Fail to find item with given id.");
			return false;
		}

		for (int i = 0; i < inventory.Count; i++) {
			if (inventory[i].itemID == id && inventory[i].itemCount < inventory[i].itemMaxCount) {
				inventory[i].itemCount++;
				return true;
			}
		}

		for (int i = 0; i < inventory.Count; i++) {
			if (inventory[i].itemName == null) {
				inventory[i] = new Item(resultItem);
				inventory[i].itemCount = 1;
				return true;
			}
		}

		// No place to hold the item, return false.
		return false;
	}

	public bool InventoryContains(int id)
	{
		for (int i = 0; i < inventory.Count; i++) {
			if (inventory[i].itemName != null && inventory[i].itemID == id) {
				return true;
			}
		}

		return false;
	}

	private void SaveInventory()
	{
		for (int i = 0; i < inventory.Count; i++) {
			PlayerPrefs.SetInt("Inventory " + i, inventory[i].itemID);
			PlayerPrefs.SetInt("Count" + i, inventory[i].itemCount);
		}
	}

	private void LoadInventory()
	{
		for (int i = 0; i < inventory.Count; i++) {
			if (PlayerPrefs.GetInt("Inventory " + i) >= 0) {
				inventory[i] = database.items[PlayerPrefs.GetInt("Inventory " + i)];
				inventory[i].itemCount = PlayerPrefs.GetInt("Count" + i);
			} else {
				inventory[i] = new Item();
			}
		}
	}

	private void drawTextureAndCount(Rect rect, Item item)
	{
		GUI.DrawTexture(rect, item.itemIcon);
		GUI.Label(new Rect(rect.x + 50, rect.y + 45, 30, 30), item.itemCount.ToString());
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
		//we can change text color depending on item rare degree
		toolTip = "<color=#ffffff>" + item.itemName + "</color>\n\n<color=#ffffff>" + item.itemDesc + "</color>";
		return toolTip;
	}

	private void UseConsumable(Item item, int slot)
	{
		item.itemCount--;
		if (item.itemCount == 0) {
			inventory[slot] = new Item();
		}

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
}
