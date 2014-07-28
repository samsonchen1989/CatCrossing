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
}

[System.Serializable]
public class QuestGoal
{
	int itemID;
	int currentNumber;
	int neededNumber;
	bool complete;
}