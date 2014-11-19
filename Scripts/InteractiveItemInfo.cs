using UnityEngine;
using System.Collections;

public class InteractiveItemInfo : MonoBehaviour
{
    // Maybe we can collect more than one item later
    public int itemID;
    public int interactiveCount;

    public int maxItemCount;
    public int minItemCount;
    private int itemCount;

    // Use this for initialization
    void Start()
    {
        if (maxItemCount < minItemCount) {
            Debug.LogError("Wrong item count range.");
        }

        if (interactiveCount <= 0) {
            Debug.LogError("Wrong interactive count.");
        }

        // Random.Range will exclusive maxItemCount, therefore add 1 here
        maxItemCount = maxItemCount + 1;

        itemCount = Random.Range(minItemCount, maxItemCount);
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    public ItemStack GetItemStack()
    {
        return ItemPrefabsDefinition.StackClone(itemID, itemCount);
    }

    public void ReduceInteractiveCount()
    {
        interactiveCount--;
        if (interactiveCount == 0) {
            DestroyImmediate(this.gameObject);
        }
    }
}
