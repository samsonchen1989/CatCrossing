using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item
{
	public string itemName;
	public int itemID;
	public string itemDesc;
	public Texture2D itemIcon;
	public int itemPower;
	public int itemSpeed;
	public ItemType itemType;

	public int itemCount;
	public int itemMaxCount;

	public enum ItemType {
		Weapon,
		Consumable,
		Quest
	}

	public Item()
	{
		itemID = -1;
	}

	public Item(string name, int id, string desc, int power, int speed, ItemType type)
	{
		itemName = name;
		itemID = id;
		itemDesc = desc;
		itemIcon = Resources.Load<Texture2D>("Item Icons/" + name);
		itemPower = power;
		itemSpeed = speed;
		itemType = type;

		itemCount = 0;
		itemMaxCount = 99;
	}

	public Item(Item preItem)
	{
		itemName = preItem.itemName;
		itemID = preItem.itemID;
		itemDesc = preItem.itemDesc;
		itemIcon = preItem.itemIcon;
		itemPower = preItem.itemPower;
		itemSpeed = preItem.itemSpeed;
		itemType = preItem.itemType;

		itemCount = 0;
		itemMaxCount = preItem.itemMaxCount;
	}

	public Item(string name, int id, string desc, int power, int speed, ItemType type, int maxCount)
		: this(name, id, desc, power, speed, type)
	{	
		itemMaxCount = maxCount;
	}

}
