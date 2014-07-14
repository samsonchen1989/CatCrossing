using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour {
	public List<Item> items = new List<Item>();

	void Start()
	{
		items.Add(new Item("Item_Grass", 0, "Cat eats grass sometimes.", 0, 0, Item.ItemType.Consumable));
		items.Add(new Item("Item_Branch", 1, "Father of fire.", 0, 0, Item.ItemType.Consumable));
		items.Add(new Item("Item_Bottle_Empty", 2, "Fill it full!.", 0, 0, Item.ItemType.Consumable, 3));
	}
}
