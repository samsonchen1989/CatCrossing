using UnityEngine;
using System.Collections;

public class QuestGoal
{
    public bool complete;
	
    public QuestGoal(bool complete = false)
    {
        this.complete = complete;
    }	

    public virtual bool CheckProgress() { return false; }

    public virtual bool DoneGoal() { return false; }
}

public class QuestGoalCollectItem : QuestGoal
{
    public int itemID;
    public int currentNumber;
    public int targetNumber;

    public QuestGoalCollectItem(int itemID, int targetNumber, int currentNumber = 0, bool complete = false) : base(complete)
    {
        this.itemID = itemID;
        this.targetNumber = targetNumber;
        this.currentNumber = currentNumber;
    }

    public override bool CheckProgress()
    {
        int inventoryNum = Inventory.Instance.GetItemCount(itemID);
        currentNumber = Mathf.Min(inventoryNum, targetNumber);

        if (currentNumber == targetNumber) {
            complete = true;
        }

        return complete;
    }

    public override string ToString()
    {
        return string.Format("{0}/{1} {2}", currentNumber, targetNumber, ItemPrefabsDefinition.GetItemName(itemID));
    }

    public override bool DoneGoal()
    {
        return Inventory.Instance.Remove(itemID, targetNumber);
    }
}

public class QuestGoalKill : QuestGoal
{
    public int enemyId;
    public int currentNumber;
    public int targetNumber;

    public QuestGoalKill(int enemyId, int targetNumber, int currentNumber = 0, bool complete = false) : base(complete)
    {
        this.enemyId = enemyId;
        this.targetNumber = targetNumber;
        this.currentNumber = currentNumber;
    }
}

public class QuestGoalVisit : QuestGoal
{
    public int targetNpcId;

    public QuestGoalVisit(int targetNpcId, bool complete = false) : base(complete)
    {
        this.targetNpcId = targetNpcId;
    }
}
