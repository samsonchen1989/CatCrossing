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

	// Use this for initialization
	void Start () {
		database = GameObject.FindGameObjectWithTag("Item Database").GetComponent<ItemDatabase>();
		if (!database) {
			Debug.LogError("Fail to find item database.");
			return;
		}

		for (int i= 0; i < slotsX * slotsY; i++) {
			slots.Add(new Item());
			inventory.Add(new Item());
		}

		AddItem(1);
		AddItem(0);
		print (InventoryContains(2));
		RemoveItem(1);
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
		toolTip = "";
		GUI.skin = skin;
		if (showInventory) {
			DrawInventory();
		}

		if (showToolTip) {
			GUI.Box(new Rect(Event.current.mousePosition.x + 10, Event.current.mousePosition.y - 100, 200, 100), toolTip,
			        skin.GetStyle("Box"));
		}
	}

	void DrawInventory()
	{
		int i = 0;
		for (int y = 0; y < slotsY; y++) {
			for (int x = 0; x < slotsX; x++) {
				Rect slotRect = new Rect(Screen.width / 2 - slotsX / 2 * 70 + x * 70,
				                         0.85f * Screen.height + y * 70, 64, 64);
				//string may be used to show item numbers later
				GUI.Box(slotRect, "", skin.GetStyle("Slot"));

				slots[i] = inventory[i];
				if (slots[i].itemName != null) {
					GUI.DrawTexture(slotRect, slots[i].itemIcon);
					if (slotRect.Contains(Event.current.mousePosition)) {
						toolTip = CreateTooltip(slots[i]);
						showToolTip = true;
					}
				}

				if (toolTip == "") {
					showToolTip = false;
				}

				i++;
			}
		}
	}

	string CreateTooltip(Item item)
	{
		//we can change text color depending on item rare degree
		toolTip = "<color=#ffffff>" + item.itemName + "</color>\n\n<color=#ffffff>" + item.itemDesc + "</color>";
		return toolTip;
	}

	void RemoveItem(int id)
	{
		for (int i = 0; i < inventory.Count; i++) {
			if (inventory[i].itemName != null && inventory[i].itemID == id) {
				inventory[i] = new Item();
				break;
			}
		}
	}

	void AddItem(int id)
	{
		for (int i = 0; i < inventory.Count; i++) {
			if (inventory[i].itemName == null) {
				for (int j = 0; j < database.items.Count; j++) {
					if (database.items[j].itemID == id) {
						inventory[i] = database.items[j];
						return;
					}
				}

				Debug.Log("Fail to find item with given id.");
				return;
			}
		}
	}

	bool InventoryContains(int id)
	{
		for (int i = 0; i < inventory.Count; i++) {
			if (inventory[i].itemName != null && inventory[i].itemID == id) {
				return true;
			}
		}

		return false;
	}
}
