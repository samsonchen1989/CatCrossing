using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDefinitionInit : MonoBehaviour {

	Dictionary<int, Item> itemDef;

	void Start()
	{
		// Read from xml file or some other local/remote file
		itemDef = ItemPrefabsDefinition.Instance.ItemDictionary;
		itemDef.Add(0, new Item("Item_Grass", 0, "Cat eats grass sometimes.", Item.ItemType.Consumable, 20));
		itemDef.Add(1, new Item("Item_Branch", 1, "Father of fire.", Item.ItemType.Consumable, 10));
		itemDef.Add(2, new Item("Item_Bottle_Empty", 2, "Fill it full!.", Item.ItemType.Consumable, 2));
	}
}
