using System.Collections;
using UnityEngine;

[System.Serializable]
public class Item
{
	public string itemName;
	public int itemID;
	public string itemDesc;
	public ItemType itemType;
	public int itemMaxStack;

	public Texture2D itemIcon;
	public GameObject ItemPrefab;
	
	public enum ItemType {
		None,
		Weapon,
		Resource,
		Consumable,
		Quest
	}
	
	public Item()
	{
		itemID = -1;
	}

	public Item(string name, int id, string desc, ItemType type)
	{
		itemName = name;
		itemID = id;
		itemDesc = desc;
		itemIcon = Resources.Load<Texture2D>("Item Icons/" + name);
		itemType = type;

		itemMaxStack = 99;
	}

	public Item(string name, int id, string desc, ItemType type, int maxCount)
		: this(name, id, desc, type)
	{
		itemMaxStack = maxCount;
	}

	protected Item(Item copyItem)
	{
		itemName = copyItem.itemName;
		itemID = copyItem.itemID;
		itemDesc = copyItem.itemDesc;
		itemIcon = copyItem.itemIcon;
		itemType = copyItem.itemType;

		itemMaxStack = copyItem.itemMaxStack;
	}

	public Item ShallowClone()
	{
		//return a shallow copy, as we don't need to clone the prefab and icon
		return (Item)this.MemberwiseClone();
	}

	public Item DeepClone()
	{
		return new Item(this);
	}
}

[System.Serializable]
public class ItemStack
{
	public int num;
	public Item item;
}
