using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ItemRecipe
{

    public string comment;
    public int craftID;
    public int itemID;
    public List<Ingredient> Ingredients = new List<Ingredient>();

    public Item Item {
        get {
            return ItemPrefabsDefinition.Instance.ItemDictionary [itemID];
        }
    }   
}

[System.Serializable]
public class Ingredient
{
    public int itemID;
    public int itemAmount;
}
