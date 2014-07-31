using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Quest
{
	public string title;
	public int id;
	public QuestProgress progress;
	public string description;
	public string hint;
	public int nextQuestId;

	public enum QuestProgress {
		NotEligible,
		Eligible,
		Accepted,
		Complete,
		Done
	}

	List<QuestGoal> questGoalList;
	List<ItemStack> rewardList;

	public Quest(string title, int id, QuestProgress progress, string description, string hint, int nextQuestId,
	             List<QuestGoal> questGoalList, List<ItemStack> rewardList)
	{
		this.title = title;
		this.id = id;
		this.progress = progress;
		this.description = description;
		this.hint = hint;
		this.nextQuestId = nextQuestId;

		this.questGoalList = questGoalList;
		this.rewardList = rewardList;
	}
}

[System.Serializable]
public class QuestGoal
{
	public int itemID;
	public int currentNumber;
	public int neededNumber;
	public bool complete;

	public QuestGoal(int itemID, int neededNumber, int currentNumber = 0, bool complete = false)
	{
		this.itemID = itemID;
		this.neededNumber = neededNumber;

		this.currentNumber = currentNumber;
		this.complete = complete;
	}
}