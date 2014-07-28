using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

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

	public int inventorySize = 14;
	public int InventorySize {
		get {
			return inventorySize;
		}
	}

	public List<ItemStack> inventory;
	
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
	}
	
	public ItemStack AddStack(ItemStack stack)
	{
		if (stack.num < 1 || stack.item == null) {
			Debug.LogError("Trying to add empty stack to inventory.");
		}

		//first run 
		foreach(ItemStack st in inventory) {
			if (st != null && st.item.itemID == stack.item.itemID) {
				while (st.num < st.item.itemMaxStack && stack.num > 0) {
					stack.num--;
					st.num++;
				}
			}
		}

		if (stack.num == 0) {
			return null;
		}

		int stackSplitNum = stack.num / (stack.item.itemMaxStack);
		Debug.Log("stackSplitNum:" + stackSplitNum);

		for (int i = 0; i < stackSplitNum; i++) {
			for (int j = 0; j < inventory.Count && (stack.num > stack.item.itemMaxStack); j++) {
				if (inventory[j] == null) {
					inventory[j] = ItemPrefabsDefinition.StackClone(stack.item.itemID, stack.item.itemMaxStack);
					stack.num -= stack.item.itemMaxStack;
				}
			}
		}

		for (int i = 0; i < inventory.Count; i++) {
			if (inventory[i] == null) {
				inventory[i] = stack;
				return null;
			}
		}

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
		ItemStack newSt = new ItemStack() {num = 1, item = stack.item};
		if (stack.num == 0) {
			inventory[pos] = null;
		}

		return newSt;
	}

	public ItemStack Remove(int pos)
	{
		ItemStack stack = inventory[pos];
		inventory[pos] = null;
		return stack;
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
